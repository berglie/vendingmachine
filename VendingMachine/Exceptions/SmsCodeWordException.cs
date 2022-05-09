namespace Vendee.VendingMachine.Exceptions;

public class SmsCodeWordException : Exception
{
    public SmsCodeWordException(string message) : base(message) { }
}