using System.ComponentModel;

namespace VendingMachine.Models;

public enum MachineState
{
    Idle,
    [Description("Insert money - Insert money into slot")]
    InsertMoney,
    [Description("Order item - Order by machine buttons")]
    OrderItem,
    [Description("SMS order - Order by SMS")]
    SmsOrder,
    [Description("Recall - Refund money")]
    RecallMoney
}