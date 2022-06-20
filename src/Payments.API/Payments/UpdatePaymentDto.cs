using System;

namespace Feli.Payments.API.Payments
{
    public class UpdatePaymentDto
    {
        public Guid Id { get; set; }
        public PaymentState State { get; set; }
    }
}
