using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shared.Extensions;

public static class SharedDependencyInjectionExtensions
{
    public static void AddDatabaseConfiguration<TContextService, TContextImplementation, TProvider>(
        this IServiceCollection services,
        IConfiguration configuration,
        DatabaseConfiguration<TProvider>? databaseConfiguration)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
        where TProvider : Enum
    {
        if (databaseConfiguration is null)
        {

            services.AddDbContext<TContextService, TContextImplementation>(
                options => options.UseInMemoryDatabase(string.Empty));

            return;
        }

        switch (databaseConfiguration.Provider)
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

            case NonRelationalDatabaseProvider.MongoDB:
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
    public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration, AuthConfiguration? authConfiguration)
    {
        if (authConfiguration is null)
            return;

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(opt =>
        {
            //var key = Encoding.ASCII.GetBytes(configuration.GetSection("Authentication:AccessTokenSecret").Value);

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

