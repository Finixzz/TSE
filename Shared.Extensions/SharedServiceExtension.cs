using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Shared.Extensions;

public static class SharedServiceExtension
{
    public delegate void AddDependencyInjection(
        IServiceCollection services,
        IConfiguration configuration);

    public static void AddServices<TContextService, TContextImplementation, TValidator>(
        this WebApplicationBuilder builder,
        string configurationString,
        AddDependencyInjection addDependencyInjection,
        RelationalDatabaseProvider? databaseProvider = null,
        string connectionString = null)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
        where TValidator : class
    {
        IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile(configurationString)
                            .Build();

        builder.Services.AddDatabaseConfiguration<TContextService, TContextImplementation>(
            configuration: configuration,
            databaseProvider: databaseProvider,
            connectionString: connectionString);

        builder.Services.AddBearerAuthentication(configuration);

        addDependencyInjection(builder.Services, configuration);

        builder.Services.AddControllers()
                        .AddNewtonsoftJson()
                        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                        .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TValidator>());

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks();
    }
}

