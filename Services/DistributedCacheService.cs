using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LocoRealt.Services
{
    public class DistributedCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // camelCase для JSON
                WriteIndented = false, // минифицированный JSON
                ReferenceHandler = ReferenceHandler.Preserve, // поддержка циклических ссылок
                MaxDepth = 128 // увеличиваем глубину, если нужно (по умолчанию 64)
            };
        }

        public async Task<T?> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            // Пытаемся получить данные из кэша
            var cachedData = await _cache.GetAsync(key, cancellationToken);

            if (cachedData != null)
            {
                try
                {
                    var json = Encoding.UTF8.GetString(cachedData);
                    var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                    if (result != null)
                        return result;
                }
                catch (JsonException ex)
                {
                    // Логируем ошибку, если нужно (можно использовать ILogger)
                    Console.WriteLine($"JSON deserialization error: {ex.Message}");
                    // Продолжаем и генерируем данные заново
                }
            }

            // Если в кэше нет данных или произошла ошибка — вызываем фабрику
            var data = await factory();

            // Сериализуем и сохраняем в кэш
            var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
            var bytes = Encoding.UTF8.GetBytes(jsonData);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
            };

            await _cache.SetAsync(key, bytes, cacheOptions, cancellationToken);

            return data;
        }
    }
}