using Feli.Payments.API.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Feli.Payments.Providers.PayPal.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPayPal(this IServiceCollection services)
        {
            services.AddTransient<IPaymentProvider, PayPalPaymentProvider>();
            return services;
        }
    }
}
