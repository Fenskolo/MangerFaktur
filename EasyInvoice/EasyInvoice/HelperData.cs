using System;
using System.Collections.Generic;
using System.Linq;
using LiczbyNaSlowaNET;

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
        private readonly IEnumerable<DaneUsluga> s_MListDt;
        private readonly WorkClass s_MWorkClass;

        public HelperData(SingleFakturaProperty singleFakturaProperty)
        {
            s_MWorkClass = singleFakturaProperty.Work;
            s_MListDt = singleFakturaProperty.GetSum();
        }

        public List<DaneTabela> GetSprzeNaby()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelHeaderSprzedawca, Prawa = DictionaryMain.LabelHeaderNabywca},
                new DaneTabela {Lewa = s_MWorkClass.Sprzedawca.NazwaFirmy, Prawa = s_MWorkClass.Nabywca.NazwaFirmy},
                new DaneTabela {Lewa = s_MWorkClass.Sprzedawca.UlicaFirmy, Prawa = s_MWorkClass.Nabywca.UlicaFirmy},
                new DaneTabela {Lewa = s_MWorkClass.Sprzedawca.MiastoFirmy, Prawa = s_MWorkClass.Nabywca.MiastoFirmy},
                new DaneTabela {Lewa = s_MWorkClass.Sprzedawca.NipFirmy, Prawa = s_MWorkClass.Nabywca.NipFirmy},
                new DaneTabela {Lewa = s_MWorkClass.Sprzedawca.InneFirmy, Prawa = s_MWorkClass.Nabywca.InneFirmy}
            };

            return dt;
        }

        public List<DaneTabela> NrBankowy()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelNumerRachunku, Prawa = s_MWorkClass.NumerRachunku}
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            var sum = s_MListDt.First();
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelZaplacone, Prawa = "0,00 PLN"},
                new DaneTabela {Lewa = DictionaryMain.LabelDoZaplaty, Prawa = sum.WartoscBrutto.ToCurrency(true)}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            var sum = s_MListDt.First();
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelRazem, Prawa = sum.WartoscBrutto.ToCurrency(true)}
            };

            return dt;
        }

        public DaneTabela RazemSlownie()
        {
            var sum = s_MListDt.First();
            var nTto = new NumberToTextOptions
            {
                Currency = Currency.PLN,
                Stems = true
            };

            var slowo = NumberToText.Convert(Convert.ToDecimal(sum.WartoscBrutto), nTto);
            return new DaneTabela {Lewa = DictionaryMain.LabelSlownie, Prawa = slowo};
        }
    }

    public static class Converter
    {
        public static string ToCurrency(this decimal ob, bool pln = false)
        {
            return ob.ToString("C").Replace(" zł", pln ? " PLN" : string.Empty);
        }
    }
}