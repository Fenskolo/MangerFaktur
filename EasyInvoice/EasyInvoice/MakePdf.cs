using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace EasyInvoice
{
    public class MakePdf
    {
        public MakePdf(MainWindow mw)
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "plik" + ".pdf";
            fileName = Path.Combine(Properties.Settings.Default.pathDest, fileName);
            var p = new GenerujPolaWDokumencie(fileName, new HelperData(SingleFakturaProperty.Singleton), new DaneNaglowek(SingleFakturaProperty.Singleton.Work.Naglowek), SingleFakturaProperty.Singleton);

            System.Diagnostics.Process.Start(fileName);

            int idFaktura;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(Properties.Settings.Default.dbConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.addNewFaktura";
                cmd.Parameters.AddWithValue("@nr_faktury", SingleFakturaProperty.Singleton.Work.Naglowek.NumerFaktury);
                cmd.Parameters.AddWithValue("@miejsceWystawienia", SingleFakturaProperty.Singleton.Work.Naglowek.MiejsceWystawienia);
                cmd.Parameters.AddWithValue("@dataWystawienia", SingleFakturaProperty.Singleton.Work.Naglowek.DataWystawienia);
                cmd.Parameters.AddWithValue("@dataSprzedazy", SingleFakturaProperty.Singleton.Work.Naglowek.DataSprzedazy);
                cmd.Parameters.AddWithValue("@dataZaplaty", SingleFakturaProperty.Singleton.Work.Naglowek.TerminZaplaty);
                cmd.Parameters.AddWithValue("@id_formaPlatnosci", GetIdFromTable(mw.formaPlatnosc, SingleFakturaProperty.Singleton.Work.Naglowek.FormaPlatnosci));
                cmd.Parameters.AddWithValue("@id_typPlatnosci", GetIdFromTable(mw.typPlatnosci, "przelew"));
                cmd.Parameters.AddWithValue("@id_sprzedawcy", GetIdFromTable(mw.firma, SingleFakturaProperty.Singleton.Work.Sprzedawca.NazwaFirmy));
                cmd.Parameters.AddWithValue("@id_nabywcy", GetIdFromTable(mw.firma, SingleFakturaProperty.Singleton.Work.Nabywca.NazwaFirmy));

                cmd.Connection.Open();
                idFaktura = Convert.ToInt32(cmd.ExecuteScalar());
            }

            foreach (DataRow row in SingleFakturaProperty.Singleton.Work.Dt.Rows)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = new SqlConnection(Properties.Settings.Default.dbConn);
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



            SingleFakturaProperty.Singleton.Work.MyDtString = SingleFakturaProperty.Singleton.Work.Dt.Serialize();

            int myID = 1;
            bool que = true;
            while (que)
            {
                myID++;
                que = !Property.Instance.Works.All(f => f.Naglowek.Id != myID);
            }
            SingleFakturaProperty.Singleton.Work.Naglowek.Id = myID;
            var x = (WorkClass)SingleFakturaProperty.Singleton.Work.Clone();
            x.Naglowek = (Naglowek)SingleFakturaProperty.Singleton.Work.Naglowek.Clone();
            x.Nabywca = (FirmaData)SingleFakturaProperty.Singleton.Work.Nabywca.Clone();
            x.Sprzedawca = (FirmaData)SingleFakturaProperty.Singleton.Work.Sprzedawca.Clone();
            Property.Instance.Works.Add(x);
            Property.Instance.NameList = Property.Instance.NameList.Distinct().ToList();
            Property.Instance.StawkaList = Property.Instance.StawkaList.Distinct().ToList();
            Property.SerializeXml();

            SingleFakturaProperty.Singleton = null;
            SingleFakturaProperty.Singleton.Work = null;
            mw.FillValuesFaktura(null);
        }

        private static object GetIdFromTable(DataTable dt, string searchValue)
        {
            return dt.Rows.Cast<DataRow>().Where(w => w[1].ToString() == searchValue).FirstOrDefault().ItemArray[0];
        }
    }
}
