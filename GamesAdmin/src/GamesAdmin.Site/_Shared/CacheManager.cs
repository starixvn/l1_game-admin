using System;
using Microsoft.Extensions.Caching.Memory;

namespace GamesAdmin.Site._Shared
{
    public enum CachedKeys
    {
        OnlineUsers,
    }

    public interface ICacheManager
    {
        void SetCache(string key, int value);

        int GetCache(string key);

        void SetCache<T>(string key, T value);

        T GetCache<T>(string key);
    }
    public class CacheManager : ICacheManager
    {
        private const int ExpiredMinutes = 2;

        private readonly IMemoryCache cache;

        public CacheManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void SetCache(string key, int value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredMinutes));

            cache.Set(key, value, cacheEntryOptions);
        }

        public int GetCache(string key)
        {
            int onlineUser;

            if (cache.TryGetValue(key, out onlineUser))
            {
                return onlineUser;
            }

            return default;
        }

        public void SetCache<T>(string key, T value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromMinutes(ExpiredMinutes));

            cache.Set(key, value, cacheEntryOptions);
        }

        public T GetCache<T>(string key)
        {
            T value;

            if (cache.TryGetValue(key, out value))
            {
                return value;
            }

            return default;
        }
    }
}
