using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Extensions;

public static class SharedAppExtensions
{
    public static void AddApplicationMiddleware(this WebApplication app,
                                                AuthConfiguration? authConfiguration,
                                                List<string>? allowedOrigins)
    {
        //Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCorsMiddleware(allowedOrigins);

        app.UseHttpsRedirection();

        app.UseAuthMiddleware(authConfiguration);

        app.MapControllers();

        app.MapHealthChecks("/healthcheck");

        app.Run();
    }

    private static void UseCorsMiddleware(this WebApplication app, List<string>? allowedOrigins)
    {
        if (allowedOrigins is null)
        {
            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        }
        else
        {
            app.UseCors(builder => builder
            .WithOrigins(allowedOrigins.ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader());
        }
    }

    private static void UseAuthMiddleware(this WebApplication app, AuthConfiguration? authConfiguration)
    {
        if (authConfiguration is null || authConfiguration.AuthenticationType == AuthenticationType.None)
            return;

        app.UseAuthentication();

        app.UseAuthorization();
    }

    public static void AddDefaultLogging(this WebApplicationBuilder builder,
                                         string loggingConfiguration)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection(loggingConfiguration));
        builder.Logging.AddDebug();
        builder.Logging.AddConsole();
    }
}

