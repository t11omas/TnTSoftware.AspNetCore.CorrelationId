using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TnTSoftware.AspNetCore.CorrelationId
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = httpContextAccessor.HttpContext.GetCorrelationId();

            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                request.Headers.Add(CorrelationIdMiddleware.CorrelationIdHeaderName, correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}