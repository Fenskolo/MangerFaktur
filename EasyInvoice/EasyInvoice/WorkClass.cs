using System;
using System.Data;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable()]
    public class WorkClass : ICloneable
    {
        private DataTable m_Dt;
        private FirmaData m_Sprzedawca;
        private FirmaData m_Nabywca;
        private Naglowek m_Naglowek;

        [XmlIgnore]
        public DataTable Dt
        {
            get
            {
                if (m_Dt == null)
                {
                    m_Dt = new DataTable
                    {
                        TableName = "TabelaFaktura"
                    };
                    m_Dt.Columns.Add(DictionaryMain.kolumnaTowar, typeof(string));
                    m_Dt.Columns.Add(DictionaryMain.kolumnaJM);
                    m_Dt.Columns.Add(DictionaryMain.kolumnaIlosc, typeof(int));
                    m_Dt.Columns.Add(DictionaryMain.kolumnaCenaNetto, typeof(decimal));
                    m_Dt.Columns.Add(DictionaryMain.kolumnaWartoscNetto, typeof(decimal));
                    m_Dt.Columns.Add(DictionaryMain.kolumnaStawkaVat);
                    m_Dt.Columns.Add(DictionaryMain.kolumnaKwotaVat, typeof(decimal));
                    m_Dt.Columns.Add(DictionaryMain.kolumnaWartoscBrutto, typeof(decimal));
                    if (!string.IsNullOrEmpty(MyDtString))
                    {
                       m_Dt = HelperXML.DeserializeObject<DataTable>(MyDtString);
                    }
                }

                return m_Dt;
            }

            set => m_Dt = value;
        }

        public FirmaData Sprzedawca
        {
            get
            {
                if (m_Sprzedawca == null)
                {
                    m_Sprzedawca = new FirmaData();
                }

                return m_Sprzedawca;
            }

            set => m_Sprzedawca = value;
        }

        public FirmaData Nabywca
        {
            get
            {
                if (m_Nabywca == null)
                {
                    m_Nabywca = new FirmaData();
                }

                return m_Nabywca;
            }

            set => m_Nabywca = value;
        }

        public bool Gotowka { get; set; }
        public bool Przelew { get; set; }
        public string NumerRachunku { get; set; }

        public Naglowek Naglowek
        {
            get
            {
                if (m_Naglowek == null)
                {

                    int month = DateTime.Now.Month;
                    int year = DateTime.Now.Year;
                    int day = DateTime.DaysInMonth(year, month);
                    m_Naglowek = new Naglowek
                    {
                        DataSprzedazy = new DateTime(year, month, day),
                        DataWystawienia = new DateTime(year, month, day)
                    };
                    m_Naglowek.TerminZaplaty = m_Naglowek.DataSprzedazy.AddDays(14);
                    m_Naglowek.NumerFaktury = "0001/" +  DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
                    m_Naglowek.DataUtworzenia = DateTime.Now;
                }

                return m_Naglowek;
            }

            set => m_Naglowek = value;
        }

        public string MyDtString { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
