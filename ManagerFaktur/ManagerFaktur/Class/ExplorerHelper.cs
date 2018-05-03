using Infragistics.Win.UltraWinListView;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ManagerFaktur
{
    class ExplorerHelper
    {
        private UltraListView ulv;
        private Infragistics.Win.Appearance appPdf;
        Infragistics.Win.Appearance appWord;

        public ExplorerHelper(UltraListView _ulv)
        {
            ulv = _ulv;
            appPdf = ulv.Appearances.Add("pdf");
            appPdf.Image = Properties.Resources.pdf;
            appWord = ulv.Appearances.Add("Word");
            appWord.Image = Properties.Resources.word;
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
                        string tekst = ExtractTextFromPdf(fileInfo.FullName).Trim();

                        FindOkresAndSymbol(tekst, out string symbol, out DateTime? okresD);

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
                if (x.Td == TypDanych.containsSymbol && tekst.Contains(x.FirstString))
                {
                    symbol = x.LastString;
                }
                else if (x.Td == TypDanych.symbolOdDo && !string.IsNullOrEmpty(getBetween(tekst, x.FirstString, x.LastString)))
                {
                    symbol = getBetween(tekst, x.FirstString, x.LastString);
                }

                if (x.Td == TypDanych.okresOdDo && !string.IsNullOrEmpty(getBetween(tekst, x.FirstString, x.LastString)))
                {
                    okres = getBetween(tekst, x.FirstString, x.LastString);
                }
            }

            if (!string.IsNullOrEmpty(okres))
            {
                if (okres.Contains("M"))
                {
                    okresD = BuildDate(okres.Split('M')[0], okres.Split('M')[1], null);
                }
                else if (okres.Contains("-") && okres[2] == '-' && okres[5] == '-' && okres.Length == 10)
                {
                    okresD = BuildDate(okres.Substring(6, 4), okres.Substring(3, 2), okres.Substring(0, 2));
                }
                else if (okres.Length == 10)
                {
                    if (Settings.Instance.ListSDelOneMonth.Contains(symbol))
                    {
                        okresD = Convert.ToDateTime(okres).AddMonths(-1);
                    }
                    else
                    {
                        okresD = Convert.ToDateTime(okres);
                    }
                }
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
                string t1 = string.Empty;

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    TextWithFontExtractionStategy s = new TextWithFontExtractionStategy();
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i, s));
                }
                text.Replace(" ", "").Replace("\r\n","");
                return text.ToString();
            }
        }
    }

    public class TextWithFontExtractionStategy : ITextExtractionStrategy
    {
        private StringBuilder result = new StringBuilder();
        
        private Vector lastBaseLine;
        private string lastFont;
        private float lastFontSize;
        
        private enum TextRenderMode
        {
            FillText = 0,
            StrokeText = 1,
            FillThenStrokeText = 2,
            Invisible = 3,
            FillTextAndAddToPathForClipping = 4,
            StrokeTextAndAddToPathForClipping = 5,
            FillThenStrokeTextAndAddToPathForClipping = 6,
            AddTextToPaddForClipping = 7
        }
        
        public void RenderText(TextRenderInfo renderInfo)
        {
            string curFont = renderInfo.GetFont().PostscriptFontName;
            //Check if faux bold is used
            if ((renderInfo.GetTextRenderMode() == (int)TextRenderMode.FillThenStrokeText))
            {
                curFont += "-Bold";
            }

            //This code assumes that if the baseline changes then we're on a newline
            Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
            Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(curBaseline[Vector.I1], curBaseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
            Single curFontSize = rect.Height;

            //See if something has changed, either the baseline, the font or the font size
            if ((this.lastBaseLine == null) || (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]) || (curFontSize != lastFontSize) || (curFont != lastFont))
            {
                //if we've put down at least one span tag close it
                if ((this.lastBaseLine != null))
                {
                    this.result.AppendLine("</span>");
                }
                //If the baseline has changed then insert a line break
                if ((this.lastBaseLine != null) && curBaseline[Vector.I2] != lastBaseLine[Vector.I2])
                {
                    this.result.AppendLine("<br />");
                }
                //Create an HTML tag with appropriate styles
                this.result.AppendFormat("<span>");// style=\"font-family:{0};font-size:{1}\">", curFont, curFontSize);
            }

            //Append the current text
            this.result.Append(renderInfo.GetText());

            //Set currently used properties
            this.lastBaseLine = curBaseline;
            this.lastFontSize = curFontSize;
            this.lastFont = curFont;
        }

        public string GetResultantText()
        {
            //If we wrote anything then we'll always have a missing closing tag so close it here
            if (result.Length > 0)
            {
                result.Append("</span>");
            }
            return result.ToString();
        }

        //Not needed
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) { }
    }
}

