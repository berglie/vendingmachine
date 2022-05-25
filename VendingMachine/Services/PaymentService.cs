using Spectre.Console;
using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Interfaces;

namespace Vendee.VendingMachine.Services;

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

    public decimal Deposit(decimal money)
    {
        AnsiConsole.MarkupLine($"[green]Adding {money} to credit[/]");
        return Balance += money;
    }

    public decimal RefundMoney()
    {
        var refund = Balance;
        Balance = 0;
        AnsiConsole.MarkupLine($"[red]Returning {refund} to customer[/]");
        return refund;
    }

    private bool HasSufficientFunds(decimal money) => Balance >= money;
}
