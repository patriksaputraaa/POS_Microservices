using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Transaction.Service.Clients
{
    public class CustomerClient
    {
        private readonly HttpClient httpClient;
        public CustomerClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CustomerDto>> GetCustomersAsync()
        {
            var customers = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CustomerDto>>("/api/Customer");
            return customers;
        }
    }
}