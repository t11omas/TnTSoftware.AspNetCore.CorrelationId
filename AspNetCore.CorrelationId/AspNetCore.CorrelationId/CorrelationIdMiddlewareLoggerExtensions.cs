using System;
using Microsoft.Extensions.Logging;

namespace TnTSoftware.AspNetCore.CorrelationId
{
    internal static class CorrelationIdMiddlewareLoggerExtensions
    {
        private static readonly string _correlationIdMessageTemplate = string.Concat("{", CorrelationIdMiddleware.CorrelationIdHeaderName, "}");

        private static readonly Action<ILogger, string, Exception> _generatedNewCorrelationId =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(400, "GenerateNewCorrelationId"),
                string.Concat("Generated new CorrelationId ", _correlationIdMessageTemplate));

        private static readonly Action<ILogger, string, Exception> _usingRequestIdCorrelationId = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(401, "UsingRequestIdCorrelationId"), string.Concat("Using CorrelationId from [Request-Id] header: ", _correlationIdMessageTemplate));
        private static readonly Action<ILogger, string, Exception> _usingCustomHeaderCorrelationId = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(402, "UsingCustomHeaderCorrelationId"), string.Concat($"Using CorrelationId from {CorrelationIdMiddleware.CorrelationIdHeaderName} header: ", _correlationIdMessageTemplate));

        public static void GeneratedNewCorrelationId(this ILogger logger, string correlationId)
        {
            _generatedNewCorrelationId(logger, correlationId, null);
        }

        public static void UsingRequestIdCorrelationId(this ILogger logger, string correlationId)
        {
            _usingRequestIdCorrelationId(logger, correlationId, null);
        }

        public static void UsingCustomHeaderCorrelationId(this ILogger logger, string correlationId)
        {
            _usingCustomHeaderCorrelationId(logger, correlationId, null);
        }
    }
}