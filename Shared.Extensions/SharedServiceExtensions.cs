﻿using FluentValidation.AspNetCore;
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

    public static void AddServices<TContextService, TContextImplementation, TValidator>(
        this WebApplicationBuilder builder,
        AddDependencyInjection addDependencyInjection,
        DatabaseConfiguration? databaseConfiguration)
        where TContextService : class
        where TContextImplementation : DbContext, TContextService
        where TValidator : class
    {
        builder.Services.AddDatabaseConfiguration<TContextService, TContextImplementation>(
            configuration: builder.Configuration,
            databaseConfiguration: databaseConfiguration);

        builder.Services.AddBearerAuthentication(builder.Configuration);

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
