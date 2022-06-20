using Feli.Payments.API.Payments;
using System.Threading.Tasks;

namespace Feli.Payments.API.Events
{
    public interface IPaymentUpdatedEvent
    {
        Task OnPaymentUpdatedAsync(PaymentDto payment);
    }
}
