using Microsoft.AspNetCore.Builder;

namespace TnTSoftware.AspNetCore.CorrelationId
{
    public static class ApplicationBuilderX
    {
        public static void UseCorrelationId(this IApplicationBuilder builder, string correlationIdHeaderName = null,
            string applicationName = null)
        {
            if(!string.IsNullOrEmpty(correlationIdHeaderName))
            {
                CorrelationIdMiddleware.CorrelationIdHeaderName = correlationIdHeaderName;
            }

            if (!string.IsNullOrEmpty(applicationName))
            {
                CorrelationIdMiddleware.ApplicationName = applicationName;
            }

            builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}