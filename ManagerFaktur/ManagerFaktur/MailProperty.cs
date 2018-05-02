using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerFaktur
{
    public partial class MailProperty : Form
    {
        public MailProperty()
        {
            InitializeComponent();
            propMail.SelectedObject = MailSettings.Ins.MyInstance;
        }

        private void btnUstaw_Click(object sender, EventArgs e)
        {
            MailSettings.Ins.MyInstance = (MailSettings)propMail.SelectedObject;
        }
    }
}
