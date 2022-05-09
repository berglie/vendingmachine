using NetMQ;
using NetMQ.Sockets;
using Spectre.Console;
using Vendee.VendingMachine.Exceptions;
using Vendee.VendingMachine.Interfaces;

namespace Vendee.VendingMachine.Services;

public class SmsService : ISmsService, IDisposable
{
    private const string SmsCodeWord = "VENDEE";
    private const int TimeoutSeconds = 10;
    private readonly ResponseSocket _responseSocket;

    public SmsService(string address) => _responseSocket = new ResponseSocket(address);

    public string ReadSms()
    {
        return AnsiConsole.Status()
            .Start("Waiting for SMS...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Clock);
                var timeout =
                    !_responseSocket.TryReceiveFrameString(TimeSpan.FromSeconds(TimeoutSeconds), out var receivedSms);

                if (timeout)
                {
                    throw new TimeoutException("Did not receive any sms withing x seconds");
                }

                if (!receivedSms.ToUpperInvariant().StartsWith($"{SmsCodeWord} "))
                {
                    throw new SmsCodeWordException($"SMS must start with code work {SmsCodeWord}");
                }

                var productName = receivedSms.Split(' ', 2)[1];
                return productName;
            });
    }

    public void SendSms(string message) => _responseSocket.SendFrame(message);

    public void Dispose() => _responseSocket?.Dispose();
}