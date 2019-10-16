using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace EasyInvoice
{
    public static class HelperXml
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null) return string.Empty;
            try
            {
                var serializers = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    serializers.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static T DeserializeObject<T>(string xml) where T : new()
        {
            if (string.IsNullOrEmpty(xml)) return new T();
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T) serializer.Deserialize(stringReader);
                }
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}