namespace ECOMM.Core.Enums
{
    public enum PaymentMethodEnum
    {
        CreditCard = 1,
        BankTransfer = 2,
        CashOnDelivery = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Shipped = 2,
        Delivered = 3,
        Canceled = 4
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3
    }
}
