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
