using Vendee.VendingMachine.Models;

namespace Vendee.VendingMachine.Interfaces;

public interface IDisplayService
{
    void DisplayBalance(decimal balance);
    void DisplayItems();
    void DisplayDispenseProgress(IItem item);
    void DisplayError(string errorMessage);
    decimal DepositPrompt();
    MachineState SelectStatePrompt();
    IItem SelectItemPrompt();

}