using System.Text.Json;

namespace MonitoringService.Api.Extensions;

public static class HttpClientExtensions
{
    public async static Task<TResult?> PostGetResponseAsync<TResult, TValue>(
        this HttpClient client, 
        string url, 
        TValue value, 
        CancellationToken cancellationToken)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var httpRes = await client.PostAsJsonAsync(requestUri: url, 
                                                   value: value, 
                                                   cancellationToken: cancellationToken);

        return httpRes.IsSuccessStatusCode ? await httpRes.Content.ReadFromJsonAsync<TResult>(options: jsonSerializerOptions, 
                                                                                              cancellationToken: cancellationToken) : default;
    }

    public async static Task PostAsync<TValue>(
        this HttpClient client, 
        string url, 
        TValue value,
        CancellationToken cancellationToken)
    {
        await client.PostAsJsonAsync(requestUri: url, 
                                     value: value,
                                     cancellationToken: cancellationToken);
    }

    public async static Task<T?> GetResponseAsync<T>(
        this HttpClient client, 
        string url,
        CancellationToken cancellationToken)
    {
        return await client.GetFromJsonAsync<T>(requestUri: url, cancellationToken: cancellationToken);
    }
}
