using Infragistics.Win.UltraWinListView;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerFaktur
{
    class ExplorerHelper
    {
        private UltraListView ulv;

        public ExplorerHelper(UltraListView _ulv)
        {
            ulv = _ulv;
        }

        public void LoadExplorer()
        {
            try
            {
                if (!Directory.Exists(Settings.Instance.DefWorkPath))
                {
                    return;
                }

                DirectoryInfo cDriveInfo = new DirectoryInfo(Settings.Instance.DefWorkPath);

                //Infragistics.Win.Appearance appFolder = this.uListView.Appearances.Add("folder");
                //appFolder.Image = Properties.Resources.folder;
                //DirectoryInfo[] directories = cDriveInfo.GetDirectories();
                //for (int i = 0; i < directories.Length; i++)
                //{
                //    DirectoryInfo directoryInfo = directories[i];

                //    UltraListViewItem item = this.uListView.Items.Add(directoryInfo.FullName, directoryInfo.Name);
                //    item.SubItems["FileType"].Value = "File Folder";
                //    item.SubItems["DateModified"].Value = directoryInfo.LastWriteTime;
                //    item.Appearance = this.uListView.Appearances["folder"];
                //}

                Infragistics.Win.Appearance appPdf = ulv.Appearances.Add("pdf");
                appPdf.Image = Properties.Resources.pdf;
                Infragistics.Win.Appearance appWord = ulv.Appearances.Add("Word");
                appWord.Image = Properties.Resources.word;

                FileInfo[] files = cDriveInfo.GetFiles("*.*", Settings.Instance.SearchO);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fileInfo = files[i];

                    if (!Settings.Instance.ListExtenstion.Contains(fileInfo.Extension.ToUpper()))
                    {
                        continue;
                    }

                    UltraListViewItem item = ulv.Items.Add(fileInfo.FullName, fileInfo.Name);
                    item.SubItems["FileSize"].Value = fileInfo.Length / 1024;
                    item.SubItems["FileType"].Value = "File";
                    item.SubItems["DateModified"].Value = fileInfo.LastWriteTime;


                    if (fileInfo.Extension.ToUpper() == ".PDF")
                    {
                        item.Appearance = appPdf;
                        string tekst = ExtractTextFromPdf(fileInfo.FullName);

                        string okres = getBetween(tekst, "\nOkresrozliczenia:", "(M-MIESIĄC)");
                        DateTime? okresD = null;
                        string symbol = getBetween(tekst, "\nSymbol:", "\nIdentyfikacjazobowiązania");

                        if (string.IsNullOrEmpty(symbol))
                        {
                            if (tekst.Contains("PRZELEWZEWNĘTRZNYDOZUS"))
                            {
                                symbol = "ZUS";
                            }
                            else if (tekst.Contains("BIURORACHUNKOWEPERFEKTS.C."))
                            {
                                symbol = "Perfekt";
                            }
                            else if (tekst.Contains("AWP POLSKA SP Z O.O."))
                            {
                                symbol = "AWP";

                                okres = getBetween(tekst, "\nData sprzeda|y: ", "\nSprzedawca:");
                                if(string.IsNullOrEmpty(okres))
                                {
                                    okres = getBetween(tekst, "\nData sprzeda|y: ", "\nSPRZEDAWCA:");
                                }
                                if (okres.Length == 10)
                                {
                                    okresD = BuildDate(okres.Substring(6, 4), okres?.Substring(3, 2), okres.Substring(0, 2));
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(okres))
                        {
                            okres = getBetween(tekst, "Dataoperacji:", "\nDataksięgowania");
                            okresD = string.IsNullOrEmpty(okres) ? new DateTime() : Convert.ToDateTime(okres).AddMonths(-1);
                        }
                        else if (!okresD.HasValue)
                        {
                            okresD = BuildDate(okres.Split('M')[0], okres.Split('M')[1], null);
                        }


                        item.SubItems["Okres"].Value = okresD;
                        item.SubItems["Symbol"].Value = symbol;

                    }
                    else
                    {
                        item.Appearance = appWord;
                    }
                }

                ulv.View = UltraListViewStyle.Details;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }



        private DateTime BuildDate(string yearS, string monthS, string dayS)
        {
            if (string.IsNullOrEmpty(yearS)|| string.IsNullOrEmpty(monthS))
            {
                return new DateTime();
            }

            int year = yearS.Length==2? Convert.ToInt32(yearS)+2000: Convert.ToInt32(yearS);
            int month = Convert.ToInt32(monthS);
            int day = !string.IsNullOrEmpty(dayS)? Convert.ToInt32(dayS) : 1;

            return new DateTime(year, month, day);
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {                
                return "";
            }
        }

        public string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }
    }
}
