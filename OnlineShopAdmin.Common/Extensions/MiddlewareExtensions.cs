using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace OnlineShopAdmin.Common.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = MediaTypeNames.Text.Plain;

                var error = context.Features.Get<IExceptionHandlerPathFeature>();
                if (error == null) return;

                logger.Error(error.Error.StackTrace);

                await context.Response.WriteAsync(error.Error.Message);
            });
        });
    }
}