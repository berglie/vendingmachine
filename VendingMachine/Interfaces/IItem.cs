using Vendee.VendingMachine.Core.Models;

namespace Vendee.VendingMachine.Core.Interfaces;

public interface IItem : IEquatable<Item>
{
    string Name { get; }

    string Manufacturer { get; }

    decimal Price { get; }
}