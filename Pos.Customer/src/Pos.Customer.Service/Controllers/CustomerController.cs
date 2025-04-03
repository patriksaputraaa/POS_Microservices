using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pos.Common;
using Pos.Customer.Service.Entities;

namespace Pos.Customer.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customers> customerRepository;

        public CustomerController(IRepository<Customers> customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> GetAll()
        {
            var customers = (await customerRepository.GetAllAsync()).Select(customer => customer.AsDto());
            return customers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetItem(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return NotFound();
            }
            return customer.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(CreateCustomerDto createCustomerDto)
        {
            var customer = new Customers
            {
                Name = createCustomerDto.Name,
                ContactNumber = createCustomerDto.ContactNumber,
                Address = createCustomerDto.Address,
                Email = createCustomerDto.Email
            };
            await customerRepository.CreateAsync(customer);
            var CustomerDto = customer.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = CustomerDto.Id }, CustomerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await customerRepository.GetByIdAsync(id);
            if (existingCustomer is null)
            {
                return NotFound();
            }
            existingCustomer.Name = updateCustomerDto.Name;
            existingCustomer.ContactNumber = updateCustomerDto.ContactNumber;
            existingCustomer.Address = updateCustomerDto.Address;
            existingCustomer.Email = updateCustomerDto.Email;
            await customerRepository.UpdateAsync(existingCustomer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingCustomer = await customerRepository.GetByIdAsync(id);
            if (existingCustomer is null)
            {
                return NotFound();
            }
            await customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}