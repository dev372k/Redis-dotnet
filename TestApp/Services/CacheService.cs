using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TestApp.Helpers;

namespace TestApp.Services
{
    public interface ICacheService
    {
        Task Set<T>(string key, T value);
        Task<T> Get<T>(string key);
        Task<bool> Remove(string key);
    }
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task Set<T>(string key, T value) => await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), CacheHelper.CacheOptions());

        public async Task<T> Get<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);
            return default;
        }

        public async Task<bool> Remove(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
                return false;

            await _cache.RemoveAsync(key);
            return true;
        }
    }
}
