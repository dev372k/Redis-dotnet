using Microsoft.Extensions.Caching.Distributed;

namespace TestApp.Helpers
{
    public class CacheHelper
    {
        public static DistributedCacheEntryOptions CacheOptions() =>
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5 * 60),
                SlidingExpiration = null
            };
    }
}
