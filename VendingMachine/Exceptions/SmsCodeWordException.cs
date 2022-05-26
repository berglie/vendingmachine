namespace Vendee.VendingMachine.Core.Exceptions;

public class SmsCodeWordException : Exception
{
    public SmsCodeWordException(string message) : base(message) { }
}