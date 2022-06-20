using Dapper;
using Feli.Payments.API.Data.Entities;
using Feli.Payments.API.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Feli.Payments.Data.Dapper.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly SqlConnection connection;

        public PaymentsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Payment> InsertPaymentAsync(Payment payment)
        {
            const string sql = "INSERT INTO Payments (Currency, Provider) OUTPUT INSERTED.* VALUES (@Currency, @Provider)";

            return await connection.QuerySingleAsync<Payment>(sql, payment);
        }

        public async Task<Payment> SelectPaymentByIdAsync(Guid id)
        {
            const string sql = "SELECT p.*, i.* FROM Payments AS p LEFT JOIN PaymentItems AS i ON p.Id = i.PaymentId WHERE p.Id = @id";

            Payment payment = null;

            await connection.QueryAsync<Payment, PaymentItem, Payment>(sql, (p, i) =>
            {
                if (payment is null)
                    payment = p;

                if (i != null)
                    payment.Items.Add(i);

                return null;
            }, new { id });

            return payment;
        }

        public async Task<IEnumerable<Payment>> SelectPaymentsAsync()
        {
            const string sql = "SELECT p.*, i.* FROM Payments AS p LEFT JOIN PaymentItems AS i ON p.Id = i.PaymentId";

            List<Payment> payments = new();

            await connection.QueryAsync<Payment, PaymentItem, Payment>(sql, (p, i) =>
            {
                var payment = payments.FirstOrDefault(x => x.Id == p.Id);

                if (payment is null)
                {
                    payment = p;
                    payments.Add(p);
                }

                if (i is not null)
                {
                    payment.Items.Add(i);
                }

                return null;
            });

            return payments;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            const string sql = "UPDATE Payments SET State = @State OUTPUT INSERTED.* WHERE Id = @Id";

            return await connection.QuerySingleAsync<Payment>(sql, payment);
        }
    }
}
