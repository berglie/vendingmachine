using VendingMachine.Models;

namespace VendingMachine;

internal class Program
{
    private static void Main(string[] args)
    {
        var inventory = new Inventory();
        var vendingMachine = new Models.VendingMachine(inventory);
        vendingMachine.InsertMoney(100);
        vendingMachine.Start();
    }
}