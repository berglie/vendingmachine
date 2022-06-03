﻿using Vendee.VendingMachine.Models;

namespace Vendee.VendingMachine.Interfaces;

public interface IItem : IEquatable<Item>
{
    string Name { get; }
    string Manufacturer { get; }
    decimal Price { get; }
    bool Equals(Item other);
    bool Equals(object obj);
    int GetHashCode();
}