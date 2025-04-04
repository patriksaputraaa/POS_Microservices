namespace Pos.Transaction.Service.Clients
{
    public class CustomerClient
    {
        private readonly HttpClient _httpClient;

        public CustomerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public CustomerClient(string baseAddress)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<IReadOnlyCollection<CustomerDto>> GetCustomersAsync()
        {
            var customers = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CustomerDto>>("/api/Customer");
            if (customers == null)
            {
                return [];
            }
            return customers;
        }
    }
}