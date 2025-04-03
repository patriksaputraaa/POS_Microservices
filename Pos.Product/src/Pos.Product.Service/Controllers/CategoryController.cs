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
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Categories> categoryRepository;

        public CategoryController(IRepository<Categories> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = (await categoryRepository.GetAllAsync()).Select(category => category.AsDto());
            return categories;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetItem(Guid id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            return category.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto createCategoryDto)
        {
            var category = new Categories
            {
                Name = createCategoryDto.Name,
            };
            await categoryRepository.CreateAsync(category);
            var CategoryDto = category.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = CategoryDto.Id }, CategoryDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id);
            if (existingCategory is null)
            {
                return NotFound();
            }
            existingCategory.Name = updateCategoryDto.Name;
            await categoryRepository.UpdateAsync(existingCategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id);
            if (existingCategory is null)
            {
                return NotFound();
            }
            await categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}