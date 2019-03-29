using System.ComponentModel;

namespace ManagerFaktur
{
    public class PasswordConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            return destinationType == typeof(string) ? "********" : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
