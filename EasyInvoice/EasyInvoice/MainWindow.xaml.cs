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
            InitializeComponent();
            SingleFakturaProperty sf = new SingleFakturaProperty();          
            
            xDG.DataContext = sf.Dt.DefaultView;

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
        }

        private void XDG1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombo(xDG);
        }

        public void PopulateCombo(XamDataGrid grid)
        {
            ComboBoxField cbJ = grid.DefaultFieldLayout.Fields[DictionaryMain.kolumnaJM] as ComboBoxField;            
            ComboBoxField cbV = grid.DefaultFieldLayout.Fields[DictionaryMain.kolumnaStawkaVat] as ComboBoxField;
            
            cbJ.ItemsSource = Property.Instance.NameList;
            cbV.ItemsSource = Property.Instance.StawkaList;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakePdf a = new MakePdf();                      
        }
    }    
}
