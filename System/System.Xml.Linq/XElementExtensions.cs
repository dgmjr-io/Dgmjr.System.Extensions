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
    /// <returns>A <see langword="string" />? .</returns>
    public static string? GetAttributeValue(this XE element, string attributeName)
    {
        var attribute = element.Attribute(attributeName);
        return attribute?.Value;
    }

    /// <summary>
    /// Extension method to convert a string to an XElement object.
    /// </summary>
    /// <param name="xml">The input XML string to be converted to XElement.</param>
    /// <param name="throwOnInvalidXml">A boolean flag indicating whether to throw an exception on invalid XML. True by default.</param>
    /// <returns>An XElement object representing the XML string, or a default XElement with name "InvalidXml" if conversion fails and throwOnInvalidXml is false.</returns>
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
    /// <returns>A string of the value at <paramref name="xpath"/>.</returns>
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
