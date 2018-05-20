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
        private DataTable xcv;
        public MainWindow()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("Towar / usługa", typeof(string));
            dt.Columns.Add("J.m.");
            dt.Columns.Add("Ilość", typeof(Int32));
            dt.Columns.Add("Cena Netto", typeof(double));
            dt.Columns.Add("Wartość netto", typeof(double));
            dt.Columns.Add("Stawka VAT");
            dt.Columns.Add("Kwota VAT", typeof(double));
            dt.Columns.Add("Wartość Brutto", typeof(double));
            
            xDG.DataContext = dt.DefaultView;
            xcv = dt;
        }

        private void XDG1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombo(xDG);
        }

        public void PopulateCombo(XamDataGrid grid)
        {
            ComboBoxField cbJ = grid.DefaultFieldLayout.Fields["J.m."] as ComboBoxField;
            //  DataView dv = grid.DataContext as DataView;

            List<string> nameList = new List<string>
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
            ComboBoxField cbV = grid.DefaultFieldLayout.Fields["Stawka VAT"] as ComboBoxField;


            List<string> stawkaList = new List<string>
            {
                "23%"
            };
            cbJ.ItemsSource = nameList;
            cbV.ItemsSource = stawkaList;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MakePdf a = new MakePdf();                      
        }
    }    
}
