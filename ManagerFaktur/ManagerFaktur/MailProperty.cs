using System;
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
            MailSettings.Ins.MyInstance = (MailSettings) propMail.SelectedObject;
        }
    }
}