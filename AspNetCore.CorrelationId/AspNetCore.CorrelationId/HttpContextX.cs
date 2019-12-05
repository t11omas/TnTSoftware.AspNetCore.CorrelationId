using Microsoft.AspNetCore.Http;

namespace TnTSoftware.AspNetCore.CorrelationId
{
    public static class HttpContextX
    {
        public static string GetCorrelationId(this HttpContext context)
        {
            return (string)context.Items[CorrelationIdMiddleware.CorrelationIdHeaderName];
        }

        internal static void SetCorrelationId(this HttpContext context, string correlationId)
        {
            context.Response.Headers.Add(CorrelationIdMiddleware.CorrelationIdHeaderName, correlationId);
            context.Items[CorrelationIdMiddleware.CorrelationIdHeaderName] = correlationId;
        }

        internal static bool TryGetHeaderValue(this HttpContext context, string headerName, out string value)
        {
            var header = context.Request.Headers[headerName];

            if (header.Count > 0)
            {
                value = header[0];
                return !string.IsNullOrEmpty(value);
            }

            value = string.Empty;
            return false;
        }
    }
}
