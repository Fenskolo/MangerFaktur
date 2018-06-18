using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EasyInvoice
{
    public  static class HelperXML
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

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static DataTable Stam(string data)
        {
            StringReader theReader = new StringReader(data);
            DataTable table = new DataTable
            {
                TableName = "TabelaFaktura"
            };
            table.Columns.Add(DictionaryMain.kolumnaTowar, typeof(string));
            table.Columns.Add(DictionaryMain.kolumnaJM);
            table.Columns.Add(DictionaryMain.kolumnaIlosc, typeof(Int32));
            table.Columns.Add(DictionaryMain.kolumnaCenaNetto, typeof(decimal));
            table.Columns.Add(DictionaryMain.kolumnaWartoscNetto, typeof(decimal));
            table.Columns.Add(DictionaryMain.kolumnaStawkaVat);
            table.Columns.Add(DictionaryMain.kolumnaKwotaVat, typeof(decimal));
            table.Columns.Add(DictionaryMain.kolumnaWartoscBrutto, typeof(decimal));
            table.ReadXml(theReader);

            return table;
        }

        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static T DeserializeObject<T>(string xml)
        where T : new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new T();
            }
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception)
            {
                return new T();
            }
        }
    }
}
