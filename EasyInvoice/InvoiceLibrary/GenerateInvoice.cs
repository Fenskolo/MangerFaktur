using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PdfFileWriter;

namespace InvoiceLibrary
{
    public class GenerateInvoice
    {
        private const string ArialFontName = "Arial";
        private readonly PdfFont _arialBold;
        public readonly PdfFont ArialNormal;
        private readonly InvoiceData _mInvoiceData;

        public readonly PdfDocument MPdfDocument;
        private readonly PdfPage _page;

        public GenerateInvoice(string fileName, InvoiceData invoiceData)
        {
            _mInvoiceData = invoiceData;

            MPdfDocument = new PdfDocument(PaperType.A4, false, UnitOfMeasure.cm, fileName)
            {
                Debug = false
            };

            _arialBold = PdfFont.CreatePdfFont(MPdfDocument, ArialFontName, FontStyle.Bold);
            ArialNormal = PdfFont.CreatePdfFont(MPdfDocument, ArialFontName, FontStyle.Regular);
            _page = new PdfPage(MPdfDocument);
        }

        private PdfContents PdfContents { get; set; }

        public void GetInvoice()
        {
            var info = PdfInfo.CreatePdfInfo(MPdfDocument);
            info.Title("Faktura");
            info.Author("TZ");
            info.Keywords("keyword");
            info.Subject("Temat");


            PdfContents = new PdfContents(_page);

            IdFaktury(_mInvoiceData.InvoiceNumber);

            double lastPosition = 26;

            CreateTable(_mInvoiceData.HeaderLeft, 1.3, 10, 8, lastPosition, lastPosition - 3, false,
                ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(_mInvoiceData.HeaderRight, 10.3, 16, 8, lastPosition, lastPosition - 1.5, false,
                ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(_mInvoiceData.GetSprzeNaby, 1.3, 30, 8, lastPosition - 1.4, lastPosition - 6.2,
                true, ContentAlignment.MiddleLeft);

            var widthRow = ArialNormal.TextWidth(8, _mInvoiceData.NrBankowyLeftRight) + 0.25;

            lastPosition = CreateTable(_mInvoiceData.NrBankowy, 1.3, 1.3 + widthRow, 8, lastPosition - 1,
                lastPosition - 6.2, false, ContentAlignment.MiddleLeft);

            lastPosition = TabelaDaneFaktura(1.3, lastPosition - 1, lastPosition - 11, 19.7, 9,
                _mInvoiceData.DataServices);

            RazemWTym(12.03, lastPosition - 0.018, lastPosition - 11, 9);

            lastPosition = Summary(12.03, lastPosition - 0.018, lastPosition - 11, 19.7, 9,
                _mInvoiceData.SummaryServiceValues);

            widthRow = ArialNormal.TextWidth(8, _mInvoiceData.ZapDoZapLeftRight) + 0.25;
            CreateTable(_mInvoiceData.ZapDoZap, 1.3, 1.3 + widthRow, 8, lastPosition - 1, lastPosition - 10, false,
                ContentAlignment.MiddleLeft);

            widthRow = ArialNormal.TextWidth(12, _mInvoiceData.SummaryLeftRight);
            lastPosition = CreateTable(_mInvoiceData.Summary, 19.7 - widthRow, 19.7, 12, lastPosition - 1,
                lastPosition - 10, false, ContentAlignment.MiddleRight);

            widthRow = ArialNormal.TextWidth(8, _mInvoiceData.SummaryTextLeftRight);
            lastPosition = CreateTable(_mInvoiceData.SummaryText, 19.7 - widthRow, 19.7, 8, lastPosition,
                lastPosition - 10, false, ContentAlignment.MiddleRight);

            RamkiEnd(1.3, lastPosition - 1.4, lastPosition - 5, 9, 7, DictionaryMain.LabelPodpisWystawiania);

            RamkiEnd(12, lastPosition - 1.4, lastPosition - 5, 19.7, 7, DictionaryMain.LabelPodpisOdbierania);

            MPdfDocument.CreateFile();
        }


        private void IdFaktury(string nrFaktury)
        {
            PdfContents.SaveGraphicsState();

            PdfContents.Translate(1.3, 21.0);

            const double width = 60;
            const double height = 7;
            const double fontSize = 40;
            var txtF = new TextBox(width);
            var txtNr = new TextBox(width);

            txtF.AddText(ArialNormal, fontSize, DictionaryMain.LabelNrFaktury);
            txtNr.AddText(_arialBold, fontSize, nrFaktury);

            var posY = height;
            PdfContents.DrawText(0.0, ref posY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtF);
            posY = height;
            var sizeLabel = ArialNormal.TextWidth(fontSize, DictionaryMain.LabelNrFaktury) + 1;
            PdfContents.DrawText(sizeLabel, ref posY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtNr);

            PdfContents.RestoreGraphicsState();
        }

        private double CreateTable(List<TableData> dt, double left, double right, double fontSize, double top,
            double bottom, bool withHeader, ContentAlignment ca)
        {
            const double marginHor = 0.04;
            const double marginVer = 0.04;

            var firstElement = dt.FirstOrDefault();
            var colWidthTitle = ArialNormal.TextWidth(fontSize, firstElement.LeftSide) + 2.0 * marginHor;
            var colWidthDetail = ArialNormal.TextWidth(fontSize, firstElement.RightSide) + 2.0 * marginHor;

            var table = new PdfTable(_page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left, bottom, right, top)
            };
            var array = new[] {colWidthTitle, colWidthDetail};
            table.SetColumnWidth(array);

            table.Borders.ClearAllBorders();

            var margin = new PdfRectangle(marginHor, marginVer);

            table.DefaultHeaderStyle.Margin = margin;
            table.DefaultHeaderStyle.BackgroundColor = Color.White;
            table.DefaultHeaderStyle.Alignment = ca;


            table.DefaultCellStyle.Margin = margin;
            if (withHeader)
            {
                table.Header[0].Style.FontSize = 12;
                table.Header[0].Style.Font = _arialBold;
                table.Header[0].Value = firstElement.LeftSide;
                table.Header[1].Value = firstElement.RightSide;
                table.Header[0].Style.FontSize = 12;
                table.Header[0].Style.Font = _arialBold;
                table.Header[1].Style.FontSize = 12;
                table.Header[1].Style.Font = _arialBold;
            }

            var i = 0;
            foreach (var item in dt)
            {
                if (withHeader && i == 0)
                {
                    i++;
                    continue;
                }

                table.Cell[0].Value = item.LeftSide;
                table.Cell[1].Value = item.RightSide;
                table.DrawRow();
            }

            var lastRowPosition = table.RowPosition[table.RowNumber];

            table.Close();

            PdfContents.SaveGraphicsState();

            PdfContents.RestoreGraphicsState();
            return lastRowPosition;
        }

        private double TabelaDaneFaktura(double left, double top, double bottom, double right, double fontSize,
            IEnumerable<DataServices> dataServices)
        {
            const double marginHor = 0.04;
            const double marginVer = 0.04;
            const double frameWidth = 0.015;

            var table = new PdfTable(_page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left, bottom, right, top)
            };
            var array = new[] {1, 9.5, 1.5, 2.5, 2.5, 3.5, 3, 3, 3.5};
            table.SetColumnWidth(array);

            table.Borders.ClearAllBorders();
            table.Borders.SetAllBorders(frameWidth, frameWidth);


            var margin = new PdfRectangle(marginHor, marginVer);

            table.DefaultHeaderStyle.Margin = margin;
            table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            table.DefaultHeaderStyle.Font = _arialBold;
            table.DefaultHeaderStyle.MultiLineText = true;
            table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            table.Header[0].Value = DictionaryMain.KolumnaLp;
            table.Header[1].Value = DictionaryMain.KolumnaTowar;
            table.Header[2].Value = DictionaryMain.KolumnaJm;
            table.Header[3].Value = DictionaryMain.KolumnaIlosc;
            table.Header[4].Value = DictionaryMain.KolumnaCenaNetto;
            table.Header[5].Value = DictionaryMain.KolumnaWartoscNetto;
            table.Header[6].Value = DictionaryMain.KolumnaStawkaVat;
            table.Header[7].Value = DictionaryMain.KolumnaKwotaVat;
            table.Header[8].Value = DictionaryMain.KolumnaWartoscBrutto;

            table.DefaultCellStyle.Margin = margin;

            for (var i = 0; i < array.Length; i++)
            {
                table.Cell[i].Style = table.CellStyle;
                table.Cell[i].Style.MultiLineText = false;
                table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            foreach (var item in dataServices)
            {
                table.Cell[0].Value = item.RecordId;
                table.Cell[1].Value = item.CaptionRecord;
                table.Cell[2].Value = item.KindAmount;
                table.Cell[3].Value = item.Amount;
                table.Cell[4].Value = ToCurrency(item.AmountNetto);

                table.Cell[5].Value = ToCurrency(item.ValueNetto);
                table.Cell[6].Value = item.VatRate;
                table.Cell[7].Value = ToCurrency(item.ValueVat);
                table.Cell[8].Value = ToCurrency(item.ValueBrutto);
                table.DrawRow();
            }

            var positionLast = table.RowPosition[table.RowNumber] - table.RowHeight;

            table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double RamkiEnd(double left, double top, double bottom, double right, double fontSize, string tekst)
        {
            const double marginHor = 0.04;
            const double marginVer = 0.04;
            const double frameWidth = 0.015;

            var table = new PdfTable(_page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left, bottom, right, top)
            };
            table.SetColumnWidth(12);

            table.Borders.ClearAllBorders();
            table.Borders.SetFrame(frameWidth);

            var margin = new PdfRectangle(marginHor, marginVer);

            table.DefaultCellStyle.Margin = margin;

            table.Cell[0].Style = table.CellStyle;
            table.Cell[0].Style.MultiLineText = false;
            table.Cell[0].Style.Alignment = ContentAlignment.MiddleCenter;
            table.CellStyle.TextDrawStyle = DrawStyle.Superscript;

            table.Cell[0].Value = string.Empty;
            table.DrawRow();
            table.Cell[0].Value = string.Empty;
            table.DrawRow();
            table.Cell[0].Value = string.Empty;
            table.DrawRow();
            table.Cell[0].Value = string.Empty;
            table.DrawRow();
            table.Cell[0].Value = tekst;
            table.DrawRow();

            var positionLast = table.RowPosition[table.RowNumber] - table.RowHeight;

            table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double Summary(double left, double top, double bottom, double right, double fontSize,
            IEnumerable<DataServices> daneUslugas)
        {
            const double marginHor = 0.04;
            const double marginVer = 0.04;
            const double frameWidth = 0.015;


            var table = new PdfTable(_page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left - 0.31, bottom, right, top)
            };
            var array = new[] {3.5, 3, 3, 3.5};
            table.SetColumnWidth(array);

            table.Borders.ClearAllBorders();
            table.Borders.SetAllBorders(frameWidth, frameWidth);


            var margin = new PdfRectangle(marginHor + 0.27, marginVer);

            table.DefaultHeaderStyle.Margin = margin;
            table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            table.DefaultHeaderStyle.Font = _arialBold;
            table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            var dataServiceses = daneUslugas.ToList();
            var firstService = dataServiceses.FirstOrDefault();

            if (firstService != null)
            {
                table.Header[0].Value = ToCurrency(firstService.ValueNetto);
                table.Header[1].Value = firstService.VatRate;
                table.Header[2].Value = ToCurrency(firstService.ValueVat);
                table.Header[3].Value = ToCurrency(firstService.ValueBrutto);
            }

            table.DefaultCellStyle.Margin = margin;

            for (var i = 0; i < array.Length; i++)
            {
                table.Cell[i].Style = table.CellStyle;
                table.Cell[i].Style.MultiLineText = false;
                table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            var z = 0;
            foreach (var item in dataServiceses)
            {
                if (z == 0)
                {
                    z++;
                    continue;
                }

                table.Cell[0].Value = ToCurrency(item.ValueNetto);
                table.Cell[1].Value = item.VatRate;
                table.Cell[2].Value = ToCurrency(item.ValueVat);
                table.Cell[3].Value = ToCurrency(item.ValueBrutto);
                table.DrawRow();
            }

            var positionLast = table.RowPosition[table.RowNumber] - table.RowHeight;

            table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private void RazemWTym(double left, double top, double bottom, double fontSize)
        {
            var tableLeft = new PdfTable(_page, PdfContents, ArialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left - 3, bottom, left - 0.5, top)
            };

            tableLeft.SetColumnWidth(2.5);
            tableLeft.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;
            tableLeft.DefaultHeaderStyle.BackgroundColor = Color.Transparent;
            tableLeft.Header[0].Value = DictionaryMain.SummaRazem;
            tableLeft.Cell[0].Style.Alignment = ContentAlignment.MiddleRight;
            tableLeft.Cell[0].Value = DictionaryMain.SummaWTym;
            tableLeft.DrawRow();
            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
        }

        public static string ToCurrency(decimal ob, bool pln = false)
        {
            return ob.ToString("C").Replace(" zł", pln ? " PLN" : string.Empty);
        }
    }
}