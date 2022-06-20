using Feli.Payments.API.Events;
using Feli.Payments.API.Services;
using Feli.Payments.Mappers;
using Feli.Payments.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feli.Payments.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPayments(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PaymentMapper));
            services.AddTransient<IPaymentsService, PaymentsService>();
            return services;
        }

        public static IServiceCollection AddPaymentUpdatedEvent<T>(this IServiceCollection services) where T : IPaymentUpdatedEvent
        {
            services.AddTransient(typeof(IPaymentUpdatedEvent), typeof(T));
            return services;
        }
    }
}
