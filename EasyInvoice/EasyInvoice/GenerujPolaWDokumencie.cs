using PdfFileWriter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace EasyInvoice
{
    public class GenerujPolaWDokumencie
    {
        private IDaneNaglowek m_DaneNaglowka;
        private IHelperData m_HelperData;
        private readonly PdfFont ArialNormal;
        private readonly PdfFont ArialBold;
        private readonly PdfPage Page;
        private PdfDocument m_PdfDocument;
        
        private PdfContents m_PdfContents { get; set; }
        private const string ArialFontName = "Arial";

        public GenerujPolaWDokumencie(string FileName, IHelperData helperData, IDaneNaglowek daneNaglowek, SingleFakturaProperty singleFaktura)
        {
            m_HelperData = helperData;
            m_DaneNaglowka = daneNaglowek;

            m_PdfDocument = new PdfDocument(PaperType.A4, false, UnitOfMeasure.cm, FileName)
            {
                Debug = false
            };

            ArialBold = PdfFont.CreatePdfFont(m_PdfDocument, ArialFontName, FontStyle.Bold, true);
            ArialNormal = PdfFont.CreatePdfFont(m_PdfDocument, ArialFontName, FontStyle.Regular, true);

            var Info = PdfInfo.CreatePdfInfo(m_PdfDocument);
            Info.Title("Faktura");
            Info.Author("TZ");
            Info.Keywords("keyword");
            Info.Subject("Temat");
            
            Page = new PdfPage(m_PdfDocument);
            m_PdfContents = new PdfContents(Page);

            IdFaktury(singleFaktura.Work.Naglowek.NumerFaktury);

            double lastPosition = 26;

            CreateTable(m_DaneNaglowka.GetNaglowekL(), 1.3, 10, 8, lastPosition, lastPosition-3, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(m_DaneNaglowka.GetNaglowekR(), 10.3, 16, 8, lastPosition, lastPosition-1.5, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(this.m_HelperData.GetSprzeNaby(), 1.3, 30, 8, lastPosition - 1.4, lastPosition-6.2, true, ContentAlignment.MiddleLeft);

            double WidthRow = ArialNormal.TextWidth(8, this.m_HelperData.NrBankowy()[0].Lewa + this.m_HelperData.NrBankowy()[0].Prawa) + 0.25;

            lastPosition = CreateTable(this.m_HelperData.NrBankowy(), 1.3, 1.3+ WidthRow, 8, lastPosition - 1, lastPosition - 6.2, false, ContentAlignment.MiddleLeft);
            
            lastPosition =TabelaDaneFaktura(1.3, lastPosition-1, lastPosition-11, 19.7, 9, singleFaktura.GetListDt());

            RazemWTym(12.03, lastPosition - 0.018, lastPosition - 11, 9);

            lastPosition = Summary(12.03, lastPosition-0.018, lastPosition-11, 19.7, 9, singleFaktura.GetSum());

            WidthRow = ArialNormal.TextWidth(8, this.m_HelperData.GetZapDoZap()[1].Lewa + this.m_HelperData.GetZapDoZap()[1].Prawa)+0.25;
            CreateTable(this.m_HelperData.GetZapDoZap(), 1.3, 1.3+ WidthRow, 8, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleLeft);

            WidthRow = ArialNormal.TextWidth(12, this.m_HelperData.Razem()[0].Lewa + this.m_HelperData.Razem()[0].Prawa);
            lastPosition = CreateTable(this.m_HelperData.Razem(), 19.7- WidthRow, 19.7, 12, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleRight);

            WidthRow = ArialNormal.TextWidth(8, this.m_HelperData.RazemSlownie().Lewa + this.m_HelperData.RazemSlownie().Prawa);
            lastPosition = CreateTable(new List<DaneTabela> { this.m_HelperData.RazemSlownie() }, 19.7 - WidthRow, 19.7, 8, lastPosition, lastPosition - 10, false, ContentAlignment.MiddleRight);

            RamkiEnd(1.3, lastPosition - 1.4, lastPosition - 5, 9, 7, DictionaryMain.labelPodpisWystawiania);

            RamkiEnd(12, lastPosition - 1.4, lastPosition - 5, 19.7, 7, DictionaryMain.labelPodpisOdbierania);

            m_PdfDocument.CreateFile();

            return;
        }
        

        private void IdFaktury(string nrFaktury)
        {
            m_PdfContents.SaveGraphicsState();
            
            m_PdfContents.Translate(1.3, 21.0);
            
            const double Width = 60;
            const double Height = 7;
            const double FontSize = 40;
            var txtF = new TextBox(Width, 0);
            var txtNr = new TextBox(Width, 0);

            txtF.AddText(ArialNormal, FontSize, DictionaryMain.labelNrFaktury);
            txtNr.AddText(ArialBold, FontSize, nrFaktury);

            double PosY =  Height;
            m_PdfContents.DrawText(0.0, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtF);
            PosY = Height;
            double sizeLabel = ArialNormal.TextWidth(FontSize, DictionaryMain.labelNrFaktury) + 1;
            m_PdfContents.DrawText(sizeLabel, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtNr);

            m_PdfContents.RestoreGraphicsState();
        }
        
        private double CreateTable(List<DaneTabela> dt, double left, double right, double fontSize, double top, double bottom, bool withHeader, ContentAlignment ca)
        {
            double LastRowPosition;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;

            double colWidthTitle = ArialNormal.TextWidth(fontSize, dt.First().Lewa) + 2.0 * MARGIN_HOR;
            double colWidthDetail = ArialNormal.TextWidth(fontSize, dt.First().Prawa) + 2.0 * MARGIN_HOR;
            
            var Table = new PdfTable(Page, m_PdfContents, ArialNormal, fontSize)
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
                Table.Header[0].Value = dt.First().Lewa;
                Table.Header[1].Value = dt.First().Prawa;
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
                Table.Cell[0].Value = item.Lewa;
                Table.Cell[1].Value = item.Prawa;
                Table.DrawRow();
            }

            LastRowPosition = Table.RowPosition[Table.RowNumber];

            Table.Close();
            
            m_PdfContents.SaveGraphicsState();
            
            m_PdfContents.RestoreGraphicsState();
            return LastRowPosition;
        }

        private double TabelaDaneFaktura(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, IEnumerable<DaneUsluga> daneUslugas)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(Page, m_PdfContents, ArialNormal, FONT_SIZE)
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

            positionLast = Table.RowPosition[Table.RowNumber]- Table.RowHeight; ;
            
            Table.Close();

            m_PdfContents.SaveGraphicsState();            
            m_PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double RamkiEnd(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, string tekst)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;

            var Table = new PdfTable(Page, m_PdfContents, ArialNormal, FONT_SIZE)
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

            m_PdfContents.SaveGraphicsState();
            m_PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private double Summary(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, IEnumerable<DaneUsluga> daneUslugas)
        {
            double positionLast;
            const double MARGIN_HOR = 0.04;
            const double MARGIN_VER = 0.04;
            const double FRAME_WIDTH = 0.015;
            

            var Table = new PdfTable(Page, m_PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT-0.31, BOTTOM, RIGHT, TOP)
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

            Table.Header[0].Value = daneUslugas.First().WartoscNetto.ToCurrency();
            Table.Header[1].Value = daneUslugas.First().StawkaVat;
            Table.Header[2].Value = daneUslugas.First().KwotaVat.ToCurrency();
            Table.Header[3].Value = daneUslugas.First().WartoscBrutto.ToCurrency();

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
                if(z==0)
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

            positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight; ;

            Table.Close();

            m_PdfContents.SaveGraphicsState();
            m_PdfContents.RestoreGraphicsState();
            return positionLast;
        }

        private void RazemWTym(double LEFT, double TOP, double BOTTOM, double FONT_SIZE)
        {
            var TableLeft = new PdfTable(Page, m_PdfContents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 3, BOTTOM, LEFT-0.5, TOP)
            };

            TableLeft.SetColumnWidth(new double[] { 2.5 });
            TableLeft.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;
            TableLeft.DefaultHeaderStyle.BackgroundColor = Color.Transparent;
            TableLeft.Header[0].Value = DictionaryMain.summaRazem;
            TableLeft.Cell[0].Style.Alignment = ContentAlignment.MiddleRight;
            TableLeft.Cell[0].Value = DictionaryMain.summaWTym;
            TableLeft.DrawRow();
            m_PdfContents.SaveGraphicsState();
            m_PdfContents.RestoreGraphicsState();
        }
    }    
}
