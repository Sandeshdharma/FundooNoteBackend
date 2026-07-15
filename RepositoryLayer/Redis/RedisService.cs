using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RepositoryLayer.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;

        public RedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetData<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }


        public async Task SetData<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
            };

            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(value),
                options);
        }

        public async Task RemoveData(string key)
        {
            await _cache.RemoveAsync(key);
        }




    }
}
