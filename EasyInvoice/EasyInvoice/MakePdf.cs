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
            
            SingleFakturaProperty.Singleton.Work.MyDtString = SingleFakturaProperty.Singleton.Work.Dt.Serialize();

            int myID = 1;
            bool que = true;
            while (que)
            {                
                myID++;
                que = !Property.Instance.Works.All(f => f.Naglowek.Id != myID);
            }
            SingleFakturaProperty.Singleton.Work.Naglowek.Id = myID;
            var x = (WorkClass)SingleFakturaProperty.Singleton.Work.Clone();
                x.Naglowek = (Naglowek)SingleFakturaProperty.Singleton.Work.Naglowek.Clone();
                x.Nabywca = (FirmaData)SingleFakturaProperty.Singleton.Work.Nabywca.Clone();
                x.Sprzedawca = (FirmaData)SingleFakturaProperty.Singleton.Work.Sprzedawca.Clone();
            Property.Instance.Works.Add(x);
            Property.SerializeXml(); 

         //   HelperXML.SerializeXml();
         //  SingleFakturaProperty.Singleton.Work.Dt.WriteXml("dt.xml");
        }
    }
}
