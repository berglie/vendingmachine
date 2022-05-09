namespace VendingMachine.Models;

public class Soda : Drink
{
    public Soda(string name, string manufacturer, decimal price, decimal volume) : base(name, manufacturer, price, volume) { }

    public static Soda CreateDefault(string name, string manufacturer, decimal price) => new(name, manufacturer, price, 0.5m);
}