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
        private bool _gotowka;
        private bool _przelew;
        private string _numerRachunku;
        private string _myDtString;

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
                     //   _dt =(DataTable)DeserializeObject(MyDtString);
                     //   XmlSerializationReader
                        _dt.ReadXml("dt.xml");
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

        public bool Gotowka { get => _gotowka; set => _gotowka = value; }
        public bool Przelew { get => _przelew; set => _przelew = value; }
        public string NumerRachunku { get => _numerRachunku; set => _numerRachunku = value; }

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
                    _naglowek.NumerFaktury = "0001/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                    _naglowek.DataUtworzenia = DateTime.Now;
                }

                return _naglowek;
            }

            set => _naglowek = value;
        }

        public string MyDtString { get => _myDtString; set => _myDtString = value; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
