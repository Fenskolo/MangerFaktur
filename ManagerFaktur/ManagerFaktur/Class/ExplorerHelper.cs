using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Appearance = Infragistics.Win.Appearance;
using Resources = ManagerFaktur.Properties.Resources;

namespace ManagerFaktur
{
    internal class ExplorerHelper
    {
        private readonly Appearance appPdf;
        private readonly Appearance appWord;
        private readonly UltraListView ulv;

        public ExplorerHelper(UltraListView _ulv)
        {
            ulv = _ulv;
            appPdf = ulv.Appearances.Add("pdf");
            appPdf.Image = Resources.pdf;
            appWord = ulv.Appearances.Add("Word");
            appWord.Image = Resources.word;
        }

        public void LoadExplorer(string Path)
        {
            try
            {
                if (!Directory.Exists(Settings.Instance.WorkPath))
                {
                    return;
                }

                var cDriveInfo = new DirectoryInfo(Path);

                var files = cDriveInfo.GetFiles("*.*", Settings.Instance.SearchOptions);
                for (var i = 0; i < files.Length; i++)
                {
                    var fileInfo = files[i];

                    if (!Settings.Instance.ListExtenstion.Contains(fileInfo.Extension.ToUpper()))
                    {
                        continue;
                    }

                    var item = ulv.Items.Add(fileInfo.FullName, fileInfo.Name);
                    item.SubItems["FileSize"].Value = fileInfo.Length / 1024;
                    item.SubItems["FileType"].Value = "File";
                    item.SubItems["DateModified"].Value = fileInfo.LastWriteTime;


                    if (fileInfo.Extension.ToUpper() == ".PDF")
                    {
                        item.Appearance = appPdf;
                        var tekst = ExtractTextFromPdf(fileInfo.FullName).Trim();

                        FindOkresAndSymbol(tekst, out var symbol, out var okresD);

                        okresD = okresD ?? new DateTime();

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

        private void FindOkresAndSymbol(string tekst, out string symbol, out DateTime? okresD)
        {
            symbol = null;
            string okres = null;
            okresD = null;

            foreach (Symbol x in Settings.Instance.SymboleOkres)
            {
                switch (x.Td)
                {
                    case TypDanych.containsSymbol when tekst.Contains(x.FirstString):
                        symbol = x.LastString;
                        break;
                    case TypDanych.symbolOdDo when !string.IsNullOrEmpty(GetBetween(tekst, x.FirstString, x.LastString)):
                        symbol = GetBetween(tekst, x.FirstString, x.LastString);
                        break;
                    case TypDanych.okresOdDo when !string.IsNullOrEmpty(GetBetween(tekst, x.FirstString, x.LastString)):
                        okres = GetBetween(tekst, x.FirstString, x.LastString);
                        break;
                }
            }

            if (string.IsNullOrEmpty(okres))
            {
                return;
            }

            if (okres.Contains("M"))
            {
                okresD = BuildDate(okres.Split('M')[0], okres.Split('M')[1], null);
            }
            else if (okres.Contains("-") && okres[2] == '-' && okres[5] == '-' || okres.Contains(".") &&
                     okres[2] == '.' && okres[5] == '.' && okres.Length == 10)
            {
                okresD = BuildDate(okres.Substring(6, 4), okres.Substring(3, 2), okres.Substring(0, 2));
            }
            else if (okres.Length == 10)
            {
                okresD = Settings.Instance.ListSDelOneMonth.Contains(symbol) ? Convert.ToDateTime(okres).AddMonths(-1) : Convert.ToDateTime(okres);
            }
        }

        private DateTime BuildDate(string yearS, string monthS, string dayS)
        {
            if (string.IsNullOrEmpty(yearS) || string.IsNullOrEmpty(monthS))
            {
                return new DateTime();
            }

            var year = yearS.Length == 2 ? Convert.ToInt32(yearS) + 2000 : Convert.ToInt32(yearS);
            var month = Convert.ToInt32(monthS);
            var day = !string.IsNullOrEmpty(dayS) ? Convert.ToInt32(dayS) : 1;

            return new DateTime(year, month, day);
        }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public string ExtractTextFromPdf(string path)
        {
            using (var reader = new PdfReader(path))
            {
                var text = new StringBuilder();
                var t1 = string.Empty;

                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    var s = new TextWithFontExtractionStategy();
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, s));
                }

                text.Replace(" ", "").Replace("\r\n", "");
                return text.ToString();
            }
        }
    }
}