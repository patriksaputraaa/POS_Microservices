using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos.Common;
using Pos.Product.Service.Entities;

namespace Pos.Product.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Products> productRepository;

        public ProductController(IRepository<Products> productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = (await productRepository.GetAllAsync()).Select(product => product.AsDto());
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetItem(Guid id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return product.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post(CreateProductDto createProductDto)
        {
            var product = new Products
            {
                Name = createProductDto.Name,
                CategoryId = createProductDto.CategoryId,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Description = createProductDto.Description
            };
            await productRepository.CreateAsync(product);
            var ProductDto = product.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = ProductDto.Id }, ProductDto);
        }

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