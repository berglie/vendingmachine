namespace Vendee.VendingMachine.Interfaces;

public interface IDispenseService
{
    IItem DispenseItem(IItem item);
}