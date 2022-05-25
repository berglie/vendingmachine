namespace Vendee.VendingMachine.Interfaces;

public interface IVendingMachine
{
    void Start();
    decimal InsertMoney(decimal amount);
    decimal RefundMoney();
    IItem GetItemFromSms();
}