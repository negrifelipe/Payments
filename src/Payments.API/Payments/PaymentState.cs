namespace Feli.Payments.API.Payments
{
    public enum PaymentState
    {
        Completed = 0,
        Cancelled = 1,
        Refunded = 2,
        Reversed = 3,
        Waiting = 4
    }
}
