using Feli.Payments.API.Payments;
using System;
using System.Collections.Generic;

namespace Feli.Payments.API.Data.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public List<PaymentItem> Items { get; set; } = new();
        public string Currency { get; set; }
        public PaymentState State { get; set; } = PaymentState.Waiting;
        public string Provider { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
