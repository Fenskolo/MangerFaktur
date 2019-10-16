using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PdfFileWriter;

namespace EasyInvoice
{
    public class GenerujPolaWDokumencie
    {
        private const string ArialFontName = "Arial";
        private readonly PdfFont _arialBold;
        private readonly PdfFont _arialNormal;
        private readonly PdfPage _page;

        public GenerujPolaWDokumencie(string FileName, SingleFakturaProperty singleFaktura)
        {
            IHelperData mHelperData = new HelperData(singleFaktura);
            IDaneNaglowek mDaneNaglowka = new DaneNaglowek(singleFaktura.Work.Naglowek);

            PdfDocument = new PdfDocument(PaperType.A4, false, UnitOfMeasure.cm, FileName)
            {
                Debug = false
            };

            _arialBold = PdfFont.CreatePdfFont(PdfDocument, ArialFontName, FontStyle.Bold);
            _arialNormal = PdfFont.CreatePdfFont(PdfDocument, ArialFontName, FontStyle.Regular);

            var Info = PdfInfo.CreatePdfInfo(PdfDocument);
            Info.Title("Faktura");
            Info.Author("TZ");
            Info.Keywords("keyword");
            Info.Subject("Temat");

            _page = new PdfPage(PdfDocument);
            PdfContents = new PdfContents(_page);

            IdFaktury(singleFaktura.Work.Naglowek.NumerFaktury);

            double lastPosition = 26;

            CreateTable(mDaneNaglowka.GetNaglowekL(), 1.3, 10, 8, lastPosition, lastPosition - 3, false,
                ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(mDaneNaglowka.GetNaglowekR(), 10.3, 16, 8, lastPosition, lastPosition - 1.5,
                false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(mHelperData.GetSprzeNaby(), 1.3, 30, 8, lastPosition - 1.4, lastPosition - 6.2,
                true, ContentAlignment.MiddleLeft);

            var widthRow =
                _arialNormal.TextWidth(8, mHelperData.NrBankowy()[0].Lewa + mHelperData.NrBankowy()[0].Prawa) + 0.25;

            lastPosition = CreateTable(mHelperData.NrBankowy(), 1.3, 1.3 + widthRow, 8, lastPosition - 1,
                lastPosition - 6.2, false, ContentAlignment.MiddleLeft);

            lastPosition = TabelaDaneFaktura(1.3, lastPosition - 1, lastPosition - 11, 19.7, 9,
                singleFaktura.GetListDt());

            RazemWTym(12.03, lastPosition - 0.018, lastPosition - 11, 9);

            lastPosition = Summary(12.03, lastPosition - 0.018, lastPosition - 11, 19.7, 9, singleFaktura.GetSum());

            widthRow = _arialNormal.TextWidth(8,
                           mHelperData.GetZapDoZap()[1].Lewa + mHelperData.GetZapDoZap()[1].Prawa) + 0.25;
            CreateTable(mHelperData.GetZapDoZap(), 1.3, 1.3 + widthRow, 8, lastPosition - 1, lastPosition - 10, false,
                ContentAlignment.MiddleLeft);

            widthRow = _arialNormal.TextWidth(12, mHelperData.Razem()[0].Lewa + mHelperData.Razem()[0].Prawa);
            lastPosition = CreateTable(mHelperData.Razem(), 19.7 - widthRow, 19.7, 12, lastPosition - 1,
                lastPosition - 10, false, ContentAlignment.MiddleRight);

            widthRow = _arialNormal.TextWidth(8, mHelperData.RazemSlownie().Lewa + mHelperData.RazemSlownie().Prawa);
            lastPosition = CreateTable(new List<DaneTabela> {mHelperData.RazemSlownie()}, 19.7 - widthRow, 19.7, 8,
                lastPosition, lastPosition - 10, false, ContentAlignment.MiddleRight);

            RamkiEnd(1.3, lastPosition - 1.4, lastPosition - 5, 9, 7, DictionaryMain.LabelPodpisWystawiania);

            RamkiEnd(12, lastPosition - 1.4, lastPosition - 5, 19.7, 7, DictionaryMain.LabelPodpisOdbierania);

            PdfDocument.CreateFile();
        }

        private PdfDocument PdfDocument { get; }
        private PdfContents PdfContents { get; }


        private void IdFaktury(string nrFaktury)
        {
            PdfContents.SaveGraphicsState();

            PdfContents.Translate(1.3, 21.0);

            const double width = 60;
            const double height = 7;
            const double fontSize = 40;
            var txtF = new TextBox(width);
            var txtNr = new TextBox(width);

            txtF.AddText(_arialNormal, fontSize, DictionaryMain.LabelNrFaktury);
            txtNr.AddText(_arialBold, fontSize, nrFaktury);

            var posY = height;
            PdfContents.DrawText(0.0, ref posY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtF);
            posY = height;
            var sizeLabel = _arialNormal.TextWidth(fontSize, DictionaryMain.LabelNrFaktury) + 1;
            PdfContents.DrawText(sizeLabel, ref posY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtNr);

            PdfContents.RestoreGraphicsState();
        }

        private double CreateTable(List<DaneTabela> dt, double left, double right, double fontSize, double top,
            double bottom, bool withHeader, ContentAlignment ca)
        {
            const double marginHor = 0.04;
            const double marginVer = 0.04;

            var colWidthTitle = _arialNormal.TextWidth(fontSize, dt.First().Lewa) + 2.0 * marginHor;
            var colWidthDetail = _arialNormal.TextWidth(fontSize, dt.First().Prawa) + 2.0 * marginHor;

            var table = new PdfTable(_page, PdfContents, _arialNormal, fontSize)
            {
                TableArea = new PdfRectangle(left, bottom, right, top)
            };
            var array = new[] {colWidthTitle, colWidthDetail};
            table.SetColumnWidth(array);

            table.Borders.ClearAllBorders();

            var Margin = new PdfRectangle(marginHor, marginVer);

            table.DefaultHeaderStyle.Margin = Margin;
            table.DefaultHeaderStyle.BackgroundColor = Color.White;
            table.DefaultHeaderStyle.Alignment = ca;


            table.DefaultCellStyle.Margin = Margin;
            if (withHeader)
            {
                table.Header[0].Style.FontSize = 12;
                table.Header[0].Style.Font = _arialBold;
                table.Header[0].Value = dt.First().Lewa;
                table.Header[1].Value = dt.First().Prawa;
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

                table.Cell[0].Value = item.Lewa;
                table.Cell[1].Value = item.Prawa;
                table.DrawRow();
            }

            var LastRowPosition = table.RowPosition[table.RowNumber];

            table.Close();

            PdfContents.SaveGraphicsState();

            PdfContents.RestoreGraphicsState();
            return LastRowPosition;
        }

        private double TabelaDaneFaktura(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE,
            IEnumerable<DaneUsluga> daneUslugas)
        {
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(_page, PdfContents, _arialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            var array = new[] {1, 9.5, 1.5, 2.5, 2.5, 3.5, 3, 3, 3.5};
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);


            var Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = _arialBold;
            Table.DefaultHeaderStyle.MultiLineText = true;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            Table.Header[0].Value = DictionaryMain.KolumnaLp;
            Table.Header[1].Value = DictionaryMain.KolumnaTowar;
            Table.Header[2].Value = DictionaryMain.KolumnaJm;
            Table.Header[3].Value = DictionaryMain.KolumnaIlosc;
            Table.Header[4].Value = DictionaryMain.KolumnaCenaNetto;
            Table.Header[5].Value = DictionaryMain.KolumnaWartoscNetto;
            Table.Header[6].Value = DictionaryMain.KolumnaStawkaVat;
            Table.Header[7].Value = DictionaryMain.KolumnaKwotaVat;
            Table.Header[8].Value = DictionaryMain.KolumnaWartoscBrutto;

            Table.DefaultCellStyle.Margin = Margin;

            for (var i = 0; i < array.Length; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            foreach (var item in daneUslugas)
            {
                Table.Cell[0].Value = item.LpTabela;
                Table.Cell[1].Value = item.OpisTabela;
                Table.Cell[2].Value = item.Rodzajilosc;
                Table.Cell[3].Value = item.Ilosc;
                Table.Cell[4].Value = item.CenaNetto.ToCurrency();

                Table.Cell[5].Value = item.WartoscNetto.ToCurrency();
                Table.Cell[6].Value = item.StawkaVat;
                Table.Cell[7].Value = item.KwotaVat.ToCurrency();
                Table.Cell[8].Value = item.WartoscBrutto.ToCurrency();
                Table.DrawRow();
            }

            var positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double RamkiEnd(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, string tekst)
        {
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(_page, PdfContents, _arialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Table.SetColumnWidth(12);

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

            var positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double Summary(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE,
            IEnumerable<DaneUsluga> daneUslugas)
        {
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;


            var Table = new PdfTable(_page, PdfContents, _arialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 0.31, BOTTOM, RIGHT, TOP)
            };
            var array = new[] {3.5, 3, 3, 3.5};
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);


            var Margin = new PdfRectangle(MARGIN_HOR + 0.27, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = _arialBold;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            var enumerable = daneUslugas.ToList();
            Table.Header[0].Value = enumerable.First().WartoscNetto.ToCurrency();
            Table.Header[1].Value = enumerable.First().StawkaVat;
            Table.Header[2].Value = enumerable.First().KwotaVat.ToCurrency();
            Table.Header[3].Value = enumerable.First().WartoscBrutto.ToCurrency();

            Table.DefaultCellStyle.Margin = Margin;

            for (var i = 0; i < array.Length; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            var z = 0;
            foreach (var item in enumerable)
            {
                if (z == 0)
                {
                    z++;
                    continue;
                }

                Table.Cell[0].Value = item.WartoscNetto.ToCurrency();
                Table.Cell[1].Value = item.StawkaVat;
                Table.Cell[2].Value = item.KwotaVat.ToCurrency();
                Table.Cell[3].Value = item.WartoscBrutto.ToCurrency();
                Table.DrawRow();
            }

            var positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight;

            Table.Close();

            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private void RazemWTym(double LEFT, double TOP, double BOTTOM, double FONT_SIZE)
        {
            var TableLeft = new PdfTable(_page, PdfContents, _arialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 3, BOTTOM, LEFT - 0.5, TOP)
            };

            TableLeft.SetColumnWidth(2.5);
            TableLeft.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;
            TableLeft.DefaultHeaderStyle.BackgroundColor = Color.Transparent;
            TableLeft.Header[0].Value = DictionaryMain.SummaRazem;
            TableLeft.Cell[0].Style.Alignment = ContentAlignment.MiddleRight;
            TableLeft.Cell[0].Value = DictionaryMain.SummaWTym;
            TableLeft.DrawRow();
            PdfContents.SaveGraphicsState();
            PdfContents.RestoreGraphicsState();
        }
    }
}