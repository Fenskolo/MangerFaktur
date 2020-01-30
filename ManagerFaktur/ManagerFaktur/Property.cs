using System;
using System.Windows.Forms;

namespace ManagerFaktur
{
    public partial class Property : Form
    {
        public Property()
        {
            InitializeComponent();
            propGrid.SelectedObject = Settings.Instance.MyInstance;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Settings.Instance.MyInstance = (Settings) propGrid.SelectedObject;
        }
    }
}