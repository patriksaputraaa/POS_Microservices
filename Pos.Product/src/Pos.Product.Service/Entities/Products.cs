using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Common;

namespace Pos.Product.Service.Entities
{
    public class Products : IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = "";
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = "";
    }
}