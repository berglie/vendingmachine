using Vendee.VendingMachine.Core.Exceptions;
using Vendee.VendingMachine.Core.Interfaces;

namespace Vendee.VendingMachine.Core.Models;

public class VendingMachine : IVendingMachine
{
    private readonly IInventory _inventory;
    private readonly IPaymentService _paymentService;
    private readonly IDispenseService _dispenseService;
    private readonly ISmsService _smsService;

    public VendingMachine(
        IInventory inventory,
        IPaymentService paymentService,
        IDispenseService dispenseService,
        ISmsService smsService)
    {
        _inventory = inventory;
        _paymentService = paymentService;
        _dispenseService = dispenseService;
        _smsService = smsService;
    }

    public decimal Balance => _paymentService.Balance;

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