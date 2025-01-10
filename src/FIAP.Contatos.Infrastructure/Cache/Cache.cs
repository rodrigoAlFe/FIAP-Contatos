using FIAP.Contatos.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;


namespace FIAP.Contatos.Infrastructure.Cache
{

    public class ContatoCache(IMemoryCache cache)
    {
        private const string CacheKey = "Contatos";

        public async Task<List<Contato>>? Get()
        {
            return cache.TryGetValue(CacheKey, out List<Contato>? contatos) ? contatos : new List<Contato>();
        }

        public void Set(List<Contato> contatos)
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
