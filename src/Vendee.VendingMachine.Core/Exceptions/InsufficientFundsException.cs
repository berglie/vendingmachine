namespace Vendee.VendingMachine.Core.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message) { }
}
