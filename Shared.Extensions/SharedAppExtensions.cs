using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Extensions;

public static class SharedAppExtensions
{
    public static void AddApplicationMiddleware(this WebApplication app, List<string> allowedOrigins = null)
    {
        //Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCorsMiddleware(allowedOrigins);

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/healthcheck");

        app.Run();
    }

    private static void UseCorsMiddleware(this WebApplication app, List<string> allowedOrigins = null)
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

    private static void UseAuthMiddleware(this WebApplication app, string authConfigString)
    {
        if (!string.IsNullOrEmpty(authConfigString))
        {
            app.UseAuthentication();

            app.UseAuthorization();
        }
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

