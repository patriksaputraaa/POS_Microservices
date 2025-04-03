using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Common;
namespace Pos.Product.Service.Entities
{
    public class Categories : IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = "";
    }
}