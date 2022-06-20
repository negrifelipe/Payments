using Feli.Payments.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Feli.Payments.API.Data.Repositories
{
    public interface IPaymentItemsRepository
    {
        Task<IEnumerable<PaymentItem>> InsertPaymentItemsAsync(IEnumerable<PaymentItem> items);
    }
}
