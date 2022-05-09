using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Interfaces;

namespace Vendee.VendingMachine.Models;

public class VendingMachine
{
    private readonly IInventory _inventory;
    private readonly ISmsService _smsService;

    public VendingMachine(IInventory inventory, ISmsService smsService)
    {
        _inventory = inventory;
        _smsService = smsService;
    }

    public decimal Balance { get; private set; }

    public void Initialize()
    {
        _inventory.Add(new Soda("Coca Cola", "The Coca-Cola Company", 27, 0.33m), 1);
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

    public IItem DispenseItemFromSms()
    {
        var productName = _smsService.ReadSms();
        if (!_inventory.Contains(productName))
        {
            throw new ItemDoesNotExistException($"Vendeelicious does not have '{productName}' in it's inventory.");
        }

        var item = _inventory.GetItem(productName);
        _inventory.Deduct(item);
        _smsService.SendSms($"{item.Name} was successfully dispensed.");
        return item;
    }

    /// <summary>
    /// This is the starter method for the machine
    /// </summary>
    public void Start()
    {
        var availableStates = Enum.GetValues(typeof(MachineState)).Cast<MachineState>()
            .Where(x => x != MachineState.Idle);

        var state = MachineState.SmsOrder;

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
                    try
                    {
                        DispenseItemFromSms();
                    }
                    catch (TimeoutException tEx)
                    {
                        // Ignore
                    }
                    catch (Exception e)
                    {
                        _smsService.SendSms(e.Message);
                    }
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
