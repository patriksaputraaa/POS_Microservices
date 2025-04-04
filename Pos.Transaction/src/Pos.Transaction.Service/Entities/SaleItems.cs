using Pos.Common;

namespace Pos.Transaction.Service.Entities
{
    public class SaleItems : IEntity
    {
        public Guid Id { get; init; }
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}