# Payments

## Description
- Payments.API: Contains all the abstractions
- Payments: Default implementation of Payments.API
- Payments.Data.Dapper: Dapper implementation of the Payment repositories
- Payments.Database: Contains the sql project and the build project used to publish and import the database
- Payments.Providers.PayPal: PayPal payment provider implementation

## Set Up
- Install the following packages into your project:
  - Feli.Payments
  - Feli.Payments.Data.Dapper
  - Feli.Payments.Providers.PayPal
- Add to your appsettings.json and fill:
``` json
"Payments": {
  "BaseUrl": "Your application base url",
  "PayPal": {
    "Receiver": "The PayPal ID or email address that with receive the payment",
    "ReturnUrl": "The URL to which PayPal redirects buyers browser after they complete their payments.",
    "CancelReturnUrl": "A URL to which PayPal redirects the buyers browsers if they cancel checkout before completing their payments.",
    "ShoppingUrl": "The URL of the page on the merchant website that buyers go to when they click the Continue Shopping button on the PayPal Shopping Cart page.",
    "Sandbox": true
  }
}
```
- Create a class that inherits IPaymentUpdatedEvent to listen for payment state events
- Register your services like this:
```cs
builder.Services.AddOptions();
builder.Services.AddTransient(sp => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services
    .AddPaymentUpdatedEvent<PaymentUpdatedEvent>() // Its a class that inherits IPaymentUpdatedEvent to listen for payment state updates
    .Configure<PaymentOptions>(builder.Configuration.GetSection("Payments"))
    .AddPayments() // register the default implementation of the payments api services
    .AddPaymentsRepositories() // register the repositories dapper implementation
    .Configure<PayPalOptions>(builder.Configuration.GetSection("Payments:PayPal"))
    .AddPayPal(); // adds support for the paypal payment provider
```
- Clone this github repos and build `src/Payments.Database/Payments.Database.Build/Payments.Database.Build.csproj`
- Import `Payments.Database.Build.dacpac` into your sql project
- Publish your database project
- Everything is now ready ! To see how i implemented it, checkout the sample project
