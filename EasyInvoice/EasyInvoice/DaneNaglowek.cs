using System.Collections.Generic;

namespace EasyInvoice
{
    public interface IDaneNaglowek
    {
        List<DaneTabela> GetNaglowekL();
        List<DaneTabela> GetNaglowekR();
    }

    public class DaneNaglowek : IDaneNaglowek
    {
        private readonly Naglowek _mNaglowek;

        public DaneNaglowek(Naglowek naglowek)
        {
            _mNaglowek = naglowek;
        }

        public List<DaneTabela> GetNaglowekL()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.LabelMiejsceWystawienia, Prawa = _mNaglowek.MiejsceWystawienia},
                new DaneTabela
                {
                    Lewa = DictionaryMain.LabelDataWystawienia, Prawa = _mNaglowek.DataWystawienia.ToShortDateString()
                },
                new DaneTabela
                    {Lewa = DictionaryMain.LabelDataSprzedazy, Prawa = _mNaglowek.DataSprzedazy.ToShortDateString()}
            };

            return dt;
        }

        public List<DaneTabela> GetNaglowekR()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela
                    {Lewa = DictionaryMain.LabelTerminZaplaty, Prawa = _mNaglowek.TerminZaplaty.ToShortDateString()},
                new DaneTabela {Lewa = DictionaryMain.LabelFormaPlatnosci, Prawa = _mNaglowek.FormaPlatnosci}
            };

            return dt;
        }
    }
}