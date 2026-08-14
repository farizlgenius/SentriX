using System.Net;

namespace SharedKernel.Exceptions;

public sealed class ApiHttpException : HttpRequestException
{
  public HttpMethod Method { get; }
  public Uri? Endpoint { get; }
  public HttpStatusCode StatusCode { get; }
  public string? ReasonPhrase { get; }
  public string? ResponseBody { get; }

  public ApiHttpException(
      HttpMethod method,
      Uri? endpoint,
      HttpStatusCode statusCode,
      string? reasonPhrase,
      string? responseBody)
      : base(
          CreateMessage(
              method,
              endpoint,
              statusCode,
              reasonPhrase,
              responseBody),
          null,
          statusCode)
  {
    Method = method;
    Endpoint = endpoint;
    StatusCode = statusCode;
    ReasonPhrase = reasonPhrase;
    ResponseBody = responseBody;
  }

  private static string CreateMessage(
      HttpMethod method,
      Uri? endpoint,
      HttpStatusCode statusCode,
      string? reasonPhrase,
      string? responseBody)
  {
    return $"""
            HTTP request failed.
            Method      : {method}
            Endpoint    : {endpoint}
            Status Code : {(int)statusCode} {reasonPhrase}
            Response    : {responseBody}
            """;
  }
}