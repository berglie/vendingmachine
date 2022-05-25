using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Interfaces;

namespace Vendee.VendingMachine.Models;

public class VendingMachine : IVendingMachine
{
    private readonly IInventory _inventory;
    private readonly IPaymentService _paymentService;
    private readonly IDispenseService _dispenseService;
    private readonly ISmsService _smsService;
    private readonly IDisplayService _displayService;

    public VendingMachine(
        IInventory inventory,
        IPaymentService paymentService,
        IDispenseService dispenseService,
        IDisplayService displayService,
        ISmsService smsService)
    {
        _inventory = inventory;
        _paymentService = paymentService;
        _dispenseService = dispenseService;
        _displayService = displayService;
        _smsService = smsService;
    }

    public decimal Balance => _paymentService.Balance;

    public void Initialize()
    {
        _inventory.Add(new Soda("Coca Cola", "The Coca-Cola Company", 27, 0.33m), 1);
        _inventory.Add(new Soda("Fanta", "The Coca-Cola Company", 27, 0.33m), 1);
        _inventory.Add(new Soda("Sprite", "The Coca-Cola Company", 27, 0.33m), 0);
        _inventory.Add(new Soda("Solo", "Ringnes AS", 28, 0.33m), 10);
        _inventory.Add(new Beer("Frydenlund", "Ringnes AS", 52, 0.50m, 4.7m), 10);
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
            _displayService.DisplayItems();
            _displayService.DisplayBalance(_paymentService.Balance);
            var selectedState = _displayService.SelectStatePrompt();

            switch (selectedState)
            {
                case MachineState.Idle:
                    break;
                case MachineState.InsertMoney:
                    var insertAmount = _displayService.DepositPrompt();
                    InsertMoney(insertAmount);
                    break;
                case MachineState.OrderItem:
                    try
                    {
                        var selectedItem = _displayService.SelectItemPrompt();
                        DispenseItem(selectedItem);
                        _displayService.DisplayDispenseProgress(selectedItem);
                    }
                    catch (Exception ex)
                    {
                        _displayService.DisplayError(ex.Message);
                        Console.ReadKey();
                    }
                    break;
                case MachineState.SmsOrder:
                    try
                    {
                        var item = GetItemFromSms();
                        DispenseItem(item);
                        _displayService.DisplayDispenseProgress(item);
                        _smsService.SendSms($"{item.Name} was successfully dispensed.");
                    }
                    catch (TimeoutException) { }
                    catch (Exception e)
                    {
                        _smsService.SendSms($"Error: {e.Message}");
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

    public decimal InsertMoney(decimal amount) =>  _paymentService.Deposit(amount);

    public decimal RefundMoney() => _paymentService.RefundMoney();

    public IItem DispenseItem(IItem item) => _dispenseService.DispenseItem(item);

    public IItem GetItemFromSms()
    {
        var productName = _smsService.ReadSms();
        if (!_inventory.Contains(productName))
        {
            throw new ItemDoesNotExistException($"Vendeelicious does not have '{productName}' in it's inventory.");
        }

        return _inventory.GetItem(productName);
    }
}