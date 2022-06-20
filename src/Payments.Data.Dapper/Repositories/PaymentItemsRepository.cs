using Dapper;
using Feli.Payments.API.Data.Entities;
using Feli.Payments.API.Data.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Feli.Payments.Data.Dapper.Repositories
{
    public class PaymentItemsRepository : IPaymentItemsRepository
    {
        private readonly SqlConnection connection;

        public PaymentItemsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<PaymentItem>> InsertPaymentItemsAsync(IEnumerable<PaymentItem> items)
        {
            const string sql = "INSERT INTO PaymentItems (Name, Amount, Price, PaymentId) OUTPUT INSERTED.* VALUES (@Name, @Amount, @Price, @PaymentId)";

            await connection.ExecuteAsync(sql, items);

            return await connection.QueryAsync<PaymentItem>("SELECT * FROM PaymentItems WHERE PaymentId = @Id", new
            {
                // does the trik ;)
                Id = items.First().PaymentId
            });
        }
    }
}
