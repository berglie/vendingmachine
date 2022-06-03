namespace Vendee.VendingMachine.Core.Interfaces;

public interface IDispenseService
{
    IItem DispenseItem(IItem item);
}