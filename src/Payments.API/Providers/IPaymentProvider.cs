using Feli.Payments.API.Payments;
using System.Threading.Tasks;

namespace Feli.Payments.API.Providers
{
    public interface IPaymentProvider
    {
        string Name { get; }
        Task<StartPaymentResultDto> StartPaymentAsync(StartPaymentDto startPayment);
    }
}
