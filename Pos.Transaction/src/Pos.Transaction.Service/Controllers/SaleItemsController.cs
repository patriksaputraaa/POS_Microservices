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
        public async Task<ActionResult<SaleItemsDto>> GetByIdAsync(Guid id)
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
            var SaleItemsDto = saleItems.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = SaleItemsDto.Id }, SaleItemsDto);
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