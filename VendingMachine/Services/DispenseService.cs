using Vendee.VendingMachine.Core.Interfaces;

namespace Vendee.VendingMachine.Core.Services;

public class DispenseService : IDispenseService
{
    private readonly IInventory _inventory;
    private readonly IPaymentService _paymentService;

    public DispenseService(IInventory inventory, IPaymentService paymentService)
    {
        _inventory = inventory;
        _paymentService = paymentService;
    }

    public IItem DispenseItem(IItem item)
    {
        _inventory.Deduct(item);
        _paymentService.Withdraw(item.Price);
        return item;
    }
}
