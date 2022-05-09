namespace Vendee.VendingMachine.Interfaces;

public interface IInventory
{
    Dictionary<IItem, int> Items { get; }
    int Count { get; }
    void Add(IItem item, int quantity = 1);
    void Remove(IItem item);
    void Deduct(IItem item, int quantity = 1);
    bool HasItem(IItem item);
    bool Contains(string productName);
    IItem GetItem(string productName);
    void Clear();
}