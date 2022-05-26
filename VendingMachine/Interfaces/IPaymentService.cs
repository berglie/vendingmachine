namespace Vendee.VendingMachine.Core.Interfaces;

public interface IPaymentService
{
    decimal Balance { get; }
    decimal Withdraw(decimal money);
    decimal Deposit(decimal money);
    decimal RefundMoney();
}