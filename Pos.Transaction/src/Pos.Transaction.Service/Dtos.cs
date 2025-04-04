using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pos.Transaction.Service
{
        public record SalesDto(Guid Id, Guid CustomerId, DateTime SaleDate, decimal TotalAmount);
        public record CreateSalesDto(Guid CustomerId, DateTime SaleDate, decimal TotalAmount);
        public record UpdateSalesDto(Guid CustomerId, DateTime SaleDate, decimal TotalAmount);

        public record SaleItemsDto(Guid Id, Guid SaleId, Guid ProductId, int Quantity, decimal Price);
        public record CreateSaleItemsDto(Guid SaleId, Guid ProductId, int Quantity, decimal Price);
        public record UpdateSaleItemsDto(Guid SaleId, Guid ProductId, int Quantity, decimal Price);

        public record CustomerDto(Guid Id, string Name, string ContactNumber, string Address, string Email);
        public record ProductDto(Guid Id, string Name, Guid CategoryId, decimal Price, int Stock, string Description);

        public record SaleTransactionDto(Guid Id, Guid CustomerId, string CustomerName, DateTime SaleDate, List<SaleProductsDto> SaleProducts, decimal TotalAmount);
        public record SaleProductsDto(Guid ProductId, string ProductName, int Quantity, decimal Price);
}