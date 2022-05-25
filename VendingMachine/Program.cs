using Vendee.VendingMachine.Models;
using Vendee.VendingMachine.Services;

namespace Vendee.VendingMachine;

internal class Program
{
    private static void Main(string[] args)
    {
        var inventory = new Inventory();
        var paymentService = new PaymentService();
        var dispenseService = new DispenseService(inventory, paymentService);
        var displayService = new DisplayService(inventory);
        var smsService = new SmsService("@tcp://*:5555");
        var vendingMachine = new Models.VendingMachine(inventory, paymentService, dispenseService, displayService, smsService);
        vendingMachine.Initialize();
        vendingMachine.Start();
    }
}