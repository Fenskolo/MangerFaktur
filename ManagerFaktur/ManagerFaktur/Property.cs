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
    public partial class Property : Form
    {
        public Property()
        {
            InitializeComponent();
            propGrid.SelectedObject = Settings.Instance.s;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Settings.Instance.s = (Settings)propGrid.SelectedObject;
        }
    }
}
