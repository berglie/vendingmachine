namespace Vendee.VendingMachine.Core.Interfaces;

public interface ISmsService
{
    string ReadSms();
    void SendSms(string message);
}