using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Common;

namespace Pos.Customer.Service.Entities
{
    public class Customers : IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
    }
}