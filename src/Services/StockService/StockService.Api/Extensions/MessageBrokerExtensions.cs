using System.Reflection;
using System.Text.RegularExpressions;

namespace StockService.Api.Extensions;

public static class MessageBrokerExtensions
{
    public static string GetQueueName<T>()
    {
        var value = Regex.Replace(typeof(T).Name, "([A-Z])", "-$1").ToLowerInvariant();
        return value.Substring(1, value.Length - 1);
    }

    public static string GetQueueNameWithProject<T>()
    {
        var value = Assembly.GetCallingAssembly().GetName().Name.ToLowerInvariant() + 
                    Regex.Replace(typeof(T).Name, "([A-Z])", "-$1").ToLowerInvariant();
        return value;
    }
}
