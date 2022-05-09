using NetMQ;
using NetMQ.Sockets;
using VendingMachine.Interfaces;

namespace VendingMachine.Services;

public class SmsService : ISmsService
{
    private const string SmsCodeWord = "VENDEE";
    private const int TimeoutSeconds = 10;
    private readonly ResponseSocket _responseSocket;

    public SmsService() => _responseSocket = new ResponseSocket("@tcp://*:5555");

    public string ReadSms()
    {
        var timeout = !_responseSocket.TryReceiveFrameString(TimeSpan.FromSeconds(TimeoutSeconds), out var receivedSms);

        if (timeout)
        {
            throw new TimeoutException("Did not receive any sms withing x seconds");
        }
        try
        {
            if (!receivedSms.ToUpperInvariant().StartsWith($"{SmsCodeWord} "))
            {
                _responseSocket.SendFrame($"SMS must start with code work {SmsCodeWord}");
            }

            var productName = receivedSms.Split(' ', 2)[1];
            return productName;
        }
        catch (Exception e)
        {
            _responseSocket.SendFrame(e.Message);
            throw;
        }
    }

    public void SendSms(string message) => _responseSocket.SendFrame(message);
}