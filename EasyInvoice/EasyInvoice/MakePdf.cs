using EasyInvoice.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EasyInvoice
{
    public class MakePdf
    {
        public MakePdf(MainWindow mw, SingleFakturaProperty fakturaProperty)
        {
            var fileName = Path.Combine(Settings.Default.pathDest, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}plik.pdf");
            var p = new GenerujPolaWDokumencie(fileName, fakturaProperty);

            Process.Start(fileName);

            var idFaktura = AddInvoiceValuesDb(mw, Settings.Default.dbConn, fakturaProperty.Work);

            foreach (DataRow row in fakturaProperty.Work.Dt.Rows)
            {
                AddInvoiceExternalValuesDb(mw, Settings.Default.dbConn, idFaktura, row);
            }

            fakturaProperty.Work.MyDtString = fakturaProperty.Work.Dt.Serialize();

            int myID = 1;
            bool que = true;
            while (que)
            {
                myID++;
                que = !Property.Instance.Works.All(f => f.Naglowek.Id != myID);
            }
            fakturaProperty.Work.Naglowek.Id = myID;
            var x = (WorkClass)fakturaProperty.Work.Clone();
            x.Naglowek = (Naglowek)fakturaProperty.Work.Naglowek.Clone();
            x.Nabywca = (FirmaData)fakturaProperty.Work.Nabywca.Clone();
            x.Sprzedawca = (FirmaData)fakturaProperty.Work.Sprzedawca.Clone();
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
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.addNewUslugaFaktura";
                cmd.Parameters.AddWithValue("@id_faktury", idFaktura);
                cmd.Parameters.AddWithValue("@id_usluga", GetIdFromTable(mw.usluga, row[DictionaryMain.kolumnaTowar].ToString()));
                cmd.Parameters.AddWithValue("@id_jednostka", GetIdFromTable(mw.jednostka, row[DictionaryMain.kolumnaJM].ToString()));
                cmd.Parameters.AddWithValue("@iloscJednostek", row[DictionaryMain.kolumnaIlosc]);
                cmd.Parameters.AddWithValue("@cenaNetto", row[DictionaryMain.kolumnaCenaNetto]);
                cmd.Parameters.AddWithValue("@wartoscNetto", row[DictionaryMain.kolumnaWartoscNetto]);
                cmd.Parameters.AddWithValue("@id_stawkaVat", GetIdFromTable(mw.stawkaVat, row[DictionaryMain.kolumnaStawkaVat].ToString()));
                cmd.Parameters.AddWithValue("@kwotaVat", row[DictionaryMain.kolumnaKwotaVat]);
                cmd.Parameters.AddWithValue("@wartoscBrutto", row[DictionaryMain.kolumnaWartoscBrutto]);
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
                cmd.Parameters.AddWithValue("@id_formaPlatnosci", GetIdFromTable(mw.formaPlatnosc, workClass.Naglowek.FormaPlatnosci));
                cmd.Parameters.AddWithValue("@id_typPlatnosci", GetIdFromTable(mw.typPlatnosci, "przelew"));
                cmd.Parameters.AddWithValue("@id_sprzedawcy", GetIdFromTable(mw.firma, workClass.Sprzedawca.NazwaFirmy));
                cmd.Parameters.AddWithValue("@id_nabywcy", GetIdFromTable(mw.firma, workClass.Nabywca.NazwaFirmy));

                cmd.Connection.Open();
                idFaktura = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return idFaktura;
        }

        private object GetIdFromTable(DataTable dt, string searchValue)
        {
            return dt.Rows.Cast<DataRow>()
                .Where(w => w[1].ToString() == searchValue)
                .FirstOrDefault()?.ItemArray[0];
        }
    }
}
