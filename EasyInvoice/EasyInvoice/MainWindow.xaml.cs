using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Data;
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
            dt.Columns.Add("Lp.");
            dt.Columns.Add("Towar / usługa");
            dt.Columns.Add("J.m.");
            dt.Columns.Add("Ilość");
            dt.Columns.Add("Cena Netto");
            dt.Columns.Add("Wartość netto");
            dt.Columns.Add("Stawka VAT");
            dt.Columns.Add("Kwota VAT");
            dt.Columns.Add("Wartość Brutto");
            //DataRow row = dt.NewRow();

            //for (int i = 0; i<dt.Columns.Count; i++)
            //{
            //    row[i] = i;
            //}
            //dt.Rows.Add(row);
           // dt.Rows.Add();

            xDG.DataContext = dt.DefaultView;
            xcv = dt;
         //   xDG.DataItems.Add(new ComboBox());
        }

        private void XDG1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombo(xDG);
           // this.xDG.AddNewRowSettings.AllowAddNewRow = Infragistics.Controls.Grids.AddNewRowLocation.b;
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
          //  xDG.DefaultFieldLayout.ac
            var x =xcv.Rows.Count;
        }
    }


    public enum D
    {
        a=0,
        b=1,
        c=2
    }
}
