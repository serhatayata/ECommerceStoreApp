namespace Web.ApiGateway.Handlers
{
    public class HttpClientDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpClientDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                if (request.Headers.Contains("Authorization"))
                    request.Headers.Remove("Authorization");

                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
