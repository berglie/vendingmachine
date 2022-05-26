namespace Vendee.VendingMachine.Core.Exceptions;

public class ItemDoesNotExistException : Exception
{
    public ItemDoesNotExistException(string message) : base(message) { }
}
