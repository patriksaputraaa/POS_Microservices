using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos.Common;
using Pos.Transaction.Service.Clients;
using Pos.Transaction.Service.Entities;

namespace Pos.Transaction.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IRepository<Sales> salesRepository;
        private readonly CustomerClient customerClient;

        public SalesController(IRepository<Sales> salesRepository, CustomerClient customerClient)
        {
            this.salesRepository = salesRepository;
            this.customerClient = customerClient;
        }

        [HttpGet]
        public async Task<IEnumerable<SalesDto>> GetAll()
        {
            var sales = (await salesRepository.GetAllAsync()).Select(sales => sales.AsDto());
            return sales;
        }
    }
}