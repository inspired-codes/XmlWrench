namespace Inspired.Codes.XmlDataWrench;

public class InvalidXmlEndException : Exception
{
    public override string Message => "Xml must end with a Tag, EOF after data block not allowed";
}
