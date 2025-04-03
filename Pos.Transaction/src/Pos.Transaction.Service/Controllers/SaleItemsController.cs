using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pos.Transaction.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleItemsController : ControllerBase
    {
        private readonly IRepository<SaleItems> saleItemsRepository;
        private readonly ProductClient productClient;

        public SaleItemsController(IRepository<SaleItems> saleItemsRepository, CustomerClient customerClient)
        {
            this.saleItemsRepository = saleItemsRepository;
            this.customerClient = customerClient;
            this.productClient = productClient;
        }

        [HttpGet]
        public async Task<IEnumerable<SaleItemsDto>> GetAll()
        {
            var saleItems = (await saleItemsRepository.GetAllAsync()).Select(saleItems => saleItems.AsDto());
            return saleItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SaleItemsDto>>> GetByIdAsync(Guid id)
        {
            var saleItems = await saleItemsRepository.GetByIdAsync(id);
            if (saleItems is null)
            {
                return NotFound();
            }
            return saleItems.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<SaleItems>> Post(CreateSaleItems createSaleItems)
        {
            var saleItems = new SaleItems
            {
                SaleId = createSaleItems.SaleId,
                ProductId = createSaleItems.ProductId,
                Quantity = createSaleItems.Quantity,
                Price = createSaleItems.Price
            };
            await saleItemsRepository.CreateAsync(product);
            var SaleItems = saleItems.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = SaleItems.Id }, SaleItems);
        }

        #====================================== SAMPAI SINI TINGGAL GANTI KE SALEITEMS===================================

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await productRepository.GetByIdAsync(id);
            if (existingProduct is null)
            {
                return NotFound();
            }
            existingProduct.Name = updateProductDto.Name;
            existingProduct.CategoryId = updateProductDto.CategoryId;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.Description = updateProductDto.Description;
            await productRepository.UpdateAsync(existingProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingProduct = await productRepository.GetByIdAsync(id);
            if (existingProduct is null)
            {
                return NotFound();
            }
            await productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}