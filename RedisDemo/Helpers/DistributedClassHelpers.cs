using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisDemo.Helpers
{
    public static class DistributedClassHelpers
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, 
            string key, T data,
            TimeSpan? fullExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = fullExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = unusedExpireTime
            };

            string json = JsonSerializer.Serialize(data);
            
            await cache.SetStringAsync(key, json, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string key)
        {
            string json = await cache.GetStringAsync(key);

            if (json is null) return default;

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
