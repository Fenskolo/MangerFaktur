using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EasyInvoice.Properties;

namespace EasyInvoice
{
    public class MakePdf
    {
        public MakePdf(MainWindow mw, SingleFakturaProperty fakturaProperty)
        {
            var fileName = Path.Combine(Settings.Default.pathDest,
                $"{DateTime.Now:yyyyMMddHHmmss}plik.pdf");
            new GenerujPolaWDokumencie(fileName, fakturaProperty);

            Process.Start(fileName);

            var idFaktura = AddInvoiceValuesDb(mw, Settings.Default.dbConn, fakturaProperty.Work);

            foreach (DataRow row in fakturaProperty.Work.Dt.Rows)
                AddInvoiceExternalValuesDb(mw, Settings.Default.dbConn, idFaktura, row);

            fakturaProperty.Work.MyDtString = fakturaProperty.Work.Dt.Serialize();

            var myId = 1;
            var que = true;
            while (que)
            {
                myId++;
                que = Property.Instance.Works.Any(f => f.Naglowek.Id == myId);
            }

            fakturaProperty.Work.Naglowek.Id = myId;
            var x = (WorkClass) fakturaProperty.Work.Clone();
            x.Naglowek = (Naglowek) fakturaProperty.Work.Naglowek.Clone();
            x.Nabywca = (FirmaData) fakturaProperty.Work.Nabywca.Clone();
            x.Sprzedawca = (FirmaData) fakturaProperty.Work.Sprzedawca.Clone();
            Property.Instance.Works.Add(x);
            Property.Instance.NameList = Property.Instance.NameList.Distinct().ToList();
            Property.Instance.StawkaList = Property.Instance.StawkaList.Distinct().ToList();
            Property.SerializeXml();

            fakturaProperty = null;
            fakturaProperty.Work = null;
            mw.FillValuesFaktura(null);
        }

        private void AddInvoiceExternalValuesDb(MainWindow mw, string connection, int idFaktura, DataRow row)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.addNewUslugaFaktura";
                cmd.Parameters.AddWithValue("@id_faktury", idFaktura);
                cmd.Parameters.AddWithValue("@id_usluga",
                    GetIdFromTable(mw.Usluga, row[DictionaryMain.KolumnaTowar].ToString()));
                cmd.Parameters.AddWithValue("@id_jednostka",
                    GetIdFromTable(mw.Jednostka, row[DictionaryMain.KolumnaJm].ToString()));
                cmd.Parameters.AddWithValue("@iloscJednostek", row[DictionaryMain.KolumnaIlosc]);
                cmd.Parameters.AddWithValue("@cenaNetto", row[DictionaryMain.KolumnaCenaNetto]);
                cmd.Parameters.AddWithValue("@wartoscNetto", row[DictionaryMain.KolumnaWartoscNetto]);
                cmd.Parameters.AddWithValue("@id_stawkaVat",
                    GetIdFromTable(mw.StawkaVat, row[DictionaryMain.KolumnaStawkaVat].ToString()));
                cmd.Parameters.AddWithValue("@kwotaVat", row[DictionaryMain.KolumnaKwotaVat]);
                cmd.Parameters.AddWithValue("@wartoscBrutto", row[DictionaryMain.KolumnaWartoscBrutto]);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private int AddInvoiceValuesDb(MainWindow mw, string connection, WorkClass workClass)
        {
            int idFaktura;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.addNewFaktura";
                cmd.Parameters.AddWithValue("@nr_faktury", workClass.Naglowek.NumerFaktury);
                cmd.Parameters.AddWithValue("@miejsceWystawienia", workClass.Naglowek.MiejsceWystawienia);
                cmd.Parameters.AddWithValue("@dataWystawienia", workClass.Naglowek.DataWystawienia);
                cmd.Parameters.AddWithValue("@dataSprzedazy", workClass.Naglowek.DataSprzedazy);
                cmd.Parameters.AddWithValue("@dataZaplaty", workClass.Naglowek.TerminZaplaty);
                cmd.Parameters.AddWithValue("@id_formaPlatnosci",
                    GetIdFromTable(mw.FormaPlatnosc, workClass.Naglowek.FormaPlatnosci));
                cmd.Parameters.AddWithValue("@id_typPlatnosci", GetIdFromTable(mw.TypPlatnosci, "przelew"));
                cmd.Parameters.AddWithValue("@id_sprzedawcy",
                    GetIdFromTable(mw.Firma, workClass.Sprzedawca.NazwaFirmy));
                cmd.Parameters.AddWithValue("@id_nabywcy", GetIdFromTable(mw.Firma, workClass.Nabywca.NazwaFirmy));

                cmd.Connection.Open();
                idFaktura = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return idFaktura;
        }

        private object GetIdFromTable(DataTable dt, string searchValue)
        {
            return dt.Rows
                .Cast<DataRow>()
                .FirstOrDefault(w => w[1].ToString() == searchValue)?.ItemArray[0];
        }
    }
}