using System.Net;
using SharedKernel.Model;

namespace SharedKernel.Domain;

public record BaseResponse( 
      DateTime Timestamp,
      HttpStatusCode Code,
      bool Success,
      string? Message
      );
public record BaseResponse<TData>(
      DateTime Timestamp,
      HttpStatusCode Code,
      bool Success,
      string? Message,
      TData? Data = default,
      BaseErrorResponse? Errors = default
      ) : BaseResponse(Timestamp,Code,Success,Message);


public record ValidateBaseResponse<TData>(
      DateTime Timestamp,
      HttpStatusCode Code,
      bool Success,
      string? Message,
      TData? Data = default,
      Dictionary<string,string[]>? Errors = default
      ) : BaseResponse(Timestamp,Code,Success,Message);

