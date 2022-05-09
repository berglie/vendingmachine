using VendingMachine.Exceptions;
using VendingMachine.Interfaces;

namespace VendingMachine.Models;

public class VendingMachine
{
    private readonly IInventory _inventory;

    public VendingMachine(IInventory inventory)
    {
        _inventory = inventory;
        Initialize();
    }

    public decimal Balance { get; private set; }

    private void Initialize()
    {
        _inventory.Add(new Soda("Fanta", "The Coca-Cola Company", 27, 0.33m), 1);
        _inventory.Add(new Soda("Sprite", "The Coca-Cola Company", 27, 0.33m), 0);
        _inventory.Add(new Soda("Solo", "Ringnes AS", 28, 0.33m), 10);
        _inventory.Add(new Beer("Frydenlund", "Ringnes AS", 52, 0.50m, 4.7m), 10);
    }

    public decimal InsertMoney(decimal money) => Balance += money;

    public decimal RefundMoney()
    {
        var refund = Balance;
        Balance = 0;
        return refund;
    }

    public IItem DispenseItem(IItem item)
    {
        if (!HasSufficientFunds(item))
        {
            throw new InsufficientFundsException($"Out of balance! You are missing {item.Price - Balance} funds.");
        }

        _inventory.Deduct(item);
        Balance -= item.Price;
        return item;
    }

    private bool HasSufficientFunds(IItem item) => Balance >= item.Price;

    /// <summary>
    /// This is the starter method for the machine
    /// </summary>
    public void Start()
    {
        var availableStates = Enum.GetValues(typeof(MachineState)).Cast<MachineState>()
            .Where(x => x != MachineState.Idle);

        var state = MachineState.OrderItem;

        while (true)
        {
            switch (state)
            {
                case MachineState.Idle:
                    break;
                case MachineState.InsertMoney:
                    InsertMoney(42);
                    Console.WriteLine(Balance);
                    Console.ReadKey();
                    break;
                case MachineState.OrderItem:
                    DispenseItem(_inventory.Items.First().Key);
                    break;
                case MachineState.SmsOrder:
                    break;
                case MachineState.RefundMoney:
                    RefundMoney();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
