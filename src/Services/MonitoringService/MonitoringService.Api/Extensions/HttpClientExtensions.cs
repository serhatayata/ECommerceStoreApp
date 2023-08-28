using System.Text.Json;

namespace MonitoringService.Api.Extensions;

public static class HttpClientExtensions
{
    public async static Task<TResult?> PostGetResponseAsync<TResult, TValue>(this HttpClient Client, string Url, TValue Value)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var httpRes = await Client.PostAsJsonAsync(Url, Value);

        return httpRes.IsSuccessStatusCode ? await httpRes.Content.ReadFromJsonAsync<TResult>(options: jsonSerializerOptions) : default;
    }

    public async static Task PostAsync<TValue>(this HttpClient Client, string Url, TValue Value)
    {
        await Client.PostAsJsonAsync(Url, Value);
    }

    public async static Task<T?> GetResponseAsync<T>(this HttpClient Client, string Url)
    {
        return await Client.GetFromJsonAsync<T>(Url);
    }
}
