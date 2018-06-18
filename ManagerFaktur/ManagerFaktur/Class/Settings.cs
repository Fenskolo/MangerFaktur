using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;

namespace ManagerFaktur
{
    [Serializable()]
    public class Settings
    {
        private static Settings instance;
        private Settings() { }
        private static string xmlFile = Properties.Settings.Default.XmlConfig;
        private List<string> _listExtenstion;
        private SymbolCollection ep;
        private List<string> _listSDelOneMonth;
        private List<string> _listMail;

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
            get
            {
                if (_listMail == null)
                {
                    _listMail = new List<string>();
                }

                return _listMail;
            }
            set => _listMail = value;
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
            set => _listExtenstion = value;
        }          

        public void Serialze()
        {
            SerializeXml();
        }

        [Editor(typeof(SymbolCollectionEditor), typeof(UITypeEditor))]
        public SymbolCollection SymboleOkres
        {
            get
            {
                if(ep==null)
                {
                    ep = new SymbolCollection();
                }
                return ep;
            }
            set => ep = value; 
        }

        [Category("SendMail"), Description("login do poczty")]
        public string Login { get; set; } = string.Empty;
        [Category("SendMail"), Description("hasło do poczty")]
        [TypeConverter(typeof(PasswordConverter))]
        [Editor(typeof(PasswordEditor), typeof(UITypeEditor))]
        public string Password { get; set; } = string.Empty;
        [Category("SendMail"), Description("Mail od")]
        public string From { get; set; } = string.Empty;
        [Category("SendMail"), Description("Mail do")]
        public string To { get; set; } = string.Empty;
        public string LogPath { get; set; } = Properties.Settings.Default.Log;

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor,"+"System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
       typeof(UITypeEditor))]
        public List<string> ListSDelOneMonth
        {
            get
            {
                if (_listSDelOneMonth == null)
                {
                    _listSDelOneMonth = new List<string>();
                }

                return _listSDelOneMonth;
            }
            set => _listSDelOneMonth = value;
        }

        public SearchOption SearchOptions { get; set; } = 0;
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("WorkPath")]
        public string WorkPath { get; set; } = string.Empty;
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [XmlElement("DestPath")]
        public string DestPath { get; set; } = string.Empty;
        [XmlElement("NazwaPoczątkuPliku")]
        public string FileNameDest { get; set; } = string.Empty;
    }

    public class SymbolCollection : CollectionBase
    {
        public Symbol this[int index] {get => (Symbol)List[index]; }

        public void Add(Symbol emp)
        {
            List.Add(emp);
        }

        public void Remove(Symbol emp)
        {
            List.Remove(emp);
        }
    }

    public class Symbol
    {
        [Category("Symbol")]
        [DisplayName("Start")]
        public string FirstString { get; set; }

        [Category("Symbol")]
        [DisplayName("End")]
        public string LastString { get; set; }

        [Category("Symbol")]
        [DisplayName("Typ")]
        public TypDanych Td { get; set; }
    }

    public enum TypDanych
    {
        containsOkres = 0,
        okresOdDo =1,
        containsSymbol =2,
        symbolOdDo=3
    }

    public class SymbolCollectionEditor : CollectionEditor
    {
        public SymbolCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            Symbol item = new Symbol();
            item = (Symbol)value;

            return base.GetDisplayText(string.Format("{0}, {1}", item.FirstString, item.LastString));
        }
    }

    
}
