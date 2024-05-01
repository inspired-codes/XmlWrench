namespace Inspired.Codes.XmlDataWrench;

public class InvalidXmlTooSmallException : Exception
{
    public override string Message => """minimum sequence length must be greater or equal seven""";
}
