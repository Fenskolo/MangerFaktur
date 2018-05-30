using Infragistics.Windows.DataPresenter;
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
            SingleFakturaProperty.Singleton.MySingleton = HelperXML.Deserialize();
            InitializeComponent();

            xDG.DataContext = SingleFakturaProperty.Singleton.Dt.DefaultView;

            lblFaktura.Content = DictionaryMain.labelNrFaktury;
            lblMiejsceWystawienia.Content = DictionaryMain.labelMiejsceWystawienia;
            lblDataWystawienia  .Content = DictionaryMain.labelDataWystawienia;
            lblDataSprzedazy  .Content = DictionaryMain.labelDataSprzedazy;
            lblTerminZaplaty  .Content = DictionaryMain.labelTerminZaplaty;
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

            Nabywca.DataContext = SingleFakturaProperty.Singleton.Nabywca;
            Sprzedawca.DataContext = SingleFakturaProperty.Singleton.Sprzedawca;
          //  PrzelewG.DataContext = sf;
            BankGotowka.DataContext = SingleFakturaProperty.Singleton;
            Naglowek.DataContext = SingleFakturaProperty.Singleton.Naglowek;

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
            MakePdf a = new MakePdf();                      
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
    }    
}
