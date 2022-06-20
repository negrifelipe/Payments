using AutoMapper;
using Feli.Payments.API.Data.Entities;
using Feli.Payments.API.Payments;
using Feli.Payments.API.Payments.Products;

namespace Feli.Payments.Mappers
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper()
        {
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<Payment, StartPaymentDto>();
            CreateMap<PaymentItem, PaymentItemDto>().ReverseMap();
        }
    }
}
