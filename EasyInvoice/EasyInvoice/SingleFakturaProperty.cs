using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable()]
    public class SingleFakturaProperty
    {
        private static SingleFakturaProperty _singleton;
        public SingleFakturaProperty() { }
        private WorkClass _work;
      

        [XmlIgnore]
        public SingleFakturaProperty MySingleton
        {
            get => _singleton;
            set
            {
                _singleton = value;
            }
        }


        public static SingleFakturaProperty Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new SingleFakturaProperty();
                }
                return _singleton;
            }


            set => _singleton = value;
        }

        public WorkClass Work
        {
            get
            {
                if(_work ==null)
                {
                    _work = new WorkClass();
                }

                return _work;
            }
            set => _work = value;
        }

        //   [XmlIgnore]
        public List<DaneUsluga> GetListDt()
        {
            List<DaneUsluga> list = new List<DaneUsluga>();

            int i = 1;
            foreach (DataRow dr in Work.Dt.Rows)
            {
                DaneUsluga dat = new DaneUsluga
                {
                    LpTabela = i,
                    OpisTabela = dr[DictionaryMain.kolumnaTowar].ToString(),
                    Rodzajilosc = dr[DictionaryMain.kolumnaJM].ToString(),
                    Ilosc = Convert.ToInt32(dr[DictionaryMain.kolumnaIlosc]),
                    CenaNetto = Decimal.Round((decimal)dr[DictionaryMain.kolumnaCenaNetto],2),
                    WartoscNetto = Decimal.Round((decimal)dr[DictionaryMain.kolumnaWartoscNetto],2),
                    StawkaVat = dr[DictionaryMain.kolumnaStawkaVat].ToString(),
                    KwotaVat = Decimal.Round((decimal)dr[DictionaryMain.kolumnaKwotaVat],2),
                    WartoscBrutto = Decimal.Round((decimal)dr[DictionaryMain.kolumnaWartoscBrutto],2)
                };
                list.Add(dat);
                i++;

            }
            return list;
        }         
    }
}

