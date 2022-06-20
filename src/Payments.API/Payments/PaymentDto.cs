using Feli.Payments.API.Payments.Products;
using System;
using System.Collections.Generic;

namespace Feli.Payments.API.Payments
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public List<PaymentItemDto> Items { get; set; }
        public string Currency { get; set; }
        public PaymentState State { get; set; }
        public string Provider { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
