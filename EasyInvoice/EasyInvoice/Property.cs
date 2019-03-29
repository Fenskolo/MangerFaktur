using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable]
    public class Property
    {
        private static Property m_Instance;
        private static readonly string xmlPath = "xmlProperty.xml";
        private List<string> m_NameList;
        private List<string> m_StawkaList;
        private List<WorkClass> m_Works;
        public Property() { }

        public static Property Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    if (File.Exists(xmlPath))
                    {
                        m_Instance = Deserialize();
                    }
                    else
                    {
                        m_Instance = new Property();
                    }
                }
                return m_Instance;
            }
            set => m_Instance = value;
        }

        public List<string> NameList
        {
            get
            {
                if (m_NameList == null || m_NameList.Count == 0)
                {
                    m_NameList = new List<string>()
                    {
                        "szt.",
                        "godz.",
                        "m2",
                        "op.",
                        "mc.",
                        "ton",
                        "km.",
                        "m3."
                    };
                }
                return m_NameList;
            }
            set
            {
                value.Where(f => NameList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                m_NameList = value;
            }
        }

        public List<string> StawkaList
        {
            get
            {
                if (m_StawkaList == null || m_StawkaList.Count == 0)
                {
                    m_StawkaList = new List<string>()
                    {
                        "23%",
                        "0%",
                        "7%"
                    };
                }
                return m_StawkaList;
            }
            set
            {
                value.Where(f => StawkaList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                m_StawkaList = value;
            }
        }

        public List<WorkClass> Works
        {
            get
            {
                if (m_Works == null)
                {
                    m_Works = new List<WorkClass>();
                }
                return m_Works;
            }

            set => m_Works = value;
        }

        public static Property Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Property));
                reader = new StreamReader(xmlPath);
                return (Property)serializer.Deserialize(reader);
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
                XmlSerializer xsSubmit = new XmlSerializer(typeof(Property));
                writer = new StreamWriter(xmlPath, false);
                xsSubmit.Serialize(writer, Instance);
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
