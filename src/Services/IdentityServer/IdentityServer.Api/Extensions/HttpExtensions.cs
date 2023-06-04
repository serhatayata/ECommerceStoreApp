using IdentityServer.Api.Handlers.ApiTokenHandlers;

namespace IdentityServer.Api.Extensions
{
    public static class HttpExtensions
    {
        public static void AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<LocalizationRequestTokenHandler>();

            #region HttpClients
            
            services.AddHttpClient("communications-send-smtp", config =>
            {
                config.BaseAddress = new Uri(uriSendSmtp);
            }).AddHttpMessageHandler<LocalizationRequestTokenHandler>();

            #endregion
        }
    }
}
