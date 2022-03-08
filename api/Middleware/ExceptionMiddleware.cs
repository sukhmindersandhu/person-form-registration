using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            this.next = next;
            this.logger = logger?.CreateLogger("API.Middleware.Exception");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                string error = ex?.InnerException?.Message ?? ex.Message;
                logger?.LogError(error);

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await HandleExceptionAsync(httpContext, error);

                return;
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string error)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.WriteAsync(error);
            return Task.CompletedTask;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}