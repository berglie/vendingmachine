using Vendee.VendingMachine.Core.Exceptions;
using Vendee.VendingMachine.Core.Interfaces;

namespace Vendee.VendingMachine.Core.Services;

public class PaymentService : IPaymentService
{
    public decimal Balance { get; private set; }

    public decimal Withdraw(decimal money)
    {
        if (!HasSufficientFunds(money))
        {
            throw new InsufficientFundsException($"Out of balance! You are missing {money - Balance} funds.");
        }

        Balance -= money;
        return Balance;
    }

    public decimal Deposit(decimal money) => Balance += money;

    public decimal RefundMoney()
    {
        var refund = Balance;
        Balance = 0;
        return refund;
    }

    private bool HasSufficientFunds(decimal money) => Balance >= money;
}
