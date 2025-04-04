using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Common;

namespace Pos.Transaction.Service.Entities
{
    public class Sales : IEntity
    {
        public Guid Id { get; init; }
        public Guid CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}