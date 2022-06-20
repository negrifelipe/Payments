using Feli.Payments.API.Payments.Products;
using System;
using System.Collections.Generic;

namespace Feli.Payments.API.Payments
{
    public class StartPaymentDto
    {
        public Guid Id { get; set; }
        public List<PaymentItemDto> Items { get; set; }
        public string Currency { get; set; }
        public string Provider { get; set; }
    }
}
