using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Customer.Service.Entities;
namespace Pos.Customer.Service
{
    public static class Extensions
    {
        public static CustomerDto AsDto(this Customers customer)
        {
            return new CustomerDto(customer.Id, customer.Name, customer.ContactNumber, customer.Address, customer.Email);
        }
    }
}