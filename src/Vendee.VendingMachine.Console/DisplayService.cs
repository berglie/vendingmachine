using Spectre.Console;
using Vendee.VendingMachine.Console.Utilities;
using Vendee.VendingMachine.Core.Interfaces;
using Vendee.VendingMachine.Core.Models;

namespace Vendee.VendingMachine.Console;

public class DisplayService
{
    private readonly IInventory _inventory;
    private readonly IEnumerable<MachineState> _states;

    public DisplayService(IInventory inventory)
    {
        _inventory = inventory;
        _states = Enum.GetValues(typeof(MachineState))
            .Cast<MachineState>()
            .Where(x => x != MachineState.Idle);
    }

    public void DisplayBalance(decimal balance)
    {
        var balancePanel = new Panel($"Balance: [darkcyan]{balance},-[/]");
        AnsiConsole.Write(balancePanel);
    }

    public void DisplayItems()
    {
        ShowHeader();
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

    public void DisplayDispenseProgress(IItem item)
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

    public void DisplayError(string errorMessage) => AnsiConsole.MarkupLine($"\n[underline red]{errorMessage}[/]");

    public void DisplayInfo(string infoMessage) => AnsiConsole.MarkupLine(infoMessage);

    public decimal DepositPrompt()
    {
        var depositAmount =  AnsiConsole.Prompt(
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

        AnsiConsole.MarkupLine($"[green]Adding {depositAmount} to credit[/]");
        return depositAmount;
    }

    public MachineState SelectStatePrompt()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MachineState>()
                .Title("\n[bold]Select item to purchase[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                .AddChoices(_states)
                .UseConverter(x => $"[yellow]{EnumHelper.GetDescription(x)}[/]"));
    }

    public IItem ShowItemProgress(Func<IItem> func)
    {
        return AnsiConsole.Status()
            .Start("Waiting for SMS...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Clock);
                return func();
            });
    }

    public IItem SelectItemPrompt()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<IItem>()
                .Title("\n[bold]Select item to purchase[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                .AddChoices(_inventory.Items.Select(x => x.Key))
                .UseConverter(x => $"[yellow]{x.Name}[/] [dim]({x.Manufacturer})[/]"));
    }

    private void ShowHeader()
    {
        AnsiConsole.Write(
            new FigletText("Vendeelicious")
                .Centered()
                .Color(Color.Red));
    }
}
