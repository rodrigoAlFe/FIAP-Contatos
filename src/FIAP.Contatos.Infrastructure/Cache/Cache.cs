using FIAP.Contatos.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;


namespace FIAP.Contatos.Infrastructure.Cache
{

    public class ContatoCache(IMemoryCache cache)
    {
        private const string CacheKey = "Contatos";

        public Task<List<Contato>?>? Get()
        {
            return Task.FromResult(cache.TryGetValue(CacheKey, out List<Contato>? contatos) ? contatos : []);
        }

        public void Set(List<Contato?>? contatos)
        {
            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(150),
                SlidingExpiration = TimeSpan.FromSeconds(60)
            };
            cache.Set(CacheKey, contatos, memoryOptions);

        }
    }
}
