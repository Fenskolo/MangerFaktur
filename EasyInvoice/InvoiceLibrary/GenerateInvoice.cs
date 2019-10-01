using PdfFileWriter;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InvoiceLibrary
{
    public class GenerateInvoice
    {
        private readonly InvoiceData m_InvoiceData;
        private readonly PdfFont ArialNormal;
        private readonly PdfFont ArialBold;
        private readonly PdfPage Page;

        private readonly PdfDocument m_PdfDocument;
        private PdfContents PdfContents { get; set; }

        private const string ArialFontName = "Arial";

        public GenerateInvoice(string FileName, InvoiceData invoiceData)
        {
            m_InvoiceData = invoiceData;

            m_PdfDocument = new PdfDocument(PaperType.A4, false, UnitOfMeasure.cm, FileName)
            {
                Debug = false
            };

            ArialBold = PdfFont.CreatePdfFont(m_PdfDocument, ArialFontName, FontStyle.Bold, true);
            ArialNormal = PdfFont.CreatePdfFont(m_PdfDocument, ArialFontName, FontStyle.Regular, true);
            Page = new PdfPage(m_PdfDocument);
        }
        public void GetInvoice()
        {

            var Info = PdfInfo.CreatePdfInfo(m_PdfDocument);
            Info.Title("Faktura");
            Info.Author("TZ");
            Info.Keywords("keyword");
            Info.Subject("Temat");


            PdfContents = new PdfContents(Page);

            IdFaktury(m_InvoiceData.InvoiceNumber);

            double lastPosition = 26;

            CreateTable(m_InvoiceData.HeaderLeft, 1.3, 10, 8, lastPosition, lastPosition - 3, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(m_InvoiceData.HeaderRight, 10.3, 16, 8, lastPosition, lastPosition - 1.5, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(m_InvoiceData.GetSprzeNaby, 1.3, 30, 8, lastPosition - 1.4, lastPosition - 6.2, true, ContentAlignment.MiddleLeft);

            double WidthRow = ArialNormal.TextWidth(8, m_InvoiceData.NrBankowyLeftRight) + 0.25;

            lastPosition = CreateTable(m_InvoiceData.NrBankowy, 1.3, 1.3 + WidthRow, 8, lastPosition - 1, lastPosition - 6.2, false, ContentAlignment.MiddleLeft);

            lastPosition = TabelaDaneFaktura(1.3, lastPosition - 1, lastPosition - 11, 19.7, 9, m_InvoiceData.DataServices);

            RazemWTym(12.03, lastPosition - 0.018, lastPosition - 11, 9);

            lastPosition = Summary(12.03, lastPosition - 0.018, lastPosition - 11, 19.7, 9, m_InvoiceData.SummaryServiceValues);

            WidthRow = ArialNormal.TextWidth(8, m_InvoiceData.ZapDoZapLeftRight) + 0.25;
            CreateTable(m_InvoiceData.ZapDoZap, 1.3, 1.3 + WidthRow, 8, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleLeft);

            WidthRow = ArialNormal.TextWidth(12, m_InvoiceData.SummaryLeftRight);
            lastPosition = CreateTable(m_InvoiceData.Summary, 19.7 - WidthRow, 19.7, 12, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleRight);

            WidthRow = ArialNormal.TextWidth(8, m_InvoiceData.SummaryTextLeftRight);
            lastPosition = CreateTable(m_InvoiceData.SummaryText, 19.7 - WidthRow, 19.7, 8, lastPosition, lastPosition - 10, false, ContentAlignment.MiddleRight);

            RamkiEnd(1.3, lastPosition - 1.4, lastPosition - 5, 9, 7, DictionaryMain.labelPodpisWystawiania);

            RamkiEnd(12, lastPosition - 1.4, lastPosition - 5, 19.7, 7, DictionaryMain.labelPodpisOdbierania);

            m_PdfDocument.CreateFile();

            return;
        }


        private void IdFaktury(string nrFaktury)
        {
            PdfContents.SaveGraphicsState();

            PdfContents.Translate(1.3, 21.0);

            const double Width = 60;
            const double Height = 7;
            const double FontSize = 40;
            var txtF = new TextBox(Width, 0);
            var txtNr = new TextBox(Width, 0);

            txtF.AddText(ArialNormal, FontSize, DictionaryMain.labelNrFaktury);
            txtNr.AddText(ArialBold, FontSize, nrFaktury);

            double PosY = Height;
            PdfContents.DrawText(0.0, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtF);
            PosY = Height;
            double sizeLabel = ArialNormal.TextWidth(FontSize, DictionaryMain.labelNrFaktury) + 1;
            PdfContents.DrawText(sizeLabel, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtNr);

            PdfContents.RestoreGraphicsState();
        }

        private double CreateTable(List<TableData> dt, double left, double right, double fontSize, double top, double bottom, bool withHeader, ContentAlignment ca)
        {
            double LastRowPosition;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;

            var firstElement = dt.FirstOrDefault();
            double colWidthTitle = ArialNormal.TextWidth(fontSize, firstElement.LeftSide) + 2.0 * MARGIN_HOR;
            double colWidthDetail = ArialNormal.TextWidth(fontSize, firstElement.RightSide) + 2.0 * MARGIN_HOR;

            var Table = new PdfTable(Page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left, bottom, right, top)
            };
            var array = new double[] { colWidthTitle, colWidthDetail };
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();

            var Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.White;
            Table.DefaultHeaderStyle.Alignment = ca;


            Table.DefaultCellStyle.Margin = Margin;
            if (withHeader)
            {
                Table.Header[0].Style.FontSize = 12;
                Table.Header[0].Style.Font = ArialBold;
                Table.Header[0].Value = firstElement.LeftSide;
                Table.Header[1].Value = firstElement.RightSide;
                Table.Header[0].Style.FontSize = 12;
                Table.Header[0].Style.Font = ArialBold;
                Table.Header[1].Style.FontSize = 12;
                Table.Header[1].Style.Font = ArialBold;
            }

            int i = 0;
            foreach (var item in dt)
            {
                if (withHeader && i == 0)
                {
                    i++;
                    continue;
                }
                Table.Cell[0].Value = item.LeftSide;
                Table.Cell[1].Value = item.RightSide;
                Table.DrawRow();
            }

            LastRowPosition = Table.RowPosition[Table.RowNumber];

            Table.Close();

            PdfContents.SaveGraphicsState();

            PdfContents.RestoreGraphicsState();
            return LastRowPosition;
        }

        private double TabelaDaneFaktura(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, IEnumerable<DataServices> dataServices)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(Page, PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            var array = new double[] { 1, 9.5, 1.5, 2.5, 2.5, 3.5, 3, 3, 3.5 };
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);


            var Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = ArialBold;
            Table.DefaultHeaderStyle.MultiLineText = true;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            Table.Header[0].Value = DictionaryMain.kolumnaLp;
            Table.Header[1].Value = DictionaryMain.kolumnaTowar;
            Table.Header[2].Value = DictionaryMain.kolumnaJM;
            Table.Header[3].Value = DictionaryMain.kolumnaIlosc;
            Table.Header[4].Value = DictionaryMain.kolumnaCenaNetto;
            Table.Header[5].Value = DictionaryMain.kolumnaWartoscNetto;
            Table.Header[6].Value = DictionaryMain.kolumnaStawkaVat;
            Table.Header[7].Value = DictionaryMain.kolumnaKwotaVat;
            Table.Header[8].Value = DictionaryMain.kolumnaWartoscBrutto;

            Table.DefaultCellStyle.Margin = Margin;

            for (int i = 0; i < array.Length; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            foreach (var item in dataServices)
            {
                Table.Cell[0].Value = item.RecordId;
                Table.Cell[1].Value = item.CaptionRecord;
                Table.Cell[2].Value = item.KindAmount;
                Table.Cell[3].Value = item.Amount;
                Table.Cell[4].Value = ToCurrency(item.AmountNetto);

                Table.Cell[5].Value = ToCurrency(item.ValueNetto);
                Table.Cell[6].Value = item.VatRate;
                Table.Cell[7].Value = ToCurrency(item.ValueVat);
                Table.Cell[8].Value = ToCurrency(item.ValueBrutto);
                Table.DrawRow();
            }

            positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight; ;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double RamkiEnd(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, string tekst)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(Page, PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Table.SetColumnWidth(new double[] { 12 });

            Table.Borders.ClearAllBorders();
            Table.Borders.SetFrame(FRAME_WIDTH);

            var Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);

            Table.DefaultCellStyle.Margin = Margin;

            Table.Cell[0].Style = Table.CellStyle;
            Table.Cell[0].Style.MultiLineText = false;
            Table.Cell[0].Style.Alignment = ContentAlignment.MiddleCenter;
            Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;

            Table.Cell[0].Value = string.Empty;
            Table.DrawRow();
            Table.Cell[0].Value = string.Empty;
            Table.DrawRow();
            Table.Cell[0].Value = string.Empty;
            Table.DrawRow();
            Table.Cell[0].Value = string.Empty;
            Table.DrawRow();
            Table.Cell[0].Value = tekst;
            Table.DrawRow();

            positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight; ;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double Summary(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, IEnumerable<DataServices> daneUslugas)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;


            var Table = new PdfTable(Page, PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 0.31, BOTTOM, RIGHT, TOP)
            };
            var array = new double[] { 3.5, 3, 3, 3.5 };
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);


            var Margin = new PdfRectangle(MARGIN_HOR + 0.27, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = ArialBold;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            var firstService = daneUslugas.FirstOrDefault();

            Table.Header[0].Value = ToCurrency(firstService.ValueNetto);
            Table.Header[1].Value = firstService.VatRate;
            Table.Header[2].Value = ToCurrency(firstService.ValueVat);
            Table.Header[3].Value = ToCurrency(firstService.ValueBrutto);

            Table.DefaultCellStyle.Margin = Margin;

            for (int i = 0; i < array.Length; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            int z = 0;
            foreach (var item in daneUslugas)
            {
                if (z == 0)
                {
                    z++;
                    continue;
                }
                Table.Cell[0].Value = ToCurrency(item.ValueNetto);
                Table.Cell[1].Value = item.VatRate;
                Table.Cell[2].Value = ToCurrency(item.ValueVat);
                Table.Cell[3].Value = ToCurrency(item.ValueBrutto);
                Table.DrawRow();
            }

            positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight; ;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private void RazemWTym(double LEFT, double TOP, double BOTTOM, double FONT_SIZE)
        {
            var TableLeft = new PdfTable(Page, PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 3, BOTTOM, LEFT - 0.5, TOP)
            };

            TableLeft.SetColumnWidth(new double[] { 2.5 });
            TableLeft.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;
            TableLeft.DefaultHeaderStyle.BackgroundColor = Color.Transparent;
            TableLeft.Header[0].Value = DictionaryMain.summaRazem;
            TableLeft.Cell[0].Style.Alignment = ContentAlignment.MiddleRight;
            TableLeft.Cell[0].Value = DictionaryMain.summaWTym;
            TableLeft.DrawRow();
            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
        }

        public static string ToCurrency(decimal ob, bool pln = false)
        {
            return ob.ToString("C").Replace(" zł", pln ? " PLN" : string.Empty);
        }
    }
}
