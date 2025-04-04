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
            return products;
        }
    }
}