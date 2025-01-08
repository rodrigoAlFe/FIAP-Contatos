using FIAP.Contatos.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;


namespace FIAP.Contatos.Infrastructure.Cache
{

    public class ContatoCache
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "Contatos";

        public ContatoCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<List<Contato>>? Get()
        {
            return _cache.TryGetValue(CacheKey, out List<Contato>? contatos) ? contatos : new List<Contato>();
        }

        public void Set(List<Contato> contatos)
        {
            var memoryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(150),
                SlidingExpiration = TimeSpan.FromSeconds(60)
            };
            _cache.Set(CacheKey, contatos, memoryOptions);

        }
    }
}
