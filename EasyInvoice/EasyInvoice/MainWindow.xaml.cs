﻿using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyInvoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       

        public MainWindow()
        {
            InitializeComponent();
            FillValuesFaktura(null);            
            lblMiejsceWystawienia.Content = DictionaryMain.labelMiejsceWystawienia;
            lblDataWystawienia.Content = DictionaryMain.labelDataWystawienia;
            lblDataSprzedazy.Content = DictionaryMain.labelDataSprzedazy;
            lblTerminZaplaty.Content = DictionaryMain.labelTerminZaplaty;
            lblFormaPlatnosc.Content = DictionaryMain.labelFormaPlatnosci;
            lblSprzedawca.Content = DictionaryMain.labelHeaderSprzedawca;
            lblNabywca.Content = DictionaryMain.labelHeaderNabywca;
            lblSprzedawcaNazwa.Content = DictionaryMain.labelNazwaSprzedawcaNabywca;
            lblNabywcaNazwa.Content = DictionaryMain.labelNazwaSprzedawcaNabywca;

            lblSprzedawcaUlica.Content = DictionaryMain.labelUlicaSprzedawcaNabywca;
            lblSprzedawcaKodMiasto.Content = DictionaryMain.labelKodMiejsowoscSprzedawcaNabywca;
            lblSprzedawcaNip.Content = DictionaryMain.labelNIPSprzedawcaNabywca;
            lblSprzedawcaInne.Content = DictionaryMain.labelInnerSprzedawcaNabywca;
            lblNabywcaUlica.Content = DictionaryMain.labelUlicaSprzedawcaNabywca;
            lblNabywcaKodMiasto.Content = DictionaryMain.labelKodMiejsowoscSprzedawcaNabywca;
            lblNabywcaNip.Content = DictionaryMain.labelNIPSprzedawcaNabywca;
            lblNabywcaInne.Content = DictionaryMain.labelInnerSprzedawcaNabywca;
            lblNumerRachunku.Content = DictionaryMain.labelNumerRachunku;
            Gotowka.Content = "Gotówka";
            Przelew.Content = "Przelew";
            cbFaktura.SelectionChanged += CbFaktura_SelectionChanged;
        }

        public void FillValuesFaktura(int? id)
        {
            if (Property.Instance.Works.Count > 0)
            {                
                var x = id.HasValue? (WorkClass)Property.Instance.Works.Where(q=>q.Naglowek.Id==id).FirstOrDefault() 
                                    :(WorkClass)Property.Instance.Works.OrderByDescending(f => f.Naglowek.Id).First();
                SingleFakturaProperty.Singleton.MySingleton.Work = (WorkClass)x.Clone();
                SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek = null;
                SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek.MiejsceWystawienia = x.Naglowek.MiejsceWystawienia;
                SingleFakturaProperty.Singleton.MySingleton.Work.Sprzedawca = (FirmaData)x.Sprzedawca.Clone();
                SingleFakturaProperty.Singleton.MySingleton.Work.Nabywca = (FirmaData)x.Nabywca.Clone();
            }
            xDG.DataContext = null;
            xDG.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Dt.DefaultView;
            Nabywca.DataContext = null;
            Nabywca.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Nabywca;
            Sprzedawca.DataContext = null;
            Sprzedawca.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Sprzedawca;
            BankGotowka.DataContext = null;
            BankGotowka.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work;
            Naglowek.DataContext = null;
            Naglowek.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek;
            lblFaktura.Content = null;
            lblFaktura.Content = DictionaryMain.labelNrFaktury;

            cbFaktura.ItemsSource = Property.Instance.Works.Select(f => f.Naglowek.Id).ToList();
            cbFaktura.SelectedValue = SingleFakturaProperty.Singleton.Work.Naglowek.Id;            
        }

        private void CbFaktura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int myId = Convert.ToInt32((sender as ComboBox).SelectedItem);
            FillValuesFaktura(myId);
        }

        private void XDG1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombo(xDG);
        }

        public void PopulateCombo(XamDataGrid grid)
        {
            try
            {                
                ComboBoxField cbJ = (ComboBoxField)grid.DefaultFieldLayout.Fields[DictionaryMain.kolumnaJM];
                ComboBoxField cbV = (ComboBoxField)grid.DefaultFieldLayout.Fields[DictionaryMain.kolumnaStawkaVat];

                cbJ.ItemsSource = Property.Instance.NameList;
                cbV.ItemsSource = Property.Instance.StawkaList;  
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {     
            xDG.ActiveRecord =null;
            MakePdf a = new MakePdf(this);                      
        }

        private void Gotowka_Click(object sender, RoutedEventArgs e)
        {
            Przelew.IsChecked = !Gotowka.IsChecked ?? Gotowka.IsChecked;
            lblNumerRachunku.Visibility = (bool)Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
            txtNumerRachunku.Visibility = (bool)Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void Przelew_Click(object sender, RoutedEventArgs e)
        {
            Gotowka.IsChecked = !Przelew.IsChecked ?? Przelew.IsChecked;
            lblNumerRachunku.Visibility = (bool)Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
            txtNumerRachunku.Visibility = (bool)Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void xDG_CellUpdated(object sender, Infragistics.Windows.DataPresenter.Events.CellUpdatedEventArgs e)
        {
            DataRecord row = e.Record;
            Cell stawkaVat = row.Cells[DictionaryMain.kolumnaStawkaVat];
            Cell ilosc = row.Cells[DictionaryMain.kolumnaIlosc];
            Cell wartoscNetto = row.Cells[DictionaryMain.kolumnaWartoscNetto];
            Cell cenaNetto = row.Cells[DictionaryMain.kolumnaCenaNetto];
            Cell kwotaVat = row.Cells[DictionaryMain.kolumnaKwotaVat];
            Cell wartoscBrutto = row.Cells[DictionaryMain.kolumnaWartoscBrutto];

                    if (ilosc.Value != DBNull.Value && ilosc.Value != null 
                    && cenaNetto.Value !=DBNull.Value &&
                        (wartoscNetto.Value == DBNull.Value || Convert.ToDecimal(wartoscNetto.Value) != GetValueIloraz(ilosc, cenaNetto)))
                    {
                        wartoscNetto.Value = GetValueIloraz(ilosc, cenaNetto);
                    }

                    if (!string.IsNullOrEmpty(stawkaVat.Value.ToString()) 
                     && wartoscNetto.Value != DBNull.Value && wartoscNetto.Value != null 
                     && (wartoscBrutto.Value == DBNull.Value || (decimal)wartoscBrutto.Value != GetValueIloraz(wartoscNetto, stawkaVat)))
                    {
                        wartoscBrutto.Value = GetValueIloraz(wartoscNetto, stawkaVat);
                    }

                    if (wartoscBrutto.Value != DBNull.Value && wartoscBrutto.Value != null 
                     && wartoscNetto.Value != null
                        && (kwotaVat.Value == DBNull.Value || (decimal)kwotaVat.Value != (decimal)wartoscBrutto.Value - (decimal)wartoscNetto.Value))
                    {
                        kwotaVat.Value = (decimal)wartoscBrutto.Value - (decimal)wartoscNetto.Value;
                    }
        }

        private static decimal getDecimalVatStawka(DataRecord row)
        {
            return 1 + Convert.ToDecimal(row.Cells[DictionaryMain.kolumnaStawkaVat].Value.ToString().Split('%')[0]) / 100;
        }

        private decimal GetValueIloraz(Cell first, Cell second)
        {
            decimal sec = second.Field.Name == DictionaryMain.kolumnaStawkaVat ? getDecimalVatStawka(second.Record) : Convert.ToDecimal(second.Value);
            decimal myDec = Convert.ToDecimal(first.Value) *  sec;

            return myDec;
        }
    }    
}
