using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable]
    public class SingleFakturaProperty
    {
        private static SingleFakturaProperty _mSingleton;
        private WorkClass _mWork;


        [XmlIgnore]
        public SingleFakturaProperty MySingleton
        {
            get => _mSingleton;
            set => _mSingleton = value;
        }


        public static SingleFakturaProperty Singleton
        {
            get => _mSingleton ?? (_mSingleton = new SingleFakturaProperty());


            set => _mSingleton = value;
        }

        public WorkClass Work
        {
            get => _mWork ?? (_mWork = new WorkClass());
            set => _mWork = value;
        }

        public IEnumerable<DaneUsluga> GetListDt()
        {
            var i = 1;
            foreach (DataRow dr in Work.Dt.Rows)
            {
                var dat = new DaneUsluga
                {
                    LpTabela = i,
                    OpisTabela = dr[DictionaryMain.KolumnaTowar].ToString(),
                    Rodzajilosc = dr[DictionaryMain.KolumnaJm].ToString(),
                    Ilosc = Convert.ToInt32(dr[DictionaryMain.KolumnaIlosc]),
                    CenaNetto = decimal.Round((decimal) dr[DictionaryMain.KolumnaCenaNetto], 2),
                    WartoscNetto = decimal.Round((decimal) dr[DictionaryMain.KolumnaWartoscNetto], 2),
                    StawkaVat = dr[DictionaryMain.KolumnaStawkaVat].ToString(),
                    KwotaVat = decimal.Round((decimal) dr[DictionaryMain.KolumnaKwotaVat], 2),
                    WartoscBrutto = decimal.Round((decimal) dr[DictionaryMain.KolumnaWartoscBrutto], 2)
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
                    if (stri != item.StawkaVat)
                    {
                        continue;
                    }

                    d2.WartoscNetto += item.WartoscNetto;
                    d2.KwotaVat += item.KwotaVat;
                    d2.WartoscBrutto += item.WartoscBrutto;
                    d2.StawkaVat = stri;
                }

                yield return d2;
            }
        }
    }
}