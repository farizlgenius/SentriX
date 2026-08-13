namespace SharedKernel.Interfaces;

public interface IHttpClient
{
      Task<TResponse> SendAsync<TRequest, TResponse>(
            HttpMethod method,
            string baseUrl,
            string endpoint,
            TRequest? request = default,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string?>? queryParams = null,
            CancellationToken ct = default);

      Task<TResponse> SendAsync<TResponse>(
      HttpMethod method,
      string baseUrl,
      string endpoint,
      Dictionary<string, string>? headers = null,
      Dictionary<string, string?>? queryParams = null,
      CancellationToken cancellationToken = default);

      Task<byte[]> SendBytesAsync(
      HttpMethod method,
      string baseUrl,
      string endpoint,
      Dictionary<string, string>? headers = null,
      Dictionary<string, string?>? queryParams = null,
      CancellationToken ct = default);

      Task<Stream> SendStreamAsync(
      HttpMethod method,
      string baseUrl,
      string endpoint,
      Dictionary<string, string>? headers = null,
      Dictionary<string, string?>? queryParams = null,
      CancellationToken ct = default);

      Task<Stream> SendStreamAsync<TRequest>(
     HttpMethod method,
     string baseUrl,
     string endpoint,
     TRequest? request = default,
     Dictionary<string, string>? headers = null,
     Dictionary<string, string?>? queryParams = null,
     CancellationToken ct = default);

}