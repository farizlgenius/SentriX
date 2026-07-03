using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Adapter.Abstraction.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Adapter.Amico.Services;

public sealed class HttpClientService(IHttpClientFactory factory) : IHttpClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task<TResponse?> SendAsync<TRequest, TResponse>(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        TRequest? request = default,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);

        var url = endpoint;

        if (queryParams?.Any() == true)
        {
            url = QueryHelpers.AddQueryString(endpoint, queryParams);
        }

        var message = new HttpRequestMessage(method, url);

        if (request != null)
        {
            var requestJson = JsonSerializer.Serialize(request, JsonOptions);

            Console.WriteLine("========== HTTP REQUEST ==========");
            Console.WriteLine($"{method} {baseUrl}{url}");
            Console.WriteLine(requestJson);
            Console.WriteLine("==================================");

            message.Content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                message.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }

        var response = await client.SendAsync(message, ct);

        var responseText = await response.Content.ReadAsStringAsync(ct);

        Console.WriteLine("========== HTTP RESPONSE ==========");
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine(responseText);
        Console.WriteLine("===================================");

        response.EnsureSuccessStatusCode();

        if (string.IsNullOrWhiteSpace(responseText))
            return default;

        return JsonSerializer.Deserialize<TResponse>(
            responseText,
            JsonOptions);
    }

    public async Task<TResponse?> SendAsync<TResponse>(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        var client = factory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);

        var url = endpoint;

        if (queryParams?.Any() == true)
        {
            url = QueryHelpers.AddQueryString(endpoint, queryParams);
        }

        var message = new HttpRequestMessage(method, url);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                message.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }

        Console.WriteLine("========== HTTP REQUEST ==========");
        Console.WriteLine($"{method} {baseUrl}{url}");
        Console.WriteLine("==================================");

        var response = await client.SendAsync(message, ct);

        var responseText = await response.Content.ReadAsStringAsync(ct);

        Console.WriteLine("========== HTTP RESPONSE ==========");
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        Console.WriteLine(responseText);
        Console.WriteLine("===================================");

        response.EnsureSuccessStatusCode();

        if (string.IsNullOrWhiteSpace(responseText))
            return default;

        return JsonSerializer.Deserialize<TResponse>(
            responseText,
            JsonOptions);
    }
}