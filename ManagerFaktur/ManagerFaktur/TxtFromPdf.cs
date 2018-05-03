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
    public partial class TxtFromPdf : Form
    {
        public TxtFromPdf(string tekst)
        {
            InitializeComponent();
            rTxt.Text = tekst;
        }        
    }
}
