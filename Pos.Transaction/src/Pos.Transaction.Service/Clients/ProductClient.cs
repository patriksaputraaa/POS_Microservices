using Polly;
using Polly.Retry;

namespace Pos.Transaction.Service.Clients
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy retryPolicy;

        public ProductClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;

            // Konfigurasi Polly untuk retry hingga 3 kali
            retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    // Log setiap percobaan gagal
                    Console.WriteLine($"Retry {retryCount} failed. Waiting {timeSpan} before next retry.");

                    // Jika sudah mencapai batas retry, matikan service
                    if (retryCount == 3)
                    {
                        Console.WriteLine("Maximum retry attempts reached. Shutting down the service.");
                        
                    }
                });
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync()
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync("api/Product");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<ProductDto>>() ?? new List<ProductDto>();
            });
        }

        public async Task<ProductDto?> UpdateProductAsync(Guid id, ProductDto productUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Product/{id}", productUpdateDto);
            response.EnsureSuccessStatusCode();
            if (response.Content.Headers.ContentLength == 0)
            {
                // Handle the case where the server returns an empty response
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
    }
}