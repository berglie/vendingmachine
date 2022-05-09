namespace VendingMachine.Interfaces;

public interface ISmsService
{
    string ReadSms();
    void SendSms(string message);
}