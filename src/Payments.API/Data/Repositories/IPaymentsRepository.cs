using Feli.Payments.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Feli.Payments.API.Data.Repositories
{
    public interface IPaymentsRepository
    {
        Task<IEnumerable<Payment>> SelectPaymentsAsync();
        Task<Payment> SelectPaymentByIdAsync(Guid id);
        Task<Payment> InsertPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
    }
}
