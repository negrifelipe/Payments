using Feli.Payments.API.Configuration;
using Feli.Payments.API.Payments;
using Feli.Payments.API.Providers;
using Feli.Payments.Providers.PayPal.Cofiguration;
using Feli.Payments.Providers.PayPal.Constants;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Feli.Payments.Providers.PayPal
{
    public class PayPalPaymentProvider : IPaymentProvider
    {
        public string Name => "PayPal";

        private readonly IOptionsMonitor<PaymentOptions> paymentOptions;
        private readonly IOptionsMonitor<PayPalOptions> paypalOptions;

        public PayPalPaymentProvider(
            IOptionsMonitor<PaymentOptions> paymentOptions,
            IOptionsMonitor<PayPalOptions> paypalOptions)
        {
            this.paymentOptions = paymentOptions;
            this.paypalOptions = paypalOptions;
        }

        public Task<StartPaymentResultDto> StartPaymentAsync(StartPaymentDto startPayment)
        {
            var notify_url = paymentOptions.CurrentValue.BaseUrl.TrimEnd('/') + "/payments/paypal/notify";

            var form = new Dictionary<string, string>()
            {
                {"cmd", "_cart" },
                {"notify_url", notify_url },
                {"amount", startPayment.Items.Sum(x => x.Price * x.Amount).ToString(CultureInfo.InvariantCulture) },
                {"currency_code", startPayment.Currency.ToString() },
                {"custom", startPayment.Id.ToString() },
                {"upload", "1" },
                {"business", paypalOptions.CurrentValue.Receiver },
                {"no_shipping", "1" },
                {"no_note", "1" }
            };

            if (!string.IsNullOrEmpty(paypalOptions.CurrentValue.ReturnUrl))
            {
                form.Add("return", paypalOptions.CurrentValue.ReturnUrl);
            }

            if (!string.IsNullOrEmpty(paypalOptions.CurrentValue.CancelReturnUrl))
            {
                form.Add("cancel_return", paypalOptions.CurrentValue.CancelReturnUrl);
            }

            if (!string.IsNullOrEmpty(paypalOptions.CurrentValue.ShoppingUrl))
            {
                form.Add("shopping_url", paypalOptions.CurrentValue.ShoppingUrl);
            }

            for (int i = 0; i < startPayment.Items.Count; i++)
            {
                var item = startPayment.Items[i];
                var number = i + 1;

                form.Add($"item_name_{number}", item.Name);
                form.Add($"quantity_{number}", item.Amount.ToString());
                form.Add($"amount_{number}", item.Price.ToString(CultureInfo.InvariantCulture));
            }

            var paypalUrl = paypalOptions.CurrentValue.Sandbox ? PayPalConstants.PayPalSandboxUrl : PayPalConstants.PayPalUrl;

            return Task.FromResult(new StartPaymentResultDto { CheckoutUrl = QueryHelpers.AddQueryString(paypalUrl, form) });
        }
    }
}
