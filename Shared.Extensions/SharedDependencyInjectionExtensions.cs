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
        RelationalDatabaseProvider? databaseProvider = null,
        string connectionString = null)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
    {
        if (connectionString is not null && databaseProvider is null)
            throw new ArgumentNullException(nameof(databaseProvider));

        if (connectionString is null && databaseProvider is not null)
            throw new ArgumentNullException(nameof(connectionString));

        switch (databaseProvider)
        {
            case RelationalDatabaseProvider.MicrosoftSQLServer:
                services.AddDbContext<TContextService, TContextImplementation>(
                     options => options.UseSqlServer(configuration.GetConnectionString(connectionString))
                     .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            case RelationalDatabaseProvider.MySQL:
                services.AddDbContext<TContextService, TContextImplementation>(
                    options => options.UseMySql(configuration.GetConnectionString(connectionString))
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            case RelationalDatabaseProvider.PostgreSQL:
                services.AddDbContext<TContextService, TContextImplementation>(
                    options => options.UseNpgsql(configuration.GetConnectionString(connectionString))
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                break;

            default:
                services.AddDbContextPool<TContextService, TContextImplementation>(
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

            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
}

