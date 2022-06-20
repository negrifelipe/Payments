using Feli.Payments.API.Configuration;
using Feli.Payments.Data.Dapper.Extensions;
using Feli.Payments.Extensions;
using Feli.Payments.Providers.PayPal.Cofiguration;
using Feli.Payments.Providers.PayPal.Extensions;
using Sample.Web.Api.Events;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddTransient(sp => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services
    .AddPaymentUpdatedEvent<PaymentUpdatedEvent>()
    .Configure<PaymentOptions>(builder.Configuration.GetSection("Payments"))
    .AddPayments()
    .AddPaymentsRepositories()
    .Configure<PayPalOptions>(builder.Configuration.GetSection("Payments:PayPal"))
    .AddPayPal();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
