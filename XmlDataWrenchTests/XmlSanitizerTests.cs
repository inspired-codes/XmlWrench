namespace Inspired.Codes.XmlDataWrench.Tests;

[TestClass()]
public class XmlSanitizerTests
{

    [TestInitialize]
    public void InitializeTest() { }

    [TestMethod()]
    public void GetSanitizedTest()
    {
        string xmlWithBadData = """<INFO language="en">Nice & Big Info, the cat size << dog size, but cat's tail << dog's tail</INFO>""";
        string expected = """<INFO language="en">Nice &amp; Big Info, the cat size &lt;&lt; dog size, but cat's tail &lt;&lt; dog's tail</INFO>""";

        var wrench = new XmlSanitizer(xmlWithBadData);
        wrench.RunXmlSanitizer();
        
        string result = wrench.GetSanitized();
        Assert.AreEqual(result, expected);
    }

    [TestMethod()]
    public void GetSanitizedTest2()
    {
        string xmlWithBadData = """<INFO language="en"></INFO>""";
        string expected = """<INFO language="en"></INFO>""";

        var wrench = new XmlSanitizer(xmlWithBadData);
        wrench.RunXmlSanitizer();

        string result = wrench.GetSanitized();
        Assert.AreEqual(result, expected);
    }

    [TestMethod()]
    public void GetSanitizedTest3()
    {
        string xmlWithBadData = """<INFO language="en"><<<>>></INFO>""";
        string expected = """<INFO language="en">&lt;&lt;&lt;&gt;&gt;&gt;</INFO>""";

        var wrench = new XmlSanitizer(xmlWithBadData);
        wrench.RunXmlSanitizer();

        string result = wrench.GetSanitized();
        Assert.AreEqual(result, expected);
    }

    [TestMethod()]
    public void GetSanitizedTest4()
    {
        string xmlWithBadData = """<INFO language="en"> < <<aloha>> > </INFO>""";
        string expected = """<INFO language="en"> &lt; &lt;&lt;aloha&gt;&gt; &gt; </INFO>""";

        var wrench = new XmlSanitizer(xmlWithBadData);
        wrench.RunXmlSanitizer();

        string result = wrench.GetSanitized();
        Assert.AreEqual(result, expected);
    }

    [TestMethod()]
    public void SkipLeadingWhiteCharsTest()
    {
        var original = "   <a>hallo</a>   ";
        var wrench = new XmlSanitizer(original);

        var startIdx = wrench.SkipLeadingWhiteChars();
        var trimmed = original[startIdx..];

        Assert.AreEqual(trimmed, "<a>hallo</a>   ");
    }

    [TestMethod()]
    public void SkipTailingWhiteCharsTest()
    {
        string original = "   <a>hallo</a>   ";
        var wrench = new XmlSanitizer(original);

        var endIdx = wrench.SkipTrailingWhiteChars();
        var trimmed = original.Substring(0, endIdx + 1);

        Assert.AreEqual(trimmed, "   <a>hallo</a>");
    }
}