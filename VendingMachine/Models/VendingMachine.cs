using Spectre.Console;
using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Interfaces;
using Vendee.VendingMachine.Utilities;

namespace Vendee.VendingMachine.Models;

public class VendingMachine
{
    private readonly IInventory _inventory;
    private readonly ISmsService _smsService;

    public VendingMachine(IInventory inventory, ISmsService smsService)
    {
        _inventory = inventory;
        _smsService = smsService;
    }

    public decimal Balance { get; private set; }

    public void Initialize()
    {
        _inventory.Add(new Soda("Coca Cola", "The Coca-Cola Company", 27, 0.33m), 1);
        _inventory.Add(new Soda("Fanta", "The Coca-Cola Company", 27, 0.33m), 1);
        _inventory.Add(new Soda("Sprite", "The Coca-Cola Company", 27, 0.33m), 0);
        _inventory.Add(new Soda("Solo", "Ringnes AS", 28, 0.33m), 10);
        _inventory.Add(new Beer("Frydenlund", "Ringnes AS", 52, 0.50m, 4.7m), 10);
    }

    public decimal InsertMoney(decimal money)
    {
        AnsiConsole.MarkupLine($"[green]Adding {money} to credit[/]");
        return Balance += money;
    }

    public decimal RefundMoney()
    {
        var refund = Balance;
        Balance = 0;
        AnsiConsole.MarkupLine($"[red]Returning {refund} to customer[/]");
        return refund;
    }

    public IItem DispenseItem(IItem item)
    {
        if (!HasSufficientFunds(item))
        {
            throw new InsufficientFundsException($"Out of balance! You are missing {item.Price - Balance} funds.");
        }

        _inventory.Deduct(item);
        Balance -= item.Price;
        ShowDispenseProgress(item);
        return item;
    }

    private bool HasSufficientFunds(IItem item) => Balance >= item.Price;

    public IItem DispenseItemFromSms()
    {
        var productName = _smsService.ReadSms();
        if (!_inventory.Contains(productName))
        {
            throw new ItemDoesNotExistException($"Vendeelicious does not have '{productName}' in it's inventory.");
        }

        var item = _inventory.GetItem(productName);
        _inventory.Deduct(item);
        ShowDispenseProgress(item);
        _smsService.SendSms($"{item.Name} was successfully dispensed.");
        return item;
    }

    /// <summary>
    /// This is the starter method for the machine
    /// </summary>
    public void Start()
    {
        var availableStates = Enum.GetValues(typeof(MachineState)).Cast<MachineState>()
            .Where(x => x != MachineState.Idle);

        while (true)
        {
            Console.Clear();
            ShowHeader();
            DisplayItems();

            var balance = new Panel($"Balance: [darkcyan]{Balance},-[/]");
            AnsiConsole.Write(balance);

            var state = AnsiConsole.Prompt(
                new SelectionPrompt<MachineState>()
                    .Title("\n[bold]Select item to purchase[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(availableStates)
                    .UseConverter(x => $"[yellow]{EnumHelper.GetDescription(x)}[/]"));

            switch (state)
            {
                case MachineState.Idle:
                    break;
                case MachineState.InsertMoney:
                    InsertMoneyPrompt();
                    break;
                case MachineState.OrderItem:
                    SelectItemsPrompt();
                    break;
                case MachineState.SmsOrder:
                    try
                    {
                        DispenseItemFromSms();
                    }
                    catch (TimeoutException) { }
                    catch (Exception e)
                    {
                        _smsService.SendSms($"Error: {e.Message}");
                    }
                    break;
                case MachineState.RefundMoney:
                    RefundMoney();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void InsertMoneyPrompt()
    {
        var money = AnsiConsole.Prompt(
            new TextPrompt<decimal>("How much money do you want to insert?")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]That's not a valid number![/]")
                .Validate(number =>
                {
                    return number switch
                    {
                        <= 0 => ValidationResult.Error("[red]You must insert a positive number[/]"),
                        _ => ValidationResult.Success(),
                    };
                }));
        InsertMoney(money);
    }

    private void SelectItemsPrompt()
    {
        try
        {
            var item = AnsiConsole.Prompt(
                new SelectionPrompt<IItem>()
                    .Title("\n[bold]Select item to purchase[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(_inventory.Items.Select(x => x.Key))
                    .UseConverter(x => $"[yellow]{x.Name}[/] [dim]({x.Manufacturer})[/]"));

            DispenseItem(item);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[underline red]{ex.Message}[/]");
            Console.ReadKey();
        }
    }

    private void DisplayItems()
    {
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Manufacturer");
        table.AddColumn("Price");
        table.AddColumn("Stock");
        table.Centered();
        foreach (var (item, stock) in _inventory.Items.OrderBy(x => x.Key.Name))
        {
            if (stock > 0)
            {
                table.AddRow(item.Name, item.Manufacturer, $"{item.Price},-", stock.ToString());
            }
            else
            {
                table.AddRow($"[strikethrough]{item.Name}[/]", $"[strikethrough]{item.Manufacturer}[/]", $"[strikethrough]{item.Price},-[/]", stock.ToString());
            }
        }

        AnsiConsole.Write(table);
    }

    private void ShowHeader()
    {
        AnsiConsole.Write(
            new FigletText("Vendeelicious")
                .Centered()
                .Color(Color.Red));
    }

    private void ShowDispenseProgress(IItem item)
    {
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task1 = ctx.AddTask($"Giving [green]{item.Name}[/] out");
                while (!ctx.IsFinished)
                {
                    task1.Increment(3);
                    Thread.Sleep(25);
                }
            });
    }
}
