namespace Inspired.Codes.XmlDataWrench;

public class InvalidXmlStartException : Exception
{
    public override string Message => """first character must be a '<'""";
}
