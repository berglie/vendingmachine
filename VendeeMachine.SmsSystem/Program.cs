using NetMQ;
using NetMQ.Sockets;

using var requestSocket = new RequestSocket(">tcp://localhost:5555");

while (true)
{
    Console.WriteLine("Input SMS: ");
    var message = Console.ReadLine();
    requestSocket.SendFrame(message);
    var response = requestSocket.ReceiveFrameString();
    Console.WriteLine($"Response: {response}");
}