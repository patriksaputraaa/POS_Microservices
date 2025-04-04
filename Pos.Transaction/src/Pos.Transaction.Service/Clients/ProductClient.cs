namespace Pos.Transaction.Service.Clients
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ProductClient(string baseAddress)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync()
        {
            var products = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<ProductDto>>("/api/Product");
            if (products == null)
            {
                return [];
            }
            return products;
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