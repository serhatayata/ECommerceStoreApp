using IdentityServer.Api.Handlers.ApiTokenHandlers;

namespace IdentityServer.Api.Extensions
{
    public static class HttpExtensions
    {
        public static void AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            services.AddScoped<LocalizationRequestTokenHandler>();

            #region HttpClients
            string gatewayClient = configuration.GetSection($"SourceOriginSettings:{env}:Gateway").Value;

            services.AddHttpClient("gateway", config =>
            {
                var baseAddress = $"{gatewayClient}";
                config.BaseAddress = new Uri(baseAddress);
            }).AddHttpMessageHandler<LocalizationRequestTokenHandler>();

            #endregion
        }
    }
}
