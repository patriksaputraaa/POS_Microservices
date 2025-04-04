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
        private readonly IRepository<SaleItems> saleItemsRepository;
        private readonly CustomerClient customerClient;
        private readonly ProductClient productClient;

        public SalesController(IRepository<Sales> salesRepository, IRepository<SaleItems> saleItemsRepository, CustomerClient customerClient, ProductClient productClient)
        {
            this.salesRepository = salesRepository;
            this.saleItemsRepository = saleItemsRepository;
            this.customerClient = customerClient;
            this.productClient = productClient;
        }

        [HttpGet]
        public async Task<IEnumerable<SalesDto>> GetAll()
        {
            var sales = (await salesRepository.GetAllAsync()).Select(sales => sales.AsDto());
            return sales;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleTransactionDto>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var sale = await salesRepository.GetByIdAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            var productCatalog = await productClient.GetProductsAsync();
            var customerCatalog = await customerClient.GetCustomersAsync();
            var saleItems = await saleItemsRepository.GetAllAsync();

            var customer = customerCatalog.SingleOrDefault(c => c.Id == sale.CustomerId);
            var totalAmount = saleItems.Where(saleItem => saleItem.SaleId == id).Sum(saleItem => saleItem.Price);
            var saleItemsDto = saleItems
                .Where(saleItem => saleItem.SaleId == id)
                .Select(saleItem =>
                {
                    var product = productCatalog.SingleOrDefault(p => p.Id == saleItem.ProductId);
                    return new SaleProductsDto(saleItem.ProductId, product?.Name, saleItem.Quantity, saleItem.Price);
                }).ToList();

            var saleTransactionDto = new SaleTransactionDto(
                sale.Id,
                sale.CustomerId,
                customer.Name,
                sale.SaleDate,
                saleItemsDto,
                totalAmount
            );

            return saleTransactionDto;
        }

        [HttpPost]
        public async Task<ActionResult<SalesDto>> Post(CreateSalesDto createSalesDto)
        {
            var sales = new Sales
            {
                CustomerId = createSalesDto.CustomerId,
                SaleDate = createSalesDto.SaleDate
            };
            await salesRepository.CreateAsync(sales);
            var salesDto = sales.AsDto();
            return CreatedAtAction(nameof(GetByIdAsync), new { id = salesDto.Id }, salesDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalesDto>> Put(Guid id, UpdateSalesDto updateSalesDto)
        {
            var existingSales = await salesRepository.GetByIdAsync(id);
            if (existingSales is null)
            {
                return NotFound();
            }
            existingSales.CustomerId = updateSalesDto.CustomerId;
            existingSales.SaleDate = updateSalesDto.SaleDate;
            await salesRepository.UpdateAsync(existingSales);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingSales = await salesRepository.GetByIdAsync(id);
            if (existingSales is null)
            {
                return NotFound();
            }
            await salesRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}