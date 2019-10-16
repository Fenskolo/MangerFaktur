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
        private readonly IEnumerable<DaneUsluga> _mListDt;
        private readonly WorkClass _mWorkClass;

        public HelperData(SingleFakturaProperty singleFakturaProperty)
        {
            _mWorkClass = singleFakturaProperty.Work;
            _mListDt = singleFakturaProperty.GetSum();
        }

        public List<DaneTabela> GetSprzeNaby()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelHeaderSprzedawca, Prawa = DictionaryMain.LabelHeaderNabywca},
                new DaneTabela {Lewa = _mWorkClass.Sprzedawca.NazwaFirmy, Prawa = _mWorkClass.Nabywca.NazwaFirmy},
                new DaneTabela {Lewa = _mWorkClass.Sprzedawca.UlicaFirmy, Prawa = _mWorkClass.Nabywca.UlicaFirmy},
                new DaneTabela {Lewa = _mWorkClass.Sprzedawca.MiastoFirmy, Prawa = _mWorkClass.Nabywca.MiastoFirmy},
                new DaneTabela {Lewa = _mWorkClass.Sprzedawca.NipFirmy, Prawa = _mWorkClass.Nabywca.NipFirmy},
                new DaneTabela {Lewa = _mWorkClass.Sprzedawca.InneFirmy, Prawa = _mWorkClass.Nabywca.InneFirmy}
            };

            return dt;
        }

        public List<DaneTabela> NrBankowy()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelNumerRachunku, Prawa = _mWorkClass.NumerRachunku}
            };

            return dt;
        }

        public List<DaneTabela> GetZapDoZap()
        {
            var sum = _mListDt.First();
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelZaplacone, Prawa = "0,00 PLN"},
                new DaneTabela {Lewa = DictionaryMain.LabelDoZaplaty, Prawa = sum.WartoscBrutto.ToCurrency(true)}
            };

            return dt;
        }

        public List<DaneTabela> Razem()
        {
            var sum = _mListDt.First();
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelRazem, Prawa = sum.WartoscBrutto.ToCurrency(true)}
            };

            return dt;
        }

        public DaneTabela RazemSlownie()
        {
            var sum = _mListDt.First();
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