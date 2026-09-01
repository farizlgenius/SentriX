using Microsoft.AspNetCore.Http;

namespace Core.Contract.DTOs.User;

public sealed record UploadImageDto(IFormFile Image);