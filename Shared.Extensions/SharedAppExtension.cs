using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Shared.Extensions;

public static class SharedAppExtension
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
}

