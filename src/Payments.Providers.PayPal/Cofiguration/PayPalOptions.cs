namespace Feli.Payments.Providers.PayPal.Cofiguration
{
    public class PayPalOptions
    {
        /// <summary>
        /// The PayPal ID or email address that with receive the payment
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// The URL to which PayPal redirects buyers' browser after they complete their payments.
        /// </summary>
        public string ReturnUrl { get; set; } = string.Empty;

        /// <summary>
        /// A URL to which PayPal redirects the buyers' browsers if they cancel checkout before completing their payments.
        /// </summary>
        public string CancelReturnUrl { get; set; } = string.Empty;

        /// <summary>
        /// The URL of the page on the merchant website that buyers go to when they click the Continue Shopping button on the PayPal Shopping Cart page.
        /// </summary>
        public string ShoppingUrl { get; set; } = string.Empty;

        /// <summary>
        /// Determine if the enviroment that will be used is sandbox.
        /// </summary>
        public bool Sandbox { get; set; } = false;
    }
}
