using VendingMachine.Models;
using VendingMachine.Services;

namespace VendingMachine;

internal class Program
{
    private static void Main(string[] args)
    {
        var inventory = new Inventory();
        var smsService = new SmsService();
        var vendingMachine = new Models.VendingMachine(inventory, smsService);
        vendingMachine.InsertMoney(100);
        vendingMachine.Initialize();
        vendingMachine.Start();
    }
}