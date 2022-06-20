using System;

namespace Feli.Payments.API.Data.Entities
{
    public class PaymentItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public Guid PaymentId { get; set; }

        public Payment Payment { get; set; }
    }
}
