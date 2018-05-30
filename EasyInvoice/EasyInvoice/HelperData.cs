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
        //private static string SprzeNaz = "Tomasz Zysk";
        //private static string NabyNaz = "Xyz";
        //private static string SprzedUl = "Reja 5";
        //private static string NabywcaUl = "ZZZ 50B";
        //private static string SprzedawcaKod = "18-200 Wysokie Mazowieckie";
        //private static string NabywcaKod = "02-672 Warszawa";
        //private static string SprzedawcaNip = "Nip 9999990050";
        //private static string NabywcaNop = "NIP 1111322380";
        //private static string SprzedawcaMail = "13@gmail.com";
        //private static string NabywcaMail = "";

        public List<DaneTabela> GetSprzeNaby()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelHeaderSprzedawca, Prawa= DictionaryMain.labelHeaderNabywca },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Sprzedawca.NazwaFirmy, Prawa= SingleFakturaProperty.Singleton.Nabywca.NazwaFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Sprzedawca.UlicaFirmy, Prawa= SingleFakturaProperty.Singleton.Nabywca.UlicaFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Sprzedawca.MiastoFirmy, Prawa= SingleFakturaProperty.Singleton.Nabywca.MiastoFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Sprzedawca.NipFirmy, Prawa= SingleFakturaProperty.Singleton.Nabywca.NipFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Sprzedawca.InneFirmy, Prawa= SingleFakturaProperty.Singleton.Nabywca.InneFirmy },
            };

            return dt;
        }

        public List<DaneTabela> NrBankowy()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa=DictionaryMain.labelNumerRachunku, Prawa= SingleFakturaProperty.Singleton.NumerRachunku}
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelZaplacone, Prawa= "0,00 PLN" },
                new DaneTabela{ Lewa= DictionaryMain.labelDoZaplaty, Prawa= getSum()[0].WartoscBrutto.ToString() + " PLN"}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelRazem, Prawa= getSum()[0].WartoscBrutto.ToString() + " PLN" }
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

            string Slowo = LiczbyNaSlowaNET.NumberToText.Convert(Convert.ToDecimal(getSum()[0].WartoscBrutto), nTTO);
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelSlownie, Prawa= Slowo }
            };

            return dt;
        }

        public List<DaneUsluga> getSum()
        {
            List<DaneUsluga> du = new List<DaneUsluga>();
            var x = SingleFakturaProperty.Singleton.GetListDt().Select(z => z.StawkaVat).Distinct().ToList();

            DaneUsluga d1 = new DaneUsluga();
            foreach(var item in SingleFakturaProperty.Singleton.GetListDt())
            {
                d1.WartoscNetto += item.WartoscNetto;
                d1.KwotaVat += item.KwotaVat;
                d1.WartoscBrutto += item.WartoscBrutto;
                d1.StawkaVat = "-";
            }
            du.Add(d1);

            foreach(var stri in x)
            {
                DaneUsluga d2 = new DaneUsluga();
                foreach (var item in SingleFakturaProperty.Singleton.GetListDt())
                {
                    if(stri==item.StawkaVat)
                    {
                        d2.WartoscNetto += item.WartoscNetto;
                        d2.KwotaVat += item.KwotaVat;
                        d2.WartoscBrutto += item.WartoscBrutto;
                        d2.StawkaVat = stri;
                    }
                }
                du.Add(d2);
            }
            
            return du;
        }
        

        //public List<DaneUsluga> GetListTable()
        //{
        //    List<DaneUsluga> list = new List<DaneUsluga>();
        //    DaneUsluga d = new DaneUsluga()
        //    {
        //        LpTabela = 1,
        //        OpisTabela = "Usługi Programistyczne",
        //        Rodzajilosc = "szt.",
        //        Ilosc = 160,
        //        CenaNetto = (decimal)10.5
        //    };
        //    d.WartoscNetto = d.Ilosc * d.CenaNetto;
        //    d.StawkaVat = "23%";
        //    d.KwotaVat = d.WartoscNetto * (decimal)0.23;
        //    d.WartoscBrutto = d.KwotaVat + d.WartoscNetto;
        //    list.Add(d);
        //    DaneUsluga d1 = new DaneUsluga()
        //    {
        //        LpTabela = 2,
        //        OpisTabela = "konsultacje",
        //        Rodzajilosc = "szt.",
        //        Ilosc = 10,
        //        CenaNetto = 10
        //    };
        //    d1.WartoscNetto = d1.Ilosc * d1.CenaNetto;
        //    d1.StawkaVat = "7%";
        //    d1.KwotaVat = d1.WartoscNetto * (decimal)0.7;
        //    d1.WartoscBrutto = d1.KwotaVat + d1.WartoscNetto;

        //    list.Add(d1);

        //    return list;
        //}
    }

    public class DaneNaglowek
    {
        //private string miejsce = "WYSOKIE MAZOWIECKIE";
        //private string dataWyst = "30-04-2018";
        //private string dataSprzed = "30-04-2018";
        //private string terminZap = "14-05-2018";
        //private string formaPlat = "PRZELEW 14 DNI";

        //public string Miejsce { set => miejsce = value; }
        //public string DataWyst { set => dataWyst = value; }
        //public string DataSprzed { set => dataSprzed = value; }
        //public string TerminZap { set => terminZap = value; }
        //public string FormaPlat { set => formaPlat = value; }

        public List<DaneTabela> GetNaglowekL()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = DictionaryMain.labelMiejsceWystawienia, Prawa = SingleFakturaProperty.Singleton.Naglowek.MiejsceWystawienia},
                new DaneTabela(){Lewa = DictionaryMain.labelDataWystawienia, Prawa = SingleFakturaProperty.Singleton.Naglowek.DataWystawienia.ToShortDateString()},
                new DaneTabela(){Lewa = DictionaryMain.labelDataSprzedazy, Prawa = SingleFakturaProperty.Singleton.Naglowek.DataSprzedazy.ToShortDateString()}
            };

            return dt;
        }

        public List<DaneTabela> GetNaglowekR()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = DictionaryMain.labelTerminZaplaty, Prawa = SingleFakturaProperty.Singleton.Naglowek.TerminZaplaty.ToShortDateString()},
                new DaneTabela(){Lewa = DictionaryMain.labelFormaPlatnosci, Prawa = SingleFakturaProperty.Singleton.Naglowek.FormaPlatnosci}
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
        public decimal CenaNetto { get; set; }
        [DataMember]
        public decimal WartoscNetto { get; set; }
        [DataMember]
        public string StawkaVat { get; set; }
        [DataMember]
        public decimal KwotaVat { get; set; }
        [DataMember]
        public decimal WartoscBrutto { get; set; }
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
