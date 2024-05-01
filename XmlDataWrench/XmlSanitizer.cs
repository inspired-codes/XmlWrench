using System.Text;

namespace Ice.XmlDataWrench;
public class XmlSanitizer
{
    public const double FactorForOutputSize = 1.2; // estimate required buffer

    private int HeadIndex { get; set; }
    private char[] Original { get; } // ref
    private int LastIndex { get; set; }
    private StringBuilder Sanitized { get; }
    private bool CanReadNextChar => HeadIndex < LastIndex;

    public XmlSanitizer(IEnumerable<char> original)
    {
        if (original is null) throw new ArgumentNullException(nameof(original));

        Original = original.ToArray();
        HeadIndex = 0;
        LastIndex = Original.Length - 1;
        Sanitized = new StringBuilder((int)(original.Count() * FactorForOutputSize));
    }

    public string GetSanitized() => Sanitized.ToString();


    /// <summary>
    /// sanitizes data block, assuming that TAGs are ok <br/>
    /// no validation for open-close consitency
    /// no sanitizing of data that looks like a &lt;TAG&gt;
    /// </summary>
    /// <exception cref="InvalidXmlStartException"></exception>
    public void RunXmlSanitizer()
    {
        SkipLeadingWhiteChars();

        SkipTrailingWhiteChars();

        ThrowIfTooShort();

        while (CanReadNextChar)
            ParseTagAndRest();
    }

    /// <summary>
    /// minimum length of xml data must be 7 chars
    /// </summary>
    /// <exception cref="InvalidXmlTooSmallException"></exception>
    private void ThrowIfTooShort()
    {
        if ((LastIndex - HeadIndex) <= 7)
            throw new InvalidXmlTooSmallException();
    }

    /// <summary>
    /// set head index to first non-white char index<br/>
    /// does noch check char is valid start tag char
    /// </summary>
    /// <returns>index of first non-whitespace char</returns>
    public int SkipLeadingWhiteChars()
    {
        while (CanReadNextChar)
        {
            if (char.IsWhiteSpace(Original[HeadIndex]))
                HeadIndex++;
            else
                break;
        }
        return HeadIndex;
    }

    /// <summary>
    /// set last index to last close tag char '>' only followed by white-space chars
    /// </summary>
    /// <returns>index of last tag close char before whitespace chars</returns>
    /// <exception cref="InvalidXmlEndException"></exception>
    public int SkipTrailingWhiteChars()
    {
        int tail = LastIndex;

        while (char.IsWhiteSpace(Original[tail]))
            tail--;

        // expected tag end char '>'
        if (Original[tail] != '>')
            throw new InvalidXmlEndException();

        LastIndex = tail;
        return LastIndex;
    }

    /// <summary>
    /// copies tag to sanitized buffer <br/>
    /// and sanitizes data block between tags
    /// </summary>
    /// <exception cref="InvalidXmlTagException"></exception>
    private void ParseTagAndRest()
    {
        char c = Original[HeadIndex];
        if (c != '<')
            throw new InvalidXmlTagException();

        AddChar(Original[HeadIndex]);
        while (CanReadNextChar)
        {
            HeadIndex++;
            c = Original[HeadIndex];

            if (c is '<')
            {
                throw new InvalidXmlTagException();
            }
            if (c is '>')
            {
                AddChar(c);
                ParseDataBlock();
                return;
            }
            else //(c is not '<' and not '>')
            {
                AddChar(c);
                continue;
            }
        }
    }

    private void ParseDataBlock()
    {
        if (!CanReadNextChar)
            return;

        while (CanReadNextChar)
        {
            HeadIndex++;
            char c = Original[HeadIndex];

            if (c is '&')
            {
                AddAmpersand();
                continue;
            }

            if (c is '>')
            {
                AddGreaterThan();
                continue;
            }

            if (c is '<')
            {
                if (NextBracketIsTagOpen(HeadIndex))  // TODO: optimize, can consume much resources
                {
                    AddLessThan();
                    continue;
                }
                else // c == '<', return to parse tag
                {
                    return;
                }
            }

            AddChar(c);
        }

        throw new InvalidXmlEndException();
    }

    /// <summary>
    /// looks ahead, if in following sequence, the next char is a '&lt;', tag open char
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private bool NextBracketIsTagOpen(int offset)
    {
        while ((offset + 1) < LastIndex)
        {
            offset++;
            char c = Original[offset];
            if (c == '<')
                return true;
        }
        return false;
    }

    private void AddChar(char c) => Sanitized.Append(c);
    private void AddAmpersand() => Sanitized.Append("&amp;");
    private void AddLessThan() => Sanitized.Append("&lt;");
    private void AddGreaterThan() => Sanitized.Append("&gt;");
}
