using System;
using System.ComponentModel;
using System.Globalization;

namespace ManagerFaktur
{
    public class PasswordConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            return destinationType == typeof(string)
                ? "********"
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}