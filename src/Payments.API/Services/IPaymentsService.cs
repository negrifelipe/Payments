using Ardalis.Result;
using Feli.Payments.API.Payments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Feli.Payments.API.Services
{
    public interface IPaymentsService
    {
        Task<IEnumerable<PaymentDto>> GetPaymentsAsync();
        Task<Result<PaymentDto>> GetPaymentByIdAsync(Guid id);
        Task<Result<PaymentDto>> CreatePaymentAsync(CreatePaymentDto createPayment);
        Task<Result<StartPaymentResultDto>> StartPaymentAsync(Guid id);
        Task<Result<PaymentDto>> UpdatePaymentAsync(UpdatePaymentDto updatePayment);
    }
}
