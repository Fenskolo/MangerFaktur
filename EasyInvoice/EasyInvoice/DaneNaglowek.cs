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
        private readonly Naglowek m_Naglowek;

        public DaneNaglowek(Naglowek naglowek)
        {
            m_Naglowek = naglowek;
        }

        public List<DaneTabela> GetNaglowekL()
        {
            List<DaneTabela> dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.labelMiejsceWystawienia, Prawa = m_Naglowek.MiejsceWystawienia},
                new DaneTabela {Lewa = DictionaryMain.labelDataWystawienia, Prawa = m_Naglowek.DataWystawienia.ToShortDateString()},
                new DaneTabela {Lewa = DictionaryMain.labelDataSprzedazy, Prawa = m_Naglowek.DataSprzedazy.ToShortDateString()}
            };

            return dt;
        }

        public List<DaneTabela> GetNaglowekR()
        {
            var dt = new List<DaneTabela>
            {
                new DaneTabela {Lewa = DictionaryMain.labelTerminZaplaty, Prawa = m_Naglowek.TerminZaplaty.ToShortDateString()},
                new DaneTabela {Lewa = DictionaryMain.labelFormaPlatnosci, Prawa = m_Naglowek.FormaPlatnosci}
            };

            return dt;
        }
    }
}
