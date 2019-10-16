using System;
using System.Data;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable]
    public class WorkClass : ICloneable
    {
        private DataTable _mDt;
        private FirmaData _mNabywca;
        private Naglowek _mNaglowek;
        private FirmaData _mSprzedawca;


        [XmlIgnore]
        public DataTable Dt
        {
            get
            {
                if (_mDt == null)
                {
                    _mDt = new DataTable
                    {
                        TableName = "TabelaFaktura"
                    };
                    _mDt.Columns.Add(DictionaryMain.KolumnaTowar, typeof(string));
                    _mDt.Columns.Add(DictionaryMain.KolumnaJm);
                    _mDt.Columns.Add(DictionaryMain.KolumnaIlosc, typeof(int));
                    _mDt.Columns.Add(DictionaryMain.KolumnaCenaNetto, typeof(decimal));
                    _mDt.Columns.Add(DictionaryMain.KolumnaWartoscNetto, typeof(decimal));
                    _mDt.Columns.Add(DictionaryMain.KolumnaStawkaVat);
                    _mDt.Columns.Add(DictionaryMain.KolumnaKwotaVat, typeof(decimal));
                    _mDt.Columns.Add(DictionaryMain.KolumnaWartoscBrutto, typeof(decimal));
                    if (!string.IsNullOrEmpty(MyDtString)) _mDt = HelperXml.DeserializeObject<DataTable>(MyDtString);
                }

                return _mDt;
            }

            set => _mDt = value;
        }

        public FirmaData Sprzedawca
        {
            get => _mSprzedawca ?? (_mSprzedawca = new FirmaData());

            set => _mSprzedawca = value;
        }

        public FirmaData Nabywca
        {
            get => _mNabywca ?? (_mNabywca = new FirmaData());

            set => _mNabywca = value;
        }

        public bool Gotowka { get; set; }
        public bool Przelew { get; set; }
        public string NumerRachunku { get; set; }

        public Naglowek Naglowek
        {
            get
            {
                if (_mNaglowek == null)
                {
                    var month = DateTime.Now.Month;
                    var year = DateTime.Now.Year;
                    var day = DateTime.DaysInMonth(year, month);
                    _mNaglowek = new Naglowek
                    {
                        DataSprzedazy = new DateTime(year, month, day),
                        DataWystawienia = new DateTime(year, month, day)
                    };
                    _mNaglowek.TerminZaplaty = _mNaglowek.DataSprzedazy.AddDays(14);
                    _mNaglowek.NumerFaktury = "0001/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
                    _mNaglowek.DataUtworzenia = DateTime.Now;
                }

                return _mNaglowek;
            }

            set => _mNaglowek = value;
        }

        public string MyDtString { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}