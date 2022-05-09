using NetMQ;
using NetMQ.Sockets;

namespace VendeeMachine.SmsSystem;

public class MessageServer : IDisposable
{
    private readonly RequestSocket _requestSocket;

    public  MessageServer(string address) => _requestSocket = new RequestSocket(address);

    public void SendMessage(string message) => _requestSocket.SendFrame(message);
    public string ReceiveMessage() => _requestSocket.ReceiveFrameString();

    public void Dispose() => _requestSocket?.Dispose();
}
