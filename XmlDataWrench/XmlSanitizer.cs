using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlDataWrench;
public class XmlSanitizer
{
    private int Offset { get; set; }
    private char[] Original { get; } // ref
    private int OriginalLen { get; }
    private StringBuilder Sanitized { get; }

    public XmlSanitizer(IEnumerable<char> original)
    {
        ArgumentNullException.ThrowIfNull(original);

        if (original.Count() < "<a><a/>".Length) throw new InvalidXmlInput();

        Original = original.ToArray();
        OriginalLen = Original.Count();
        Offset = 0;
        Sanitized = new StringBuilder((int)(original.Count() * 1.5));
    }

    public string GetSanitized() => Sanitized.ToString();

    public IEnumerable<char> GetOriginal() => Original;

    /// <summary>
    /// sanitizes data block, assuming that TAGs are ok <br/>
    /// no validation for open-close consitency
    /// no sanitizing of data that looks like a &lt;TAG&gt;
    /// </summary>
    /// <exception cref="InvalidXmlInput"></exception>
    public void RunXmlSanitizer()
    {
        while (char.IsWhiteSpace(Original[Offset]))
            Offset++;

        char c = Original[Offset];
        if (c != '<')
            throw new InvalidXmlInput();

        AddChar(c);
        ParseTagAndRest();
    }

    private void ParseTagAndRest()
    {

        while ((Offset + 1) < OriginalLen)
        {
            Offset++;
            char c = Original[Offset];

            if (c is not '<' and not '>')
            {
                AddChar(c);
                continue;
            }
            if (c is '<')
            {
                AddLessThan();
                continue;
            }
            if (c is '>')
            {
                AddChar(c);
                ParseDataBlock();
            }
        }
    }

    private void ParseDataBlock()
    {
        //bool nextIsTagOpen;

        while ((Offset + 1) < OriginalLen)
        {
            Offset++;
            char c = Original[Offset];

            if (c is not '<' and not '>')
            {
                AddChar(c);
            }
            if (c is '<')
            {
                if (NextBracketIsTagOpen(Offset)) // can consume much resources
                {
                    AddLessThan();
                }
                else
                {
                    AddChar(c);
                    return;
                }
            }
            if (c is '>')
            {
                AddGreaterThan();
            }
        }
    }

    private bool NextBracketIsTagOpen(int offset, string tagName = "")
    {
        while ((offset + 1) < OriginalLen)
        {
            offset++;
            char c = Original[offset];
            if (c == '<')
                return true;
        }
        return false;
    }

    private void AddChar(char c) => Sanitized.Append(c);
    private void AddLessThan() => Sanitized.Append("&lt;");
    private void AddGreaterThan() => Sanitized.Append("&gt;");
}
