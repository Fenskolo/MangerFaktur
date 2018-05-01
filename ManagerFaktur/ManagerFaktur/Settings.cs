using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ManagerFaktur
{
    [Serializable()]
    public class Settings
    {
        private static Settings instance;
        private Settings() { }
        private static string xmlFile = "settings.xml";
        private string _defWorkPath = string.Empty;
        private string _defDestPath = string.Empty;

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    if (File.Exists(xmlFile))
                    {
                        instance = Deserialize();
                    }
                    else
                    {
                        instance = new Settings();
                        SerializeXml();
                    }                   
                }

                return instance;
            }
        }

        public static Settings Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Settings));
                reader = new StreamReader(xmlFile);
                return (Settings)serializer.Deserialize(reader);
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
                XmlSerializer xsSubmit = new XmlSerializer(typeof(Settings));
                writer = new StreamWriter(xmlFile, false);
                xsSubmit.Serialize(writer, instance);
            }
            catch( Exception ex)
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

        [System.Xml.Serialization.XmlElement("WorkPath")]
        public string DefWorkPath
        {
            get
            {
                return _defWorkPath;
            }

            set
            {
                _defWorkPath = value;
            }
        }

        [System.Xml.Serialization.XmlElement("DestPath")]
        public string DefDestPath
        {
            get
            {
                return _defDestPath;
            }

            set
            {
                _defDestPath = value;
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public Settings s
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                SerializeXml();
            }
        }
    }
}
