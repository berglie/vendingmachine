using Vendee.VendingMachine.Models;
using Vendee.VendingMachine.Services;

namespace Vendee.VendingMachine;

internal class Program
{
    private static void Main(string[] args)
    {
        var inventory = new Inventory();
        var smsService = new SmsService("@tcp://*:5555");
        var vendingMachine = new Models.VendingMachine(inventory, smsService);
        vendingMachine.InsertMoney(100);
        vendingMachine.Initialize();
        vendingMachine.Start();
    }
}