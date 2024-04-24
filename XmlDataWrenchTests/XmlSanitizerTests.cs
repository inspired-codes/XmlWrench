using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlDataWrench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlDataWrench.Tests;

[TestClass()]
public class XmlSanitizerTests
{
    string xmlWithBadData = """<tag bla="asdf">Nice Info < Big Info</tag>""";
    string expected = """<tag bla="asdf">Nice Info &lt; Big Info</tag>""";

    // reconstituted
    [TestMethod()]
    public void GetOriginalTest()
    {
        var wrench = new XmlSanitizer(xmlWithBadData);
        IEnumerable<char> returned = wrench.GetOriginal();
        Assert.AreEqual(xmlWithBadData, new string(returned.ToArray()));
    }

    [TestMethod()]
    public void GetSanitizedTest()
    {
        var wrench = new XmlSanitizer(xmlWithBadData);
        wrench.RunXmlSanitizer();
        string result = wrench.GetSanitized();
        Assert.AreEqual(expected, result);
    }
}