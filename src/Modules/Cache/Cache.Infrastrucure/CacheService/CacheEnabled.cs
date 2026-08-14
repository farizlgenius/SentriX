using System;
using System.Text.Json;
using Cache.Contract.Interfaces;
using StackExchange.Redis;

namespace Cache.Infrastructure.CacheService;

public class CacheEnabled(IConnectionMultiplexer redis) : ICache
{
      public async Task<bool> DeleteAsync(string key)
      {
            var db = redis.GetDatabase();
            return await db.KeyDeleteAsync(key);
      }

      public async Task<T?> GetAsync<T>(string key)
      {
            var db = redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (!value.HasValue)
                  return default;


            return JsonSerializer.Deserialize<T>(value.ToString()!);
      }

      public async Task<bool> KeyExistsAsync(string key)
      {
            var db = redis.GetDatabase();
            return await db.KeyExistsAsync(key);
      }

      public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
      {
            var db = redis.GetDatabase();
            var json = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, json, expiry);
      }
}
