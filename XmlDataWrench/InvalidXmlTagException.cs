namespace Ice.XmlDataWrench;

public class InvalidXmlTagException : Exception
{
    public override string Message => "Xml must have valid TAG's ending with '>'";
}
