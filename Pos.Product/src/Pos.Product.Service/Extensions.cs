using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Product.Service.Entities;
namespace Pos.Product.Service
{
    public static class Extensions
    {
        public static ProductDto AsDto(this Products product)
        {
            return new ProductDto(product.Id, product.Name, product.CategoryId, product.Price, product.Stock, product.Description);
        }

        public static CategoryDto AsDto(this Categories category)
        {
            return new CategoryDto(category.Id, category.Name);
        }
    }
}