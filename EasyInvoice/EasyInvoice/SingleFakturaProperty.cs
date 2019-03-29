using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable()]
    public class SingleFakturaProperty
    {
        private static SingleFakturaProperty m_Singleton;
        public SingleFakturaProperty() { }
        private WorkClass m_Work;
      

        [XmlIgnore]
        public SingleFakturaProperty MySingleton
        {
            get => m_Singleton;
            set
            {
                m_Singleton = value;
            }
        }


        public static SingleFakturaProperty Singleton
        {
            get
            {
                if (m_Singleton == null)
                {
                    m_Singleton = new SingleFakturaProperty();
                }
                return m_Singleton;
            }


            set => m_Singleton = value;
        }

        public WorkClass Work
        {
            get
            {
                if(m_Work ==null)
                {
                    m_Work = new WorkClass();
                }

                return m_Work;
            }
            set => m_Work = value;
        }

        public IEnumerable<DaneUsluga> GetListDt()
        {
            int i = 1;
            foreach (DataRow dr in Work.Dt.Rows)
            {
                var dat = new DaneUsluga
                {
                    LpTabela = i,
                    OpisTabela = dr[DictionaryMain.kolumnaTowar].ToString(),
                    Rodzajilosc = dr[DictionaryMain.kolumnaJM].ToString(),
                    Ilosc = Convert.ToInt32(dr[DictionaryMain.kolumnaIlosc]),
                    CenaNetto = decimal.Round((decimal)dr[DictionaryMain.kolumnaCenaNetto],2),
                    WartoscNetto = decimal.Round((decimal)dr[DictionaryMain.kolumnaWartoscNetto],2),
                    StawkaVat = dr[DictionaryMain.kolumnaStawkaVat].ToString(),
                    KwotaVat = decimal.Round((decimal)dr[DictionaryMain.kolumnaKwotaVat],2),
                    WartoscBrutto = decimal.Round((decimal)dr[DictionaryMain.kolumnaWartoscBrutto],2)
                };
                yield return dat;
                i++;
            }
        }


        public IEnumerable<DaneUsluga> GetSum()
        {
            var x = GetListDt().Select(z => z.StawkaVat).Distinct().ToList();

            var d1 = new DaneUsluga();
            foreach (var item in GetListDt())
            {
                d1.WartoscNetto += item.WartoscNetto;
                d1.KwotaVat += item.KwotaVat;
                d1.WartoscBrutto += item.WartoscBrutto;
                d1.StawkaVat = "-";
            }
            yield return d1;

            foreach (var stri in x)
            {
                var d2 = new DaneUsluga();
                foreach (var item in GetListDt())
                {
                    if (stri == item.StawkaVat)
                    {
                        d2.WartoscNetto += item.WartoscNetto;
                        d2.KwotaVat += item.KwotaVat;
                        d2.WartoscBrutto += item.WartoscBrutto;
                        d2.StawkaVat = stri;
                    }
                }
                yield return d2;
            }
        }
    }
}

