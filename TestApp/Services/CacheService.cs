using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TestApp.Helpers;

namespace TestApp.Services
{
    public interface ICacheService
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
        bool Remove(string key);
    }
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public void Set<T>(string key, T value) => _cache.SetString(key, JsonConvert.SerializeObject(value), CacheHelper.CacheOptions());

        public T Get<T>(string key)
        {
            var value = _cache.GetString(key);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);
            return default;
        }

        public bool Remove(string key)
        {
            var value = _cache.GetString(key);
            if (string.IsNullOrEmpty(value))
                return false;

            _cache.Remove(key);
            return true;
        }
    }
}
