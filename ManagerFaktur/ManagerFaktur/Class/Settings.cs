using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
        private string _fileNameStart = string.Empty;
        private List<string> _listExtenstion;

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
            catch(Exception)
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

        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("WorkPath")]
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

        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("DestPath")]
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
        public Settings MyInstance
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

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
        "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(UITypeEditor))]
        
        public List<string> ListExtenstion
        {
            get
            {
                if(_listExtenstion == null)
                {
                    _listExtenstion = new List<string>();
                }

                return _listExtenstion;
            }

            set
            {
                _listExtenstion = value;
            }
        }

        [XmlElement("NazwaPoczątkuPliku")]
        public string FileNameStart
        {
            get
            {
                return _fileNameStart;
            }

            set
            {
                _fileNameStart = value;
            }
        }

        public void Serialze()
        {
            SerializeXml();
        }
    }
}
