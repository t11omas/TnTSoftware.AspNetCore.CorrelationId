using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TnTSoftware.AspNetCore.CorrelationId
{
    internal class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public static string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public static string CorrelationIdHeaderName { get; set; } = HeaderNames.CorrelationId;

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId;
            if (!context.TryGetHeaderValue(CorrelationIdHeaderName, out correlationId))
            {
                if (context.TryGetHeaderValue(HeaderNames.RequestId, out correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                }
            }


            context.SetCorrelationId(correlationId);

            var logger = context.RequestServices.GetRequiredService<ILogger<CorrelationIdMiddleware>>();
            using (logger.BeginScope("{@ServiceName} {@SessionId}", ApplicationName, correlationId))
            {
                await this._next(context);
            }
        }
    }
}