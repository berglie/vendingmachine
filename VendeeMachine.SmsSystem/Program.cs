using VendeeMachine.SmsSystem;

var messageServer = new MessageServer(">tcp://localhost:5555");

while (true)
{
    Console.WriteLine("Input SMS: ");
    var message = Console.ReadLine();
    messageServer.SendMessage(message);
    var response = messageServer.ReceiveMessage();
    Console.WriteLine($"Response: {response}");
}