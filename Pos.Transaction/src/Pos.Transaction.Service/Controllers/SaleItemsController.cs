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
    public class SaleItemsController : ControllerBase
    {
        private readonly IRepository<SaleItems> saleItemsRepository;
        private readonly IRepository<Sales> salesRepository;
        private readonly ProductClient productClient;

        public SaleItemsController(IRepository<SaleItems> saleItemsRepository, IRepository<Sales> salesRepository, ProductClient productClient)
        {
            this.saleItemsRepository = saleItemsRepository;
            this.salesRepository = salesRepository;
            this.productClient = productClient;
        }

        [HttpGet]
        public async Task<IEnumerable<SaleItemsDto>> GetAll()
        {
            var saleItems = (await saleItemsRepository.GetAllAsync()).Select(saleItems => saleItems.AsDto());
            return saleItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleItemsDto>> GetItem(Guid id)
        {
            var saleItems = await saleItemsRepository.GetByIdAsync(id);
            if (saleItems is null)
            {
                return NotFound();
            }
            return saleItems.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<SaleItemsDto>> Post(CreateSaleItemsDto createSaleItemsDto)
        {
            var saleItems = new SaleItems
            {
                SaleId = createSaleItemsDto.SaleId,
                ProductId = createSaleItemsDto.ProductId,
                Quantity = createSaleItemsDto.Quantity,
                Price = createSaleItemsDto.Price
            };
            await saleItemsRepository.CreateAsync(saleItems);

            var sales = await salesRepository.GetByIdAsync(saleItems.SaleId);
            if (sales != null)
            {
                sales.TotalAmount += saleItems.Price * saleItems.Quantity;
                await salesRepository.UpdateAsync(sales);
            }

            var product = await productClient.GetProductsAsync();
            var existingProduct = product.SingleOrDefault(p => p.Id == saleItems.ProductId);
            if (existingProduct != null)
            {
                var updatedProduct = new ProductDto(
                    existingProduct.Id,
                    existingProduct.Name,
                    existingProduct.CategoryId,
                    existingProduct.Price,
                    existingProduct.Stock - saleItems.Quantity,
                    existingProduct.Description
                    );
                await productClient.UpdateProductAsync(updatedProduct.Id, updatedProduct);
            }

            var saleItemsDto = saleItems.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = saleItemsDto.Id }, saleItemsDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateSaleItemsDto updateSaleItemsDto)
        {
            var existingSaleItems = await saleItemsRepository.GetByIdAsync(id);
            if (existingSaleItems is null)
            {
                return NotFound();
            }
            existingSaleItems.SaleId = updateSaleItemsDto.SaleId;
            existingSaleItems.ProductId = updateSaleItemsDto.ProductId;
            existingSaleItems.Quantity = updateSaleItemsDto.Quantity;
            existingSaleItems.Price = updateSaleItemsDto.Price;
            await saleItemsRepository.UpdateAsync(existingSaleItems);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingSaleItems = await saleItemsRepository.GetByIdAsync(id);
            if (existingSaleItems is null)
            {
                return NotFound();
            }
            await saleItemsRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}