using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
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
        private string _defWorkPath = string.Empty;
        private string _defDestPath = string.Empty;
        private string _fileNameStart = string.Empty;
        private SearchOption searchO = 0;
        private List<string> _listExtenstion;
        private SymbolCollection ep;

        /// <summary>
        /// Mail
        /// </summary>
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _from = string.Empty;
        private string _to = string.Empty;



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

        public SearchOption SearchO
        {
            get
            {
                return searchO;
            }

            set
            {
                searchO = value;
            }
        }

        public void Serialze()
        {
            SerializeXml();
        }

        [Editor(typeof(SymbolCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public SymbolCollection Symbole
        {
            get {
                if(ep==null)
                {
                    ep = new SymbolCollection();
                }
                return ep;
                    }
            set { ep = value; }
        }

        [Category("SendMail"), Description("login do poczty")]
        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
            }
        }

        [Category("SendMail"), Description("hasło do poczty")]
        [TypeConverter(typeof(PasswordConverter))]
        [Editor(typeof(PasswordEditor), typeof(UITypeEditor))]
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        [Category("SendMail"), Description("Mail od")]
        public string From
        {
            get
            {
                return _from;
            }

            set
            {
                _from = value;
            }
        }

        [Category("SendMail"),Description("Mail do")]
        public string To
        {
            get
            {
                return _to;
            }

            set
            {
                _to = value;
            }
        }
    }

    public class SymbolCollection : CollectionBase
    {
        public Symbol this[int index]
        {
            get { return (Symbol)List[index]; }
        }

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
        private string firstString;
        private string lastString;
        
        [Category("Symbol")]
        [DisplayName("Start")]
        public string FirstString
        {
            get { return @firstString; }
            set { firstString = @value; }
        }

        [Category("Symbol")]
        [DisplayName("End")]
        public string LastString
        {
            get { return @lastString; }
            set { lastString = @value; }
        }

        //[Category("Employee")]
        //[DisplayName("Date of Hire")]
        //[Description("The hire date of the employee.")]
        //public DateTime DateOfHire
        //{
        //    get { return dateOfHire; }
        //    set { dateOfHire = value; }
        //}
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

    class PasswordConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return destinationType == typeof(string) ? "********" :
                base.ConvertTo(context, culture, value, destinationType);


        }
    }

    class PasswordEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)
                provider.GetService(typeof(IWindowsFormsEditorService));
            if (svc != null)
            {
                TextBox tb;
                Button btn;
                Form frm = new Form
                {
                    Controls = {
                 (tb = new TextBox { PasswordChar = '*', Dock = DockStyle.Top,
                     Text = (string)value}),
                 (btn = new Button { Text = "OK", Dock = DockStyle.Bottom, DialogResult = DialogResult.OK})
            },
                    AcceptButton = btn
                };

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    value = tb.Text;
                }
            }
            return value;
        }
    }
}
