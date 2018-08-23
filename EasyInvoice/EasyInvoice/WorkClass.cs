using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable()]
    public class WorkClass : ICloneable
    {
        private DataTable _dt;
        private FirmaData _sprzedawca;
        private FirmaData _nabywca;
        private Naglowek _naglowek;

        [XmlIgnore]
        public DataTable Dt
        {
            get
            {
                if (_dt == null)
                {

                    _dt = new DataTable
                    {
                        TableName = "TabelaFaktura"
                    };
                    _dt.Columns.Add(DictionaryMain.kolumnaTowar, typeof(string));
                    _dt.Columns.Add(DictionaryMain.kolumnaJM);
                    _dt.Columns.Add(DictionaryMain.kolumnaIlosc, typeof(Int32));
                    _dt.Columns.Add(DictionaryMain.kolumnaCenaNetto, typeof(decimal));
                    _dt.Columns.Add(DictionaryMain.kolumnaWartoscNetto, typeof(decimal));
                    _dt.Columns.Add(DictionaryMain.kolumnaStawkaVat);
                    _dt.Columns.Add(DictionaryMain.kolumnaKwotaVat, typeof(decimal));
                    _dt.Columns.Add(DictionaryMain.kolumnaWartoscBrutto, typeof(decimal));
                    if (!string.IsNullOrEmpty(MyDtString))
                    {
                       // _dt.ReadXml("dt.xml");
                    //}
                    //else
                    //{
                       _dt = HelperXML.DeserializeObject<DataTable>(MyDtString);
                    }
                }

                return _dt;
            }

            set => _dt = value;
        }

        public FirmaData Sprzedawca
        {
            get
            {
                if (_sprzedawca == null)
                {
                    _sprzedawca = new FirmaData();
                }

                return _sprzedawca;
            }

            set => _sprzedawca = value;
        }

        public FirmaData Nabywca
        {
            get
            {
                if (_nabywca == null)
                {
                    _nabywca = new FirmaData();
                }

                return _nabywca;
            }

            set => _nabywca = value;
        }

        public bool Gotowka { get; set; }
        public bool Przelew { get; set; }
        public string NumerRachunku { get; set; }

        public Naglowek Naglowek
        {
            get
            {
                if (_naglowek == null)
                {

                    int month = DateTime.Now.Month;
                    int year = DateTime.Now.Year;
                    int day = DateTime.DaysInMonth(year, month);
                    _naglowek = new Naglowek
                    {
                        DataSprzedazy = new DateTime(year, month, day),
                        DataWystawienia = new DateTime(year, month, day)
                    };
                    _naglowek.TerminZaplaty = _naglowek.DataSprzedazy.AddDays(14);
                    _naglowek.NumerFaktury = "0001/" +  DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
                    _naglowek.DataUtworzenia = DateTime.Now;
                }

                return _naglowek;
            }

            set => _naglowek = value;
        }

        public string MyDtString { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
