using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shared.Extensions;

public static class SharedDependencyInjectionExtensions
{
    public static void AddDatabaseConfiguration<TContextService, TContextImplementation>(
        this IServiceCollection services,
        IConfiguration configuration,
        DatabaseConfiguration? databaseConfiguration)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
    {
        if (databaseConfiguration?.Configuration is not null && databaseConfiguration?.Provider is null)
            throw new ArgumentNullException(nameof(databaseConfiguration.Provider));

        if (databaseConfiguration?.Configuration is null && databaseConfiguration?.Provider is not null)
            throw new ArgumentNullException(nameof(databaseConfiguration.Configuration));

        switch (databaseConfiguration?.Provider)
        {
            case RelationalDatabaseProvider.MicrosoftSQLServer:
                services.AddDbContext<TContextService, TContextImplementation>(
                     options => options.UseSqlServer(configuration.GetConnectionString(databaseConfiguration.Configuration))
                     .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            case RelationalDatabaseProvider.MySQL:
                services.AddDbContext<TContextService, TContextImplementation>(
                    options => options.UseMySql(configuration.GetConnectionString(databaseConfiguration.Configuration))
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            case RelationalDatabaseProvider.PostgreSQL:
                services.AddDbContext<TContextService, TContextImplementation>(
                    options => options.UseNpgsql(configuration.GetConnectionString(databaseConfiguration.Configuration))
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            default:
                services.AddDbContext<TContextService, TContextImplementation>(
                    options => options.UseInMemoryDatabase(string.Empty));
                break;
        }
    }

    /// <summary>
    /// TODO - Improve !
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("Authentication:AccessTokenSecret").Value);

            // TODO
            //opt.TokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = TOADD,
            //    ValidAudience = TOADD,
            //    IssuerSigningKey = key,
            //};
        });
    }
}

