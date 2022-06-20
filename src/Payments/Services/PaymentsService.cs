using Ardalis.Result;
using AutoMapper;
using Feli.Payments.API.Data.Entities;
using Feli.Payments.API.Data.Repositories;
using Feli.Payments.API.Events;
using Feli.Payments.API.Payments;
using Feli.Payments.API.Providers;
using Feli.Payments.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feli.Payments.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository paymentsRepository;
        private readonly IPaymentItemsRepository paymentItemsRepository;
        private readonly IEnumerable<IPaymentProvider> paymentProviders;
        private readonly IEnumerable<IPaymentUpdatedEvent> paymentUpdatedEvents;
        private readonly IMapper mapper;

        public PaymentsService(
            IPaymentsRepository paymentsRepository,
            IPaymentItemsRepository paymentItemsRepository,
            IEnumerable<IPaymentProvider> paymentProviders,
            IEnumerable<IPaymentUpdatedEvent> paymentUpdatedEvents,
            IMapper mapper)
        {
            this.paymentsRepository = paymentsRepository;
            this.paymentItemsRepository = paymentItemsRepository;
            this.paymentProviders = paymentProviders;
            this.paymentUpdatedEvents = paymentUpdatedEvents;
            this.mapper = mapper;
        }

        public async Task<Result<PaymentDto>> CreatePaymentAsync(CreatePaymentDto createPayment)
        {
            if (!paymentProviders.Any(provider => provider.Name == createPayment.Provider))
            {
                return Result<PaymentDto>.Invalid(new()
                {
                    new ValidationError()
                    {
                        Identifier = nameof(createPayment.Provider),
                        ErrorMessage = "The payment provider was not found."
                    }
                });
            }

            if (createPayment.Currency.Length != 3)
            {
                return Result<PaymentDto>.Invalid(new()
                {
                    new ValidationError()
                    {
                        Identifier = nameof(createPayment.Currency),
                        ErrorMessage = "The currency lenght is not 3. Make sure that it's a valid currency code. This service has adopted ISO 4217"
                    }
                });
            }

            if (!ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == createPayment.Currency))
            {
                return Result<PaymentDto>.Invalid(new()
                {
                    new ValidationError()
                    {
                        Identifier = nameof(createPayment.Currency),
                        ErrorMessage = $"Could not find a currency with code {createPayment.Currency}"
                    }
                });
            }

            var payment = mapper.Map<Payment>(createPayment);
            payment = await paymentsRepository.InsertPaymentAsync(payment);

            var paymentItems = createPayment.Items.Select(i => new PaymentItem
            {
                Name = i.Name,
                Amount = i.Amount,
                Price = i.Price,
                PaymentId = payment.Id
            });

            paymentItems = await paymentItemsRepository.InsertPaymentItemsAsync(paymentItems);
            payment.Items = paymentItems.ToList();

            return mapper.Map<PaymentDto>(payment);
        }

        public async Task<Result<PaymentDto>> GetPaymentByIdAsync(Guid id)
        {
            var payment = await paymentsRepository.SelectPaymentByIdAsync(id);

            if (payment is null)
                return Result<PaymentDto>.NotFound();

            return Result.Success(mapper.Map<PaymentDto>(payment));
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync()
        {
            var payments = await paymentsRepository.SelectPaymentsAsync();

            return mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<Result<StartPaymentResultDto>> StartPaymentAsync(Guid id)
        {
            var payment = await paymentsRepository.SelectPaymentByIdAsync(id);

            if (payment is null)
            {
                return Result<StartPaymentResultDto>.NotFound();
            }

            var provider = paymentProviders.FirstOrDefault(x => x.Name == payment.Provider);

            var startPayment = mapper.Map<StartPaymentDto>(payment);

            var result = await provider.StartPaymentAsync(startPayment);

            return Result<StartPaymentResultDto>.Success(result);
        }

        public async Task<Result<PaymentDto>> UpdatePaymentAsync(UpdatePaymentDto updatePayment)
        {
            var payment = await paymentsRepository.SelectPaymentByIdAsync(updatePayment.Id);

            if (payment is null)
            {
                return Result<PaymentDto>.NotFound();
            }

            payment.State = updatePayment.State;
            await paymentsRepository.UpdatePaymentAsync(payment);

            var paymentDto = mapper.Map<PaymentDto>(payment);

            foreach (var @event in paymentUpdatedEvents)
            {
                await @event.OnPaymentUpdatedAsync(paymentDto);
            }

            return Result.Success(paymentDto);
        }
    }
}
