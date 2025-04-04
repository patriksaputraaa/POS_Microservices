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

        public SaleItemsController(IRepository<SaleItems> saleItemsRepository, ProductClient productClient)
        {
            this.saleItemsRepository = saleItemsRepository;
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