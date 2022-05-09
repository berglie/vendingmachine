using System.ComponentModel;

namespace VendingMachine.Utilities;

internal class EnumHelper
{
    public static string GetDescription(Enum en)
    {
        var type = en.GetType();
        var memberInfo = type.GetMember(en.ToString());
        if (memberInfo.Length <= 0)
        {
            return en.ToString();
        }

        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : en.ToString();
    }
}