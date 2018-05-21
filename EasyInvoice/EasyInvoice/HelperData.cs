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

        public List<DaneTabela> NrBankowy()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= "Numer rachunku bankowego: ", Prawa= "12 4444 3333 5555 3333 7777 9999 mBank" }
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= "Zapłacono:", Prawa= "0,00 PLN" },
                new DaneTabela{ Lewa= "Do zapłaty", Prawa= getSum()[0].WartoscB.ToString() + " PLN"}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= "Razem:", Prawa= getSum()[0].WartoscB.ToString() + " PLN" }
            };

            return dt;
        }

        public List<DaneTabela> RazemSlownie()
        {
            LiczbyNaSlowaNET.NumberToTextOptions nTTO = new LiczbyNaSlowaNET.NumberToTextOptions()
            {
                Currency = LiczbyNaSlowaNET.Currency.PLN,
                Stems = true
            };

            string Slowo = LiczbyNaSlowaNET.NumberToText.Convert(Convert.ToDecimal(getSum()[0].WartoscB), nTTO);
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= "Słownie:", Prawa= Slowo }
            };

            return dt;
        }

        public List<DaneUsluga> getSum()
        {
            List<DaneUsluga> du = new List<DaneUsluga>();
            var x = GetListTable().Select(z => z.StawkaV).Distinct().ToList();

            DaneUsluga d1 = new DaneUsluga();
            foreach(var item in GetListTable())
            {
                d1.WartoscN += item.WartoscN;
                d1.KwotaV += item.KwotaV;
                d1.WartoscB += item.WartoscB;
                d1.StawkaV = "-";
            }
            du.Add(d1);

            foreach(var stri in x)
            {
                DaneUsluga d2 = new DaneUsluga();
                foreach (var item in GetListTable())
                {
                    if(stri==item.StawkaV)
                    {
                        d2.WartoscN += item.WartoscN;
                        d2.KwotaV += item.KwotaV;
                        d2.WartoscB += item.WartoscB;
                        d2.StawkaV = stri;
                    }
                }
                du.Add(d2);
            }
            
            return du;
        }
        

        public List<DaneUsluga> GetListTable()
        {
            List<DaneUsluga> list = new List<DaneUsluga>();
            DaneUsluga d = new DaneUsluga()
            {
                LpTabela = 1,
                OpisTabela = "Usługi Programistyczne",
                Rodzajilosc = "szt.",
                Ilosc = 160,
                CenaN = 10
            };
            d.WartoscN = d.Ilosc * d.CenaN;
            d.StawkaV = "23%";
            d.KwotaV = d.WartoscN * 0.23;
            d.WartoscB = d.KwotaV + d.WartoscN;
            list.Add(d);
            DaneUsluga d1 = new DaneUsluga()
            {
                LpTabela = 2,
                OpisTabela = "konsultacje",
                Rodzajilosc = "szt.",
                Ilosc = 10,
                CenaN = 10
            };
            d1.WartoscN = d1.Ilosc * d1.CenaN;
            d1.StawkaV = "7%";
            d1.KwotaV = d1.WartoscN * 0.7;
            d1.WartoscB = d1.KwotaV + d1.WartoscN;

            list.Add(d1);

            return list;
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
