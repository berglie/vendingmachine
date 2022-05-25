using NetMQ;
using NetMQ.Sockets;

namespace Vendee.VendingMachine.SmsSystem;

public class MessageServer : IDisposable
{
    private readonly RequestSocket _requestSocket;

    public MessageServer(string address) => _requestSocket = new RequestSocket(address);

    public string ReceiveMessage() => _requestSocket.ReceiveFrameString();
    public void SendMessage(string message) => _requestSocket.SendFrame(message);
    public void Dispose() => _requestSocket?.Dispose();
}
