using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasyInvoice
{
    [Serializable]
    public class Property 
    {
        private static Property _instance;
        private static readonly string xmlPath = "xmlProperty.xml";
        List<string> _nameList;
        List<string> _stawkaList;
        List<WorkClass> _works;
        public Property() { }

        public static Property Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (File.Exists(xmlPath))
                    {
                        _instance=Deserialize();
                    }
                    else
                    {
                        _instance = new Property();
                    }
                }
                return _instance;
            }
            set => _instance = value;
        }

        public List<string> NameList
        {
            get
            {
                if (_nameList == null)
                {
                    _nameList = new List<string>()
                    {
                        "szt.",
                        "godz.",
                        "m2",
                        "op.",
                        "mc.",
                        "ton",
                        "km.",
                        "m3."
                    };
                }
                return _nameList;
            }
            set
            {
                value.Where(f => NameList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                _nameList = value;
            }
        }

        public List<string> StawkaList
        {
            get
            {
                if (_stawkaList == null)
                {
                    _stawkaList = new List<string>()
                    {
                        "23%",
                        "0%",
                        "7%"
                    };
                }
                return _stawkaList;
            }
            set
            {
                value.Where(f => StawkaList.Contains(f)).ToList().ForEach(a => value.Remove(a));
                _stawkaList = value;
            }
        }

        public List<WorkClass> Works
        { get
            {
                if(_works == null)
                {
                    _works = new List<WorkClass>();
                }
                return _works;
            }
           
            set => _works = value; }

        public static Property Deserialize()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Property));
                reader = new StreamReader(xmlPath);
                return (Property)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static void SerializeXml()
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(Property));
                writer = new StreamWriter(xmlPath, false);
                xsSubmit.Serialize(writer, Property.Instance);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }

    public class DictionaryMain
    {
        public const string labelNrFaktury = "Faktura";
        public const string labelMiejsceWystawienia = "MIEJSCE WYSTAWIENIA:";
        public const string labelDataWystawienia = "DATA WYSTAWIENIA:";
        public const string labelDataSprzedazy = "DATA SPRZEDAŻY:";
        public const string labelTerminZaplaty = "TERMIN ZAPŁATY:";
        public const string labelFormaPlatnosci = "FORMA PŁATNOŚĆ:";
        public const string labelHeaderSprzedawca = "Sprzedawca";
        public const string labelHeaderNabywca = "Nabywca i płatnik";
        public const string labelNazwaSprzedawcaNabywca = "Nazwa firmy:";
        public const string labelUlicaSprzedawcaNabywca = "Ulica:";
        public const string labelKodMiejsowoscSprzedawcaNabywca = "Kod p. i miejscowość:";
        public const string labelNIPSprzedawcaNabywca = "NIP:";
        public const string labelInnerSprzedawcaNabywca = "Inne:";
        public const string labelNumerRachunku = "Numer rachunku bankowego: ";
        public const string labelZaplacone = "Zapłacono:";
        public const string labelDoZaplaty = "Do zapłaty";
        public const string labelRazem = "Razem:";
        public const string labelSlownie = "Słownie:";
        public const string labelPodpisWystawiania = "Podpis osoby upoważnionej do wystawienia faktury";
        public const string labelPodpisOdbierania = "Podpis osoby upoważnionej do odebrania faktury";

        ///////////
        ///Tabela
        ///
        public const string kolumnaLp= "Lp.";
        public const string kolumnaTowar= "Towar / usługa";
        public const string kolumnaJM= "J.m.";
        public const string kolumnaIlosc= "Ilość";
        public const string kolumnaCenaNetto= "Cena Netto";
        public const string kolumnaWartoscNetto= "Wartość netto";
        public const string kolumnaStawkaVat= "Stawka VAT";
        public const string kolumnaKwotaVat= "Kwota VAT";
        public const string kolumnaWartoscBrutto= "Wartość Brutto";


        public const string summaRazem = "Razem";
        public const string summaWTym = "W tym";
    }


}
