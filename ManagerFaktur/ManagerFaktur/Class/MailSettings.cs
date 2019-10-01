using Infragistics.Win.Design;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace ManagerFaktur
{
    public class MailSettings
    {
        private static MailSettings ins;
        private MailSettings() { }

        private List<string> _listMail;
        private List<string> _listAtach;

        public static MailSettings Ins
        {
            get
            {
                if (ins == null)
                {
                    ins = new MailSettings();
                }
                return ins;
            }
            set => ins = value;
        }


        public string Subject { get; set; } = string.Empty;

        [XmlIgnore]
        [Browsable(false)]
        public MailSettings MyInstance { get => ins; set => ins = value; }

        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]
        public string Message { get; set; } = string.Empty;

        [XmlIgnore]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," + "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
         typeof(UITypeEditor))]
        public List<string> ListMail
        {
            get
            {
                if (_listMail == null)
                {
                    _listMail = Settings.Instance.ListMail;
                }
                return _listMail;
            }
            set
            {
                _listMail = value;
                Settings.Instance.ListMail = value;
            }
        }

        [DisplayName("Mail Do")]
        [DefaultValue("")]
        [TypeConverter(typeof(FormatStringConverter))]
        public string MailTo { get; set; } = string.Empty;

        [Browsable(false)]
        public List<string> ListAtach
        {
            get
            {
                if (_listAtach == null)
                {
                    _listAtach = new List<string>();
                }

                return _listAtach;
            }
            set => _listAtach = value;
        }
    }
}
