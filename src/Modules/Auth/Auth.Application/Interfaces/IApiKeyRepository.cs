using System;

namespace Auth.Application.Interfaces;

public interface IApiKeyRepository
{
      Task<bool> ValidateApiKeyAsync(string apiKey);
}
