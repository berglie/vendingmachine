using Vendee.VendingMachine.Console;
using Vendee.VendingMachine.Core.Models;
using Vendee.VendingMachine.Core.Services;
using Vendee.VendingMachine.SmsSystem;

var inventory = new Inventory();
var paymentService = new PaymentService();
var dispenseService = new DispenseService(inventory, paymentService);
var displayService = new DisplayService(inventory);
var smsService = new SmsService("@tcp://*:5555");
var vendingMachine = new VendingMachine(inventory, paymentService, dispenseService, smsService);
InitializeInventory();

while (true)
{
    Console.Clear();
    displayService.DisplayItems();
    displayService.DisplayBalance(paymentService.Balance);
    var selectedState = displayService.SelectStatePrompt();

    switch (selectedState)
    {
        case MachineState.Idle:
            break;
        case MachineState.InsertMoney:
            var insertAmount = displayService.DepositPrompt();
            vendingMachine.InsertMoney(insertAmount);
            break;
        case MachineState.OrderItem:
            try
            {
                var selectedItem = displayService.SelectItemPrompt();
                vendingMachine.DispenseItem(selectedItem);
                displayService.DisplayDispenseProgress(selectedItem);
            }
            catch (Exception ex)
            {
                displayService.DisplayError(ex.Message);
                Console.ReadKey();
            }
            break;
        case MachineState.SmsOrder:
            try
            {
                var item = displayService.ShowItemProgress(vendingMachine.GetItemFromSms);
                vendingMachine.DispenseItem(item);
                displayService.DisplayDispenseProgress(item);
                smsService.SendSms($"{item.Name} was successfully dispensed.");
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                smsService.SendSms($"Error: {e.Message}");
            }
            break;
        case MachineState.RefundMoney:
            var refundAmount = vendingMachine.RefundMoney();
            displayService.DisplayInfo($"Returned {refundAmount} to customer");
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}

void InitializeInventory()
{
    inventory.Add(new Soda("Coca Cola", "The Coca-Cola Company", 27, 0.33m), 1);
    inventory.Add(new Soda("Fanta", "The Coca-Cola Company", 27, 0.33m), 1);
    inventory.Add(new Soda("Sprite", "The Coca-Cola Company", 27, 0.33m), 0);
    inventory.Add(new Soda("Solo", "Ringnes AS", 28, 0.33m), 10);
    inventory.Add(new Beer("Frydenlund", "Ringnes AS", 52, 0.50m, 4.7m), 10);
}