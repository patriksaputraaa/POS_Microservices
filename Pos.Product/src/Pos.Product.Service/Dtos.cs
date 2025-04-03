using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Product.Service
{
        public record ProductDto(Guid Id, string Name, Guid CategoryId, decimal Price, int Stock, string Description);
        public record CreateProductDto(string Name, Guid CategoryId, decimal Price, int Stock, string Description);
        public record UpdateProductDto(string Name, Guid CategoryId, decimal Price, int Stock, string Description);

        public record CategoryDto(Guid Id, string Name);
        public record CreateCategoryDto(string Name);
        public record UpdateCategoryDto(string Name);
}