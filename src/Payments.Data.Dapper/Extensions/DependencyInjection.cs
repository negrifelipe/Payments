using Feli.Payments.API.Data.Repositories;
using Feli.Payments.Data.Dapper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Feli.Payments.Data.Dapper.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPaymentsRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddTransient<IPaymentItemsRepository, PaymentItemsRepository>();
            return services;
        }
    }
}
