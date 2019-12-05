using System;
using System.Collections.Generic;
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
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public static string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public static string CorrelationIdHeaderName { get; set; } = HeaderNames.CorrelationId;

        public async Task InvokeAsync(HttpContext context)
        {
            var properties = new Dictionary<string, object>
            {
                [HeaderNames.ApplicationName] = ApplicationName
            };
            using (this._logger.BeginScope(properties))
            {
                string correlationId;
                if (!context.TryGetHeaderValue(CorrelationIdHeaderName, out correlationId))
                {
                    if (!context.TryGetHeaderValue(HeaderNames.RequestId, out correlationId))
                    {
                        correlationId = Guid.NewGuid().ToString();
                        this._logger.GeneratedNewCorrelationId(correlationId);
                    }
                    else
                    {
                        this._logger.UsingRequestIdCorrelationId(correlationId);
                    }
                }
                else
                {
                    this._logger.UsingCustomHeaderCorrelationId(correlationId);
                }


                context.SetCorrelationId(correlationId);

                properties = new Dictionary<string, object>
                {
                    [CorrelationIdHeaderName] = correlationId
                };
                using (this._logger.BeginScope(properties))
                {
                    await this._next(context);
                }
            }
        }
    }
}