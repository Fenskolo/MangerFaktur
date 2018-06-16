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
        public List<DaneTabela> GetSprzeNaby()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelHeaderSprzedawca, Prawa= DictionaryMain.labelHeaderNabywca },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Work.Sprzedawca.NazwaFirmy, Prawa= SingleFakturaProperty.Singleton.Work.Nabywca.NazwaFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Work.Sprzedawca.UlicaFirmy, Prawa= SingleFakturaProperty.Singleton.Work.Nabywca.UlicaFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Work.Sprzedawca.MiastoFirmy, Prawa= SingleFakturaProperty.Singleton.Work.Nabywca.MiastoFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Work.Sprzedawca.NipFirmy, Prawa= SingleFakturaProperty.Singleton.Work.Nabywca.NipFirmy },
                new DaneTabela{ Lewa= SingleFakturaProperty.Singleton.Work.Sprzedawca.InneFirmy, Prawa= SingleFakturaProperty.Singleton.Work.Nabywca.InneFirmy },
            };

            return dt;
        }

        public List<DaneTabela> NrBankowy()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa=DictionaryMain.labelNumerRachunku, Prawa= SingleFakturaProperty.Singleton.Work.NumerRachunku}
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelZaplacone, Prawa= "0,00 PLN" },
                new DaneTabela{ Lewa= DictionaryMain.labelDoZaplaty, Prawa= GetSum()[0].WartoscBrutto.ToString() + " PLN"}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelRazem, Prawa= GetSum()[0].WartoscBrutto.ToString() + " PLN" }
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

            string Slowo = LiczbyNaSlowaNET.NumberToText.Convert(Convert.ToDecimal(GetSum()[0].WartoscBrutto), nTTO);
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelSlownie, Prawa= Slowo }
            };

            return dt;
        }

        public List<DaneUsluga> GetSum()
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
    }

    public class DaneNaglowek
    {
        public List<DaneTabela> GetNaglowekL()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = DictionaryMain.labelMiejsceWystawienia, Prawa = SingleFakturaProperty.Singleton.Work.Naglowek.MiejsceWystawienia},
                new DaneTabela(){Lewa = DictionaryMain.labelDataWystawienia, Prawa = SingleFakturaProperty.Singleton.Work.Naglowek.DataWystawienia.ToShortDateString()},
                new DaneTabela(){Lewa = DictionaryMain.labelDataSprzedazy, Prawa = SingleFakturaProperty.Singleton.Work.Naglowek.DataSprzedazy.ToShortDateString()}
            };

            return dt;
        }

        public List<DaneTabela> GetNaglowekR()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela(){Lewa = DictionaryMain.labelTerminZaplaty, Prawa = SingleFakturaProperty.Singleton.Work.Naglowek.TerminZaplaty.ToShortDateString()},
                new DaneTabela(){Lewa = DictionaryMain.labelFormaPlatnosci, Prawa = SingleFakturaProperty.Singleton.Work.Naglowek.FormaPlatnosci}
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
