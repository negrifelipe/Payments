using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Feli.Payments.API.Payments;
using Feli.Payments.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Web.Api.Enpoints.Payments
{
    public class PostPaymentEndpoint : EndpointBaseAsync
        .WithRequest<CreatePaymentDto>
        .WithActionResult<PaymentDto>
    {
        private readonly IPaymentsService paymentsService;

        public PostPaymentEndpoint(IPaymentsService paymentsService)
        {
            this.paymentsService = paymentsService;
        }

        [HttpPost("/api/payments")]
        public override async Task<ActionResult<PaymentDto>> HandleAsync([FromBody] CreatePaymentDto request, CancellationToken cancellationToken = default)
        {
            var result = await paymentsService.CreatePaymentAsync(request);

            return result.ToActionResult(this);
        }
    }
}
