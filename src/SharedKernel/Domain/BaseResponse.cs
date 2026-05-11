using System.Net;

namespace SharedKernel.Domain;

public record BaseResponse(HttpStatusCode Code,string Message,DateTime Timestamp);
