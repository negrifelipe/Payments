using Feli.Payments.API.Events;
using Feli.Payments.API.Payments;

namespace Sample.Web.Api.Events
{
    public class PaymentUpdatedEvent : IPaymentUpdatedEvent
    {
        private readonly ILogger<PaymentUpdatedEvent> logger;

        public PaymentUpdatedEvent(ILogger<PaymentUpdatedEvent> logger)
        {
            this.logger = logger;
        }

        public Task OnPaymentUpdatedAsync(PaymentDto payment)
        {
            logger.LogInformation("Payment: {id} has been updated. State: {state}", payment.Id, payment.State);
            // all your logic here
            return Task.CompletedTask;
        }
    }
}
