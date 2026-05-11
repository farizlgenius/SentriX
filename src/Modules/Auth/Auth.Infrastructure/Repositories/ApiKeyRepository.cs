using System;
using Auth.Application.Interfaces;
using Auth.Infrastructure.Persistence;

namespace Auth.Infrastructure.Repositories;

public sealed class ApiKeyRepository(AuthDbContext context) : IApiKeyRepository
{
      public Task<bool> ValidateApiKeyAsync(string apiKey)
      {
            throw new NotImplementedException();
      }
}
