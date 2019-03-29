using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyInvoice
{
    public interface IHelperData
    {
        List<DaneTabela> GetSprzeNaby();
        List<DaneTabela> GetZapDoZap();
        List<DaneTabela> NrBankowy();
        List<DaneTabela> Razem();
        DaneTabela RazemSlownie();
    }

    public class HelperData : IHelperData
    {
        private readonly WorkClass m_WorkClass;
        private readonly IEnumerable<DaneUsluga> m_ListDt;

        public HelperData(SingleFakturaProperty singleFakturaProperty)
        {
            m_WorkClass = singleFakturaProperty.Work;
            m_ListDt = singleFakturaProperty.GetSum();

        }
        public List<DaneTabela> GetSprzeNaby()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelHeaderSprzedawca, Prawa= DictionaryMain.labelHeaderNabywca },
                new DaneTabela{ Lewa= m_WorkClass.Sprzedawca.NazwaFirmy, Prawa= m_WorkClass.Nabywca.NazwaFirmy },
                new DaneTabela{ Lewa= m_WorkClass.Sprzedawca.UlicaFirmy, Prawa= m_WorkClass.Nabywca.UlicaFirmy },
                new DaneTabela{ Lewa= m_WorkClass.Sprzedawca.MiastoFirmy, Prawa=m_WorkClass.Nabywca.MiastoFirmy },
                new DaneTabela{ Lewa= m_WorkClass.Sprzedawca.NipFirmy, Prawa= m_WorkClass.Nabywca.NipFirmy },
                new DaneTabela{ Lewa= m_WorkClass.Sprzedawca.InneFirmy, Prawa= m_WorkClass.Nabywca.InneFirmy },
            };

            return dt;
        }

        public List<DaneTabela> NrBankowy()
        {
            List<DaneTabela> dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa=DictionaryMain.labelNumerRachunku, Prawa= m_WorkClass.NumerRachunku}
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            var sum = m_ListDt.First();
            var dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelZaplacone, Prawa= "0,00 PLN" },
                new DaneTabela{ Lewa= DictionaryMain.labelDoZaplaty, Prawa= sum.WartoscBrutto.ToCurrency(true)}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            var sum = m_ListDt.First();
            var dt = new List<DaneTabela>()
            {
                new DaneTabela{ Lewa= DictionaryMain.labelRazem, Prawa= sum.WartoscBrutto.ToCurrency(true) }
            };

            return dt;
        }

        public DaneTabela RazemSlownie()
        {
            var sum = m_ListDt.First();
            var nTTO = new LiczbyNaSlowaNET.NumberToTextOptions()
            {
                Currency = LiczbyNaSlowaNET.Currency.PLN,
                Stems = true
            };

            string Slowo = LiczbyNaSlowaNET.NumberToText.Convert(Convert.ToDecimal(sum.WartoscBrutto), nTTO);
            return new DaneTabela { Lewa = DictionaryMain.labelSlownie, Prawa = Slowo };
        }
    }

    public static class Converter
    {
        public static string ToCurrency(this decimal ob, bool pln =false)
        {
            return ob.ToString("C").Replace(" zł", pln? " PLN": string.Empty);
        }
    }
}
