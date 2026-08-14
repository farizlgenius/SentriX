using System;
using Cache.Contract.Interfaces;

namespace Cache.Infrastructure.CacheService;

public sealed class CacheDisabled : ICache
{
      public Task<bool> DeleteAsync(string key) => Task.FromResult<bool>(default);

      public Task<T?> GetAsync<T>(string key) => Task.FromResult<T?>(default);

      public Task<bool> KeyExistsAsync(string key) => Task.FromResult<bool>(default);

      public Task SetAsync<T>(string key, T value, TimeSpan expiry) => Task.CompletedTask;
}
