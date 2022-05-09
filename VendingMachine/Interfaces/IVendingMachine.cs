namespace Vendee.VendingMachine.Interfaces;

public interface IVendingMachine
{
    decimal Balance { get; }
    decimal InsertMoney(decimal money);
    decimal RefundMoney();
    IItem DispenseItem(IItem item);
    void Start();
}