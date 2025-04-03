using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Customer.Service
{
        public record CustomerDto(Guid Id, string Name, string ContactNumber, string Address, string Email);
        public record CreateCustomerDto(string Name, string ContactNumber, string Address, string Email);
        public record UpdateCustomerDto(string Name, string ContactNumber, string Address, string Email);
}