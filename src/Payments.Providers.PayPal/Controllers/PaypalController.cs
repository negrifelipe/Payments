using Feli.Payments.API.Payments;
using Feli.Payments.API.Services;
using Feli.Payments.Providers.PayPal.Cofiguration;
using Feli.Payments.Providers.PayPal.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Feli.Payments.Providers.PayPal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PaypalController : ControllerBase
    {
        private readonly IOptionsMonitor<PayPalOptions> options;
        private readonly IPaymentsService paymentsService;
        private readonly HttpClient client;

        public PaypalController(IPaymentsService paymentsService, IHttpClientFactory httpClientFactory, IOptionsMonitor<PayPalOptions> options)
        {
            this.options = options;
            this.paymentsService = paymentsService;
            this.client = httpClientFactory.CreateClient("paypal-ipn");
        }

        [HttpPost("notify")]
        public async Task<IActionResult> NotifyAsync()
        {
            using var reader = new StreamReader(Request.Body, Encoding.ASCII);
            var payload = await reader.ReadToEndAsync();

            await ValidateAsync(payload);

            return Ok();
        }

        private async Task ValidateAsync(string body)
        {
            var form = HttpUtility.ParseQueryString(body);

            if (!Guid.TryParse(form["custom"], out var custom))
                return;

            var paymentResult = await paymentsService.GetPaymentByIdAsync(custom);

            if (!paymentResult.IsSuccess)
                return;

            var payment = paymentResult.Value;

            var paypalUrl = options.CurrentValue.Sandbox ? PayPalConstants.PayPalSandboxUrl : PayPalConstants.PayPalUrl;

            var content = new StringContent("cmd=_notify-validate&" + body);
            var response = await client.PostAsync(paypalUrl, content);

            if (await response.Content.ReadAsStringAsync() != "VERIFIED")
                return;

            if (form["receiver_email"] != options.CurrentValue.Receiver)
                return;

            if (form["mc_currency"] != payment.Currency)
                return;

            if (form["mc_gross"] != payment.Items.Sum(i => i.Price * i.Amount).ToString(CultureInfo.InvariantCulture))
                return;

            var status = form["payment_status"];

            if (status == "Completed")
                payment.State = PaymentState.Completed;
            else if (status == "Refunded")
                payment.State = PaymentState.Refunded;
            else if (status == "Reversed")
                payment.State = PaymentState.Reversed;
            else if (status == "Denied" || status == "Failed")
                payment.State = PaymentState.Cancelled;

            if (payment.State != PaymentState.Waiting)
            {
                await paymentsService.UpdatePaymentAsync(new UpdatePaymentDto
                {
                    Id = payment.Id,
                    State = payment.State
                });
            }
        }
    }
}
