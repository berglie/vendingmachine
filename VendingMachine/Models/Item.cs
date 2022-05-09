using VendingMachine.Interfaces;

namespace VendingMachine.Models;

public abstract class Item : IItem
{
    protected Item(string name, string manufacturer, decimal price)
    {
        Name = name;
        Manufacturer = manufacturer;
        Price = price;
    }

    public string Name { get; }

    public string Manufacturer { get; }

    public decimal Price { get; }

    public bool Equals(Item other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name && Manufacturer == other.Manufacturer;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Item)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Manufacturer);
}
