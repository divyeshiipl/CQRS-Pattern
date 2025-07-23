namespace Application.Common.Exceptions;

public class CustomErrorException : Exception
{
    public string CustomErrorMessage { get; }
    public string Name { get; }
    public CustomErrorException(string pName, string pCustomErrorMessage)
    {
        Name = pName;
        CustomErrorMessage = pCustomErrorMessage;
    }
}