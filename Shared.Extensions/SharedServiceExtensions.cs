using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Shared.Extensions;

public static class SharedServiceExtensions
{
    public delegate void AddDependencyInjection(
        IServiceCollection services,
        IConfiguration configuration);

    /// <summary>
    /// AddServices extension method for relational database provider type
    /// </summary>
    /// <typeparam name="TContextService"></typeparam>
    /// <typeparam name="TContextImplementation"></typeparam>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="builder"></param>
    /// <param name="addDependencyInjection"></param>
    /// <param name="databaseConfiguration"></param>
    /// <param name="authConfiguration"></param>
    public static void AddServices<TContextService, TContextImplementation, TProvider, TValidator>(
        this WebApplicationBuilder builder,
        AddDependencyInjection addDependencyInjection,
        DatabaseConfiguration<TProvider>? databaseConfiguration,
        AuthConfiguration? authConfiguration)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
        where TProvider : Enum
        where TValidator : class
    {
        builder.Services.AddDatabaseConfiguration<TContextService, TContextImplementation, TProvider>(
            configuration: builder.Configuration,
            databaseConfiguration: databaseConfiguration);

        builder.Services.AddBearerAuthentication(builder.Configuration, authConfiguration);

        addDependencyInjection(builder.Services, builder.Configuration);

        builder.Services.AddControllers()
                        .AddNewtonsoftJson()
                        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                        .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TValidator>());

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks();
    }

    /// <summary>
    /// AddServices extension method for relational database provider type
    /// </summary>
    /// <typeparam name="TContextImplementation"></typeparam>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="builder"></param>
    /// <param name="addDependencyInjection"></param>
    /// <param name="databaseConfiguration"></param>
    /// <param name="authConfiguration"></param>
    public static void AddServices<TContextImplementation, TProvider, TValidator>(
        this WebApplicationBuilder builder,
        AddDependencyInjection addDependencyInjection,
        DatabaseConfiguration<TProvider> databaseConfiguration,
        AuthConfiguration? authConfiguration)
        where TContextImplementation : class
        where TProvider : Enum
        where TValidator : class
    {
        builder.Services.AddDatabaseConfiguration<TContextImplementation, TProvider>(
            configuration: builder.Configuration,
            databaseConfiguration: databaseConfiguration);

        builder.Services.AddBearerAuthentication(builder.Configuration, authConfiguration);

        addDependencyInjection(builder.Services, builder.Configuration);

        builder.Services.AddControllers()
                        .AddNewtonsoftJson()
                        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                        .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TValidator>());

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks();
    }
}

