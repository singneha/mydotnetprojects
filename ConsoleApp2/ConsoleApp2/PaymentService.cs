namespace Test;

interface IPaymentInterface
{
    void ProcessPayment(decimal amount);
}

class CreditCardPayment : IPaymentInterface
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount:C}");
    }
}

class PayPalPayment : IPaymentInterface
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount:C}");
    }
}

class PaymentProcessor
{
    private readonly IPaymentInterface _processor;
    public PaymentProcessor(IPaymentInterface processor)
    {
        _processor = processor;
    }

    public void Process(decimal amount)
    {
        _processor.ProcessPayment(amount);
    }
}
