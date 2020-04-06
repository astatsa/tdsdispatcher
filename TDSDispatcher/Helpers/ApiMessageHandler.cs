using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TDSDispatcher.Helpers
{
    class ApiMessageHandler : DelegatingHandler
    {
        private readonly SessionContext sessionContext;
        public ApiMessageHandler(HttpMessageHandler innerHandler, SessionContext sessionContext)
        {
            this.sessionContext = sessionContext;
            InnerHandler = innerHandler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = sessionContext?.Token;
            if (!String.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
