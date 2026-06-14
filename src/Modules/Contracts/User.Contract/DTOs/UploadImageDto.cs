using Microsoft.AspNetCore.Http;

namespace User.Contract.DTOs;

public sealed record UploadImageDto(IFormFile Image);