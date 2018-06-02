using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{
    public class Property
    {
        private static Property _instance;
        List<string> _nameList;
        List<string> _stawkaList;
        public Property() { }

        public static Property Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Property();
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

            set => _nameList = value;
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

            set => _stawkaList = value;
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
