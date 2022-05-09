namespace Vendee.VendingMachine.Exceptions;

public class ItemDoesNotExistException : Exception
{
    public ItemDoesNotExistException(string message) : base(message) { }
}
