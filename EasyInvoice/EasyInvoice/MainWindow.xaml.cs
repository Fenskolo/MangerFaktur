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
            DataRow row = dt.NewRow();

            for (int i = 0; i<dt.Columns.Count; i++)
            {
                row[i] = i;
            }
            dt.Rows.Add(row);
           // dt.Rows.Add();

            xDG.DataSource = dt.DefaultView;
         //   xDG.DataItems.Add(new ComboBox());
        }
    }

    public enum D
    {
        a=0,
        b=1,
            c=2
    }
}
