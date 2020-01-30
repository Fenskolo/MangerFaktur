using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EasyInvoice.Properties;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;

namespace EasyInvoice
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly DataTable Firma;
        public readonly DataTable FormaPlatnosc;
        public readonly DataTable Jednostka;
        public readonly DataTable StawkaVat;
        public readonly DataTable TypPlatnosci;
        public readonly DataTable Usluga;

        public MainWindow()
        {
            InitializeComponent();

            var ds = GetDataSetDb();
            FormaPlatnosc = ds.Tables[0];
            Jednostka = ds.Tables[1];
            StawkaVat = ds.Tables[2];
            TypPlatnosci = ds.Tables[3];
            Usluga = ds.Tables[4];
            Firma = ds.Tables[5];

            FillValuesFaktura(null);
            LblMiejsceWystawienia.Content = DictionaryMain.LabelMiejsceWystawienia;
            LblDataWystawienia.Content = DictionaryMain.LabelDataWystawienia;
            LblDataSprzedazy.Content = DictionaryMain.LabelDataSprzedazy;
            LblTerminZaplaty.Content = DictionaryMain.LabelTerminZaplaty;
            LblFormaPlatnosc.Content = DictionaryMain.LabelFormaPlatnosci;
            LblSprzedawca.Content = DictionaryMain.LabelHeaderSprzedawca;
            LblNabywca.Content = DictionaryMain.LabelHeaderNabywca;
            LblSprzedawcaNazwa.Content = DictionaryMain.LabelNazwaSprzedawcaNabywca;
            LblNabywcaNazwa.Content = DictionaryMain.LabelNazwaSprzedawcaNabywca;

            LblSprzedawcaUlica.Content = DictionaryMain.LabelUlicaSprzedawcaNabywca;
            LblSprzedawcaKodMiasto.Content = DictionaryMain.LabelKodMiejsowoscSprzedawcaNabywca;
            LblSprzedawcaNip.Content = DictionaryMain.LabelNipSprzedawcaNabywca;
            LblSprzedawcaInne.Content = DictionaryMain.LabelInnerSprzedawcaNabywca;
            LblNabywcaUlica.Content = DictionaryMain.LabelUlicaSprzedawcaNabywca;
            LblNabywcaKodMiasto.Content = DictionaryMain.LabelKodMiejsowoscSprzedawcaNabywca;
            LblNabywcaNip.Content = DictionaryMain.LabelNipSprzedawcaNabywca;
            LblNabywcaInne.Content = DictionaryMain.LabelInnerSprzedawcaNabywca;
            LblNumerRachunku.Content = DictionaryMain.LabelNumerRachunku;
            Gotowka.Content = "Gotówka";
            Przelew.Content = "Przelew";
            CbFaktura.SelectionChanged += CbFaktura_SelectionChanged;
        }

        private static DataSet GetDataSetDb()
        {
            var ds = new DataSet();
            using (var adapter = new SqlDataAdapter())
            {
                using (var cmd = new SqlCommand())
                {
                    adapter.SelectCommand = cmd;
                    cmd.Connection = new SqlConnection(Settings.Default.dbConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.getSlowniki";
                    cmd.Connection.Open();
                    adapter.Fill(ds);
                }
            }

            return ds;
        }

        public void FillValuesFaktura(int? id)
        {
            if (Property.Instance.Works.Count > 0)
            {
                var x = id.HasValue
                    ? Property.Instance.Works.FirstOrDefault(q => q.Naglowek.Id == id)
                    : Property.Instance.Works.OrderByDescending(f => f.Naglowek.Id).First();

                if (x != null)
                {
                    SingleFakturaProperty.Singleton.MySingleton.Work = (WorkClass) x.Clone();
                    SingleFakturaProperty.Singleton.MySingleton.Work.Dt = null;
                    SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek = null;
                    if (SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek != null)
                    {
                        SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek.FormaPlatnosci =
                            x.Naglowek.FormaPlatnosci;
                        SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek.MiejsceWystawienia =
                            x.Naglowek.MiejsceWystawienia;
                    }

                    SingleFakturaProperty.Singleton.MySingleton.Work.Sprzedawca = (FirmaData) x.Sprzedawca.Clone();
                    SingleFakturaProperty.Singleton.MySingleton.Work.Nabywca = (FirmaData) x.Nabywca.Clone();
                }
            }

            XDg.DataContext = null;
            if (SingleFakturaProperty.Singleton.MySingleton.Work.Dt != null)
            {
                XDg.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Dt.DefaultView;
            }

            Nabywca.DataContext = null;
            Nabywca.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Nabywca;
            Sprzedawca.DataContext = null;
            Sprzedawca.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Sprzedawca;
            BankGotowka.DataContext = null;
            BankGotowka.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work;
            Naglowek.DataContext = null;
            Naglowek.DataContext = SingleFakturaProperty.Singleton.MySingleton.Work.Naglowek;
            LblFaktura.Content = null;
            LblFaktura.Content = DictionaryMain.LabelNrFaktury;

            CbFaktura.ItemsSource = Property.Instance.Works.Select(f => f.Naglowek.Id).ToList();
            CbFaktura.SelectedValue = SingleFakturaProperty.Singleton.Work.Naglowek.Id;
        }

        private void CbFaktura_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var myId = Convert.ToInt32((sender as ComboBox)?.SelectedItem);
            FillValuesFaktura(myId);
        }

        private void XDG1_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateCombo(XDg);
        }

        private void PopulateCombo(XamDataGrid grid)
        {
            try
            {
                var cbJ = (ComboBoxField) grid.DefaultFieldLayout.Fields[DictionaryMain.KolumnaJm];
                var cbV = (ComboBoxField) grid.DefaultFieldLayout.Fields[DictionaryMain.KolumnaStawkaVat];
                var cbN = (ComboBoxField) grid.DefaultFieldLayout.Fields[DictionaryMain.KolumnaTowar];

                cbJ.ItemsSource = Jednostka.Rows.Cast<DataRow>().Select(s => s[1]).ToList();
                cbV.ItemsSource = StawkaVat.Rows.Cast<DataRow>().Select(s => s[1]).ToList();
                cbN.ItemsSource = Usluga.Rows.Cast<DataRow>().Select(s => s[1]).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XDg.ActiveRecord = null;
            var makePdf = new MakePdf(this, SingleFakturaProperty.Singleton);
        }

        private void Gotowka_Click(object sender, RoutedEventArgs E)
        {
            Przelew.IsChecked = !Gotowka.IsChecked ?? Gotowka.IsChecked;
            LblNumerRachunku.Visibility = (bool) Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
            TxtNumerRachunku.Visibility = (bool) Przelew.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void Przelew_Click(object Sender, RoutedEventArgs E)
        {
            Gotowka.IsChecked = !Przelew.IsChecked ?? Przelew.IsChecked;
            LblNumerRachunku.Visibility = Przelew.IsChecked != null && (bool) Przelew.IsChecked
                ? Visibility.Visible
                : Visibility.Hidden;
            TxtNumerRachunku.Visibility = Przelew.IsChecked != null && (bool) Przelew.IsChecked
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void xDG_CellUpdated(object Sender, CellUpdatedEventArgs E)
        {
            var row = E.Record;
            var stawkaVat = row.Cells[DictionaryMain.KolumnaStawkaVat];
            var ilosc = row.Cells[DictionaryMain.KolumnaIlosc];
            var wartoscNetto = row.Cells[DictionaryMain.KolumnaWartoscNetto];
            var cenaNetto = row.Cells[DictionaryMain.KolumnaCenaNetto];
            var kwotaVat = row.Cells[DictionaryMain.KolumnaKwotaVat];
            var wartoscBrutto = row.Cells[DictionaryMain.KolumnaWartoscBrutto];

            if (ilosc.Value != DBNull.Value && ilosc.Value != null
                                            && cenaNetto.Value != DBNull.Value &&
                                            (wartoscNetto.Value == DBNull.Value ||
                                             Convert.ToDecimal(wartoscNetto.Value) != GetValueIloraz(ilosc, cenaNetto)))
            {
                wartoscNetto.Value = GetValueIloraz(ilosc, cenaNetto);
            }

            if (!string.IsNullOrEmpty(stawkaVat.Value.ToString())
                && wartoscNetto.Value != DBNull.Value && wartoscNetto.Value != null
                && (wartoscBrutto.Value == DBNull.Value ||
                    (decimal) wartoscBrutto.Value != GetValueIloraz(wartoscNetto, stawkaVat)))
            {
                wartoscBrutto.Value = GetValueIloraz(wartoscNetto, stawkaVat);
            }

            if (wartoscBrutto.Value != DBNull.Value && wartoscBrutto.Value != null
                                                    && wartoscNetto.Value != null
                                                    && (kwotaVat.Value == DBNull.Value || (decimal) kwotaVat.Value !=
                                                        (decimal) wartoscBrutto.Value - (decimal) wartoscNetto.Value))
            {
                kwotaVat.Value = (decimal) wartoscBrutto.Value - (decimal) wartoscNetto.Value;
            }
        }

        private static decimal GetDecimalVatStawka(DataRecord Row)
        {
            return 1 + Convert.ToDecimal(Row.Cells[DictionaryMain.KolumnaStawkaVat].Value.ToString().Split('%')[0]) /
                   100;
        }

        private decimal GetValueIloraz(Cell first, Cell second)
        {
            var sec = second.Field.Name == DictionaryMain.KolumnaStawkaVat
                ? GetDecimalVatStawka(second.Record)
                : Convert.ToDecimal(second.Value);
            var myDec = Convert.ToDecimal(first.Value) * sec;

            return myDec;
        }
    }
}