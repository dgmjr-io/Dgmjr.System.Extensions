// // // #if !NETSTANDARD1_0
// #if NETSTANDARD2_0 || NETSTANDARD2_1// && !N`ET6_0_OR_GREATER
// extern alias XDoc;
// extern alias XPathDoc;
// using XPathDoc::System.Xml.XPath;
// using XA = XDoc::System.Xml.Linq.XAttribute;
// using XC = XDoc::System.Xml.Linq.XComment;
// using XD = XDoc::System.Xml.Linq.XDocument;
// using XE = XDoc::System.Xml.Linq.XElement;
// using XN = XDoc::System.Xml.Linq.XName;
// using XNS = XDoc::System.Xml.Linq.XNamespace;
// using XO = XDoc::System.Xml.Linq.XNode;
// #else
// using XA = System.Xml.Linq.XAttribute;
// using XC = System.Xml.Linq.XComment;
// using XD = System.Xml.Linq.XDocument;
// using XE = System.Xml.Linq.XElement;
// using XN = System.Xml.Linq.XName;
// using XNS = System.Xml.Linq.XNamespace;
// using XO = System.Xml.Linq.XNode;
// #endif
using System;

namespace System.Xml.Linq;
using System.Xml.XPath;

/// <summary>
/// A set of extensions for <see cref="XE" />s.
/// </summary>
public static class XElementExtensions
{
    /// <summary>
    /// Gets the attribute value.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="attributeName">The attribute name.</param>
    /// <returns>A string? .</returns>
    public static string? GetAttributeValue(this XE element, string attributeName)
    {
        var attribute = element.Attribute(attributeName);
        return attribute?.Value;
    }

    public static XE ToXElement(this string xml, bool throwOnInvalidXml = true)
    {
        try
        {
            return XE.Parse(xml);
        }
        catch (Exception ex)
        {
            if (throwOnInvalidXml)
            {
                throw new ArgumentException("The XML was invalid.", nameof(xml), ex);
            }

            return new XE("InvalidXml");
        }
    }

    /// <summary>
    /// Selects the xpath.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="xpath">The xpath.</param>
    /// <returns>An array of <see cref="XE"/>s.</returns>
    public static XE[] SelectXpath(this XE element, string xpath)
    {
        // #if !NETSTANDARD2_0_OR_GREATER
        //         throw new PlatformNotSupportedException("This method is not supported on this platform.");
        // #else
        return element.XPathSelectElements(xpath)?.ToArray() ?? Empty<XE>();
        // #endif
    }

    /// <summary>
    /// Selects the xpath value.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="xpath">The xpath.</param>
    /// <returns>A string of the value at xpath <paramref name="xpath"/>.</returns>
    public static string? SelectXpathValue(this XE element, string xpath)
    {
#if !NETSTANDARD2_0_OR_GREATER
        throw new PlatformNotSupportedException("This method is not supported on this platform.");
#else
        return element.SelectXpath(xpath).FirstOrDefault()?.Value;
#endif
    }

    /// <summary>
    /// Parses the <paramref name="xml"/> and returns the <paramref name="xpath"/>.
    /// </summary>
    /// <param name="xml">The xml.</param>
    /// <param name="xpath">The xpath.</param>
    /// <param name="throwOnInvalidXml">If true, throw on invalid xml.</param>
    /// <returns>An array of <see cref="XE"/>s.</returns>
    /// <exception>If the XML or the xpath was invalid and <paramref name="throwOnInvalidXml"/> is set to <see langword="true" />.</exception>
    public static XE[] SelectXpath(this string xml, string xpath, bool throwOnInvalidXml = true)
    {
#if !NETSTANDARD2_0_OR_GREATER
        throw new PlatformNotSupportedException("This method is not supported on this platform.");
#else
        try
        {
            var document = XD.Parse(xml);
            return document.XPathSelectElements(xpath).ToArray();
        }
        catch
        {
            if (throwOnInvalidXml)
            {
                throw;
            }

            return Empty<XE>();
        }
#endif
    }
}
