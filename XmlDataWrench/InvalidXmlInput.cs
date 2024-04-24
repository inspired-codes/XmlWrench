namespace XmlDataWrench;

public class InvalidXmlInput : Exception
{
    public override string Message => """first character must be a '<'""";
}
