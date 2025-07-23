namespace Domain.Common;

public static class EnumHelper
{
    public static bool IsEnumValueExists(string value, Type pType)
    {
        return Enum.IsDefined(pType, value);
    }
    public static bool IsEnumIdExists(int value, Type pType)
    {
        return Enum.IsDefined(pType, value);
    }
    public static bool IsEnumIdExists(string value, Type pType)
    {
        if (!string.IsNullOrEmpty(value))
            return Enum.IsDefined(pType, Int32.Parse(value));
        else return false;
    }
}
