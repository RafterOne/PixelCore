using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Linq;


namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for common XML-related tasks.
	/// </summary>
	public static class XmlHelper
	{
		/// <summary>
		/// Serializes an object to XML.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static XmlDocument SerializeObject<T>(T obj)
		{
			XmlDocument xmlDoc = new XmlDocument();
			XPathNavigator nav = xmlDoc.CreateNavigator();
			using (XmlWriter writer = nav.AppendChild())
			{
				XmlSerializer ser = new XmlSerializer(typeof(T));
				ser.Serialize(writer, obj);
			}
			return xmlDoc;
		}

		/// <summary>
		/// Serializes a list to XML.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="rootName"></param>
		/// <returns></returns>
		public static XmlDocument SerializeList<T>(List<T> obj, string rootName)
		{
			XmlDocument xmlDoc = new XmlDocument();
			XPathNavigator nav = xmlDoc.CreateNavigator();
			using (XmlWriter writer = nav.AppendChild())
			{
				XmlSerializer ser = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootName));
				ser.Serialize(writer, obj);
			}
			return xmlDoc;
		}

        /// <summary>
        /// Returns a new XML document with the given root element name.
        /// </summary>
        /// <param name="root">The name of the root element.</param>
        /// <returns>A new XMLDocument.</returns>
		public static XmlDocument CreateDocument(string root)
		{
			var doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement(root));
			return doc;
		}

        /// <summary>
        /// Creates a new XML Document from the provided XML string.
        /// </summary>
        /// <param name="xmlString">A string in XML format.</param>
        /// <returns>A new XMLDocument.</returns>
        public static XmlDocument GetXmlDocumentByString(string xmlString)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xmlString.Trim());
                return doc;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a new XDocument from the provded XML string.
        /// </summary>
        /// <param name="xmlString">A string in XML format.</param>
        /// <returns>A new XDocument.</returns>
        public static XDocument GetXDocument(string xmlString)
        {
            XmlDocument xmlDoc = GetXmlDocumentByString(xmlString);

            return GetXDocument(xmlDoc);
        }

        /// <summary>
        /// Creates a new XDocument from and XMLDocument.
        /// </summary>
        /// <param name="doc">An XMLDocument.</param>
        /// <returns>A new XDocument based on the provided XMLDocument.</returns>
        public static XDocument GetXDocument(XmlDocument doc)
        {
            return XDocument.Load(new XmlNodeReader(doc));
        }

        /// <summary>
        /// Returns an attribute from an XElement, if available. Otherwise, return null.
        /// </summary>
        /// <param name="element">An XElement to attempt to pull an attribute from.</param>
        /// <param name="attribute">The name of the attribute to pull.</param>
        /// <returns>A string if the attribute is present, otherwise null.</returns>
        public static string GetSafeAttributeValue(XElement element, string attribute)
        {
            try
            {
                return element.Attribute(attribute).Value;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
	}
}
