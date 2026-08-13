using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Exceptions;
using SharedKernel.Interfaces;

namespace SharedKernel.Services;

public sealed class HttpClientService(IHttpClientFactory factory) : IHttpClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };


    public async Task<TResponse> SendAsync<TRequest, TResponse>(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        TRequest? request = default,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        using var message = CreateRequest(
            method,
            baseUrl,
            endpoint,
            headers,
            queryParams);

        if (request != null)
        {
            var requestJson = JsonSerializer.Serialize(
                request,
                JsonOptions);

            Console.WriteLine("========== HTTP REQUEST ==========");
            Console.WriteLine(requestJson);
            Console.WriteLine("==================================");

            message.Content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
        }


        using var response = await SendRequestAsync(
            baseUrl,
            message,
            ct);


        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength == 0)
            throw new InvalidOperationException(
            $"The HTTP response from '{endpoint}' was empty, but a non-null payload of type '{typeof(TResponse).Name}' was expected.");


        return await response.Content.ReadFromJsonAsync<TResponse>(
            JsonOptions,
            ct) ?? throw new InvalidOperationException($"Failed to deserialize HTTP response from '{endpoint}' into a non-null instance of type '{typeof(TResponse).Name}'.");
    }


    public async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        using var message = CreateRequest(
            method,
            baseUrl,
            endpoint,
            headers,
            queryParams);


        using var response = await SendRequestAsync(
            baseUrl,
            message,
            ct);


        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength == 0)
            throw new InvalidOperationException(
            $"The HTTP response from '{endpoint}' was empty, but a non-null payload of type '{typeof(TResponse).Name}' was expected.");


        return await response.Content.ReadFromJsonAsync<TResponse>(
            JsonOptions,
            ct) ?? throw new InvalidOperationException($"Failed to deserialize HTTP response from '{endpoint}' into a non-null instance of type '{typeof(TResponse).Name}'.");
    }


    // ==============================
    // Binary Stream Response
    // ==============================

    public async Task<Stream> SendStreamAsync(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        var message = CreateRequest(
            method,
            baseUrl,
            endpoint,
            headers,
            queryParams);


        var response = await factory
            .CreateClient()
            .SendAsync(
                message,
                HttpCompletionOption.ResponseHeadersRead,
                ct);


        response.EnsureSuccessStatusCode();


        return await response.Content.ReadAsStreamAsync(ct);
    }


    // ==============================
    // Binary Byte Response
    // ==============================

    public async Task<byte[]> SendBytesAsync(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string?>? queryParams = null,
        CancellationToken ct = default)
    {
        using var message = CreateRequest(
            method,
            baseUrl,
            endpoint,
            headers,
            queryParams);


        using var response = await factory
            .CreateClient()
            .SendAsync(message, ct);


        response.EnsureSuccessStatusCode();


        return await response.Content.ReadAsByteArrayAsync(ct);
    }


    private HttpRequestMessage CreateRequest(
        HttpMethod method,
        string baseUrl,
        string endpoint,
        Dictionary<string, string>? headers,
        Dictionary<string, string?>? queryParams)
    {
        var url = endpoint;

        if (queryParams?.Any() == true)
        {
            url = QueryHelpers.AddQueryString(
                endpoint,
                queryParams);
        }


        var message = new HttpRequestMessage(
            method,
            new Uri(new Uri(baseUrl), url));


        if (headers != null)
        {
            foreach (var header in headers)
            {
                message.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }


        return message;
    }


    private async Task<HttpResponseMessage> SendRequestAsync(
        string baseUrl,
        HttpRequestMessage message,
        CancellationToken ct)
    {
        Console.WriteLine("========== HTTP REQUEST ==========");
        Console.WriteLine($"{message.Method} {message.RequestUri}");
        Console.WriteLine("==================================");


        var client = factory.CreateClient();


        return await client.SendAsync(
            message,
            ct);
    }

    public async Task<Stream> SendStreamAsync<TRequest>(
  HttpMethod method,
  string baseUrl,
  string endpoint,
  TRequest? request = default,
  Dictionary<string, string>? headers = null,
  Dictionary<string, string?>? queryParams = null,
  CancellationToken ct = default)
    {
        var message = CreateRequest(
            method,
            baseUrl,
            endpoint,
            headers,
            queryParams);


        if (request != null)
        {
            var requestJson = JsonSerializer.Serialize(
                request,
                JsonOptions);

            message.Content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
        }


        var response = await factory
            .CreateClient()
            .SendAsync(
                message,
                HttpCompletionOption.ResponseHeadersRead,
                ct);


        response.EnsureSuccessStatusCode();


        return await response.Content.ReadAsStreamAsync(ct);
    }
}