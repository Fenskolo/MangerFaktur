using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace ManagerFaktur
{
    [Serializable]
    public class Settings
    {
        private static Settings instance;
        private static readonly string xmlFile = Properties.Settings.Default.XmlConfig;
        private SymbolCollection ep;
        private List<string> m_ListExtenstion;
        private List<string> m_ListMail;
        private List<string> m_ListSDelOneMonth;

        private Settings()
        {
        }

        public static Settings Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                if (File.Exists(xmlFile))
                {
                    instance = Deserialize();
                }
                else
                {
                    instance = new Settings();
                    SerializeXml();
                }

                return instance;
            }
        }


        [XmlIgnore]
        [Browsable(false)]
        public Settings MyInstance
        {
            get => instance;
            set
            {
                instance = value;
                SerializeXml();
            }
        }

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
                "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        public List<string> ListMail
        {
            get { return m_ListMail ?? (m_ListMail = new List<string>()); }
            set => m_ListMail = value;
        }

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
                "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        public List<string> ListExtenstion
        {
            get
            {
                if (m_ListExtenstion == null)
                {
                    m_ListExtenstion = new List<string>();
                }

                return m_ListExtenstion;
            }
            set => m_ListExtenstion = value;
        }

        [Editor(typeof(SymbolCollectionEditor), typeof(UITypeEditor))]
        public SymbolCollection SymboleOkres
        {
            get
            {
                if (ep == null)
                {
                    ep = new SymbolCollection();
                }

                return ep;
            }
            set => ep = value;
        }

        [Category("SendMail")]
        [Description("login do poczty")]
        public string Login { get; set; } = string.Empty;

        [Category("SendMail")]
        [Description("hasło do poczty")]
        [TypeConverter(typeof(PasswordConverter))]
        [Editor(typeof(PasswordEditor), typeof(UITypeEditor))]
        public string Password { get; set; } = string.Empty;

        [Category("SendMail")]
        [Description("Mail od")]
        public string From { get; set; } = string.Empty;

        [Category("SendMail")]
        [Description("Mail do")]
        public string To { get; set; } = string.Empty;

        public string LogPath { get; set; } = Properties.Settings.Default.Log;

        [Editor(
            @"System.Windows.Forms.Design.StringCollectionEditor," +
            "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        public List<string> ListSDelOneMonth
        {
            get
            {
                if (m_ListSDelOneMonth == null)
                {
                    m_ListSDelOneMonth = new List<string>();
                }

                return m_ListSDelOneMonth;
            }
            set => m_ListSDelOneMonth = value;
        }

        public SearchOption SearchOptions { get; set; } = 0;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("WorkPath")]
        public string WorkPath { get; set; } = string.Empty;

        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("DestPath")]
        public string DestPath { get; set; } = string.Empty;

        [XmlElement("NazwaPoczątkuPliku")] public string FileNameDest { get; set; } = string.Empty;

        public static Settings Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Settings));
                reader = new StreamReader(xmlFile);
                return (Settings) serializer.Deserialize(reader);
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
                var xsSubmit = new XmlSerializer(typeof(Settings));
                writer = new StreamWriter(xmlFile, false);
                xsSubmit.Serialize(writer, instance);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public void Serialze()
        {
            SerializeXml();
        }
    }
}