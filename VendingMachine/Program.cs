using VendingMachine.Models;

namespace VendingMachine;

internal class Program
{
    private static void Main(string[] args)
    {
        var vendingMachine = new Models.VendingMachine();
        vendingMachine.Start();
    }
}