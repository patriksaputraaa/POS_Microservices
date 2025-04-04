using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pos.Transaction.Service.Entities;
namespace Pos.Transaction.Service
{
    public static class Extensions
    {
        public static SalesDto AsDto(this Sales sales)
        {
            return new SalesDto(sales.Id, sales.CustomerId, sales.SaleDate, sales.TotalAmount);
        }

        public static SaleItemsDto AsDto(this SaleItems saleItems)
        {
            return new SaleItemsDto(saleItems.Id, saleItems.SaleId, saleItems.ProductId, saleItems.Quantity, saleItems.Price);
        }
    }
}