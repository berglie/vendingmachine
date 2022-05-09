using Spectre.Console;
using Vendee.VendingMachine.SmsSystem;

ShowHeader();

var messageServer = new MessageServer(">tcp://localhost:5555");

while (true)
{
    var message = AnsiConsole.Ask<string>("\nWhat's your [blue]message[/]?\n");
    messageServer.SendMessage(message);
    var response = AnsiConsole.Status()
        .Start($"'[bold yellow]{message}[/]' was sent, waiting for response...", ctx =>
        {
            ctx.Spinner(Spinner.Known.Clock);
            return messageServer.ReceiveMessage();
        });

    var fontColor = response.ToLowerInvariant().StartsWith("error") ? "red" : "green";
    AnsiConsole.Write(new Markup($"[dim]Response from Vendeelicious:[/] '[bold {fontColor}]{response}[/]'"));
    Console.WriteLine();
}

void ShowHeader()
{
    AnsiConsole.Write(
        new FigletText("Vendee SMS System")
            .Centered()
            .Color(Color.Green));
}