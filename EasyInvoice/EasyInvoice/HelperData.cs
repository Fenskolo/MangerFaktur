using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    class HelperData
    {
        private static string lSprzed = "Sprzedawca";
        private static string lNabyw = "Nabywca i płatnik";
        private static string SprzeNaz = "Tomasz Zysk";
        private static string NabyNaz = "Xyz";
        private static string SprzedUl = "Reja 5";
        private static string NabywcaUl = "ZZZ 50B";
        private static string SprzedawcaKod = "18-200 Wysokie Mazowieckie";
        private static string NabywcaKod = "02-672 Warszawa";
        private static string SprzedawcaNip = "Nip 9999990050";
        private static string NabywcaNop = "NIP 1111322380";
        private static string SprzedawcaMail = "13@gmail.com";
        private static string NabywcaMail = "";

        public List<DaneTabela> GetSprzeNaby()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= lSprzed, Prawa= lNabyw },
                new DaneTabela{ Lewa= SprzeNaz, Prawa= NabyNaz },
                new DaneTabela{ Lewa= SprzedUl, Prawa= NabywcaUl },
                new DaneTabela{ Lewa= SprzedawcaKod, Prawa= NabywcaKod },
                new DaneTabela{ Lewa= SprzedawcaNip, Prawa= NabywcaNop },
                new DaneTabela{ Lewa= SprzedawcaMail, Prawa= NabywcaMail },
            };

            return dt;
        }
    }

    public class DaneNaglowek
    {
        private readonly string lMiejsce = "MIEJSCE WYSTAWIENIA:";
        private readonly string lDataWys = "DATA WYSTAWIENIA";
        private readonly string lDataSprz = "DATA SPRZEDAŻY";
        private readonly string lTerminZap = "TERMIN ZAPŁATY:";
        private readonly string lFormaPlatn = "FORMA PŁATNOŚĆ:";
        private string miejsce = "WYSOKIE MAZOWIECKIE";
        private string dataWyst = "30-04-2018";
        private string dataSprzed = "30-04-2018";
        private string terminZap = "14-05-2018";
        private string formaPlat = "PRZELEW 14 DNI";

        public string Miejsce { set => miejsce = value; }
        public string DataWyst { set => dataWyst = value; }
        public string DataSprzed { set => dataSprzed = value; }
        public string TerminZap { set => terminZap = value; }
        public string FormaPlat { set => formaPlat = value; }

        public List<DaneTabela> GetNaglowekL()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = lMiejsce, Prawa = miejsce},
                new DaneTabela(){Lewa = lDataWys, Prawa = dataWyst},
                new DaneTabela(){Lewa = lDataSprz, Prawa = dataSprzed}
            };

            return dt;
        }

        public List<DaneTabela> GetNaglowekR()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = lTerminZap, Prawa = terminZap},
                new DaneTabela(){Lewa = lFormaPlatn, Prawa = formaPlat}
            };

            return dt;
        }

    }

    [DataContract]
    public class DaneUsluga
    {
        [DataMember]
        public int LpTabela { get; set; }
        [DataMember]
        public string OpisTabela { get; set; }
        [DataMember]
        public string Rodzajilosc { get; set; }
        [DataMember]
        public int Ilosc { get; set; }
        [DataMember]
        public double CenaN { get; set; }
        [DataMember]
        public double WartoscN { get; set; }
        [DataMember]
        public string StawkaV { get; set; }
        [DataMember]
        public double KwotaV { get; set; }
        [DataMember]
        public double WartoscB { get; set; }
    }

    [DataContract]
    public class DaneTabela
    {
        [DataMember]
        public string Lewa { get; set; }
        [DataMember]
        public string Prawa { get; set; }
    }
}
