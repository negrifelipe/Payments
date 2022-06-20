using Feli.Payments.API.Payments.Products;
using System.Collections.Generic;

namespace Feli.Payments.API.Payments
{
    public class CreatePaymentDto
    {
        public List<PaymentItemDto> Items { get; set; }
        public string Currency { get; set; }
        public string Provider { get; set; }
    }
}
