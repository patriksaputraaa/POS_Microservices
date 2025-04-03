using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Transaction.Service.Clients
{
    public class ProductClient
    {
        private readonly HttpClient httpClient;
        public ProductClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync()
        {
            var products = await httpClient.GetFromJsonAsync<IReadOnlyCollection<ProductDto>>("/api/Product");
            return products;
        }
    }
}