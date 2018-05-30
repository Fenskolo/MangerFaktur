using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasyInvoice
{
    public class HelperXML
    {
        private static string xmlFile = "faktura.xml";
        public static SingleFakturaProperty Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(SingleFakturaProperty));
                reader = new StreamReader(xmlFile);
                return (SingleFakturaProperty)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static void SerializeXml()
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(SingleFakturaProperty));
                writer = new StreamWriter(xmlFile, false);
                xsSubmit.Serialize(writer, SingleFakturaProperty.Singleton.MySingleton);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
