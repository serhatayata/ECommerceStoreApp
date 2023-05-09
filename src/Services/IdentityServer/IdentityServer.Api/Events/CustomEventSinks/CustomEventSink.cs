using IdentityServer4.Events;
using IdentityServer4.Services;
using Serilog;

namespace IdentityServer.Api.Events.CustomEventSinks
{
    public class CustomEventSink : IEventSink
    {
        private readonly ILogger<CustomEventSink> _logger;

        public CustomEventSink(ILogger<CustomEventSink> logger)
        {
            _logger = logger;
        }

        public Task PersistAsync(Event evt)
        {
            if (evt.EventType == EventTypes.Success ||
                evt.EventType == EventTypes.Information)
            {
                _logger.LogInformation("{Name} ({Id}), Details: {@details}",
                    evt.Name,
                    evt.Id,
                    evt);
            }
            else
            {
                _logger.LogError("{Name} ({Id}), Details: {@details}",
                    evt.Name,
                    evt.Id,
                    evt);
            }

            return Task.CompletedTask;
        }
    }
}
