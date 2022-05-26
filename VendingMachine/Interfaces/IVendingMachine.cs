namespace Vendee.VendingMachine.Core.Interfaces;

public interface IVendingMachine
{
    decimal InsertMoney(decimal amount);
    decimal RefundMoney();
    IItem GetItemFromSms();
}