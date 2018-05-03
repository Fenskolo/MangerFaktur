﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ManagerFaktur
{
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
                        (tb = new TextBox { PasswordChar = '*', Dock = DockStyle.Top, Text = (string)value}),
                        (btn = new Button { Text = "OK", Dock = DockStyle.Bottom, DialogResult = DialogResult.OK})
                    }, AcceptButton = btn
                };

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    value = tb.Text;
                }
            }
            return value;
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
}
