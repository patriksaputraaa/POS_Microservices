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
}