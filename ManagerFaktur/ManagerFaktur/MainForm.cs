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
    public partial class MF : Form
    {
        public MF()
        {
            InitializeComponent();
        }

        private void Ustawienia_Click(object sender, EventArgs e)
        {
            var pG = new Property();
            pG.ShowDialog();
        }

        private void MF_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Instance.Serialze();
        }
    }
}
