using System;
using System.Text.Json;
using Cache.Contract.Interfaces;
using StackExchange.Redis;

namespace Cache.Infrastructure.CacheService;

public class CacheEnabled(IConnectionMultiplexer redis) : ICache
{
      public async Task<T?> GetAsync<T>(string key)
      {
            try
            {
                  var db = redis.GetDatabase();
                  var value = await db.StringGetAsync(key);

                  if (!value.HasValue)
                        return default;


                  return JsonSerializer.Deserialize<T>(value.ToString()!);
            }
            catch
            {
                  return default;
            }
      }

      public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
      {
            try
            {
                  var db = redis.GetDatabase();
                  var json = JsonSerializer.Serialize(value);
                  await db.StringSetAsync(key, json, expiry);
            }
            catch
            {
                  // Redis runtime failure → ignore
            }
      }
}
