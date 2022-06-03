namespace Vendee.VendingMachine.Core.Models;

public class Drink : Item
{
    public Drink(string name, string manufacturer, decimal price, decimal volume) : base(name, manufacturer, price) => Volume = volume;

    public decimal Volume { get; }
}
