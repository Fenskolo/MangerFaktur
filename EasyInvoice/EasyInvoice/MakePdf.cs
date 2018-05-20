using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyInvoice
{
    public class MakePdf
    {
        public MakePdf()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "plik" + ".pdf";
            GenerujPolaWDokumencie p = new GenerujPolaWDokumencie(fileName);

            System.Diagnostics.Process.Start(fileName);          
        }
    }
}
