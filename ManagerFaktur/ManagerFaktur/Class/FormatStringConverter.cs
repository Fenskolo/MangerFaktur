using System;
using System.ComponentModel;

namespace ManagerFaktur
{
    public class FormatStringConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {           
            return new StandardValuesCollection(MailSettings.Ins.ListMail);
        }
    }
}
