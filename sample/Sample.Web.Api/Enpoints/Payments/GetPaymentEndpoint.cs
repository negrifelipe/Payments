using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Feli.Payments.API.Payments;
using Feli.Payments.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Web.Api.Enpoints.Payments
{
    public class GetPaymentEndpoint : EndpointBaseAsync
        .WithRequest<Guid>
        .WithActionResult<StartPaymentResultDto>
    {
        private readonly IPaymentsService paymentsService;

        public GetPaymentEndpoint(IPaymentsService paymentsService)
        {
            this.paymentsService = paymentsService;
        }

        [HttpGet("/api/payments/{id}")]
        public override async Task<ActionResult<StartPaymentResultDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await paymentsService.StartPaymentAsync(id);

            return result.ToActionResult(this);
        }
    }
}
