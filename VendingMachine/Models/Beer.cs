namespace Vendee.VendingMachine.Models;

public class Beer : Drink
{
    public Beer(string name, string manufacturer, decimal price, decimal volume, decimal alcoholPercentage) : base(name, manufacturer, price, volume) => AlcoholPercentage = alcoholPercentage;

    public decimal AlcoholPercentage { get; }

    public static Beer Create(string name, string manufacturer, decimal price, decimal volume, decimal alcoholPercentage) => new(name, manufacturer, price, volume, alcoholPercentage);

    public static Beer CreateDefault(string name, string manufacturer, decimal price) => new(name, manufacturer, price, 0.5m, 4.7m);
}