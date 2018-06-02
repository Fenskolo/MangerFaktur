using PdfFileWriter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EasyInvoice
{    
    public class GenerujPolaWDokumencie
    {
        DaneNaglowek dNaglowek = new DaneNaglowek();
        HelperData hData = new HelperData();
        private static PdfFont ArialNormal;
        private static PdfFont ArialBold;
        private static PdfFont ArialItalic;
        private static PdfFont ArialBoldItalic;
        private static PdfFont TimesNormal;
        private static PdfDocument document;
        private static PdfPage Page;
        private static PdfContents Contents;        

        public GenerujPolaWDokumencie(string FileName)
        {
            double lastPosition = 26;
            bool landscape = false;

            document = new PdfDocument(PaperType.A4, landscape, UnitOfMeasure.cm, FileName)
            {
                Debug = false
            };
            
            PdfInfo Info = PdfInfo.CreatePdfInfo(document);
            Info.Title("Faktura");
            Info.Author("TZ");
            Info.Keywords("keyword");
            Info.Subject("Temat");
            
            DefineFontResources();
            Page = new PdfPage(document);
            Contents = new PdfContents(Page);

            IdFaktury();

            CreateTable(dNaglowek.GetNaglowekL(), 1.3, 10, 8, lastPosition, lastPosition-3, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(dNaglowek.GetNaglowekR(), 10.3, 16, 8, lastPosition, lastPosition-1.5, false, ContentAlignment.MiddleLeft);

            lastPosition = CreateTable(hData.GetSprzeNaby(), 1.3, 30, 8, lastPosition - 1.4, lastPosition-6.2, true, ContentAlignment.MiddleLeft);

            Double WidthRow = ArialNormal.TextWidth(8, hData.NrBankowy()[0].Lewa + hData.NrBankowy()[0].Prawa) + 0.25;

            lastPosition = CreateTable(hData.NrBankowy(), 1.3, 1.3+WidthRow, 8, lastPosition - 1, lastPosition - 6.2, false, ContentAlignment.MiddleLeft);


            lastPosition =TabelaDaneFaktura(1.3, lastPosition-1, lastPosition-11, 19.7, 9);

            RazemWTym(12.03, lastPosition - 0.01, lastPosition - 11, 9);

            lastPosition = Summary(12.03, lastPosition-0.01, lastPosition-11, 19.7, 9);

            WidthRow = ArialNormal.TextWidth(8, hData.GetZapDoZap()[1].Lewa + hData.GetZapDoZap()[1].Prawa)+0.25; 
            CreateTable(hData.GetZapDoZap(), 1.3, 1.3+ WidthRow, 8, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleLeft);

            WidthRow = ArialNormal.TextWidth(12, hData.Razem()[0].Lewa + hData.Razem()[0].Prawa);
            lastPosition = CreateTable(hData.Razem(), 19.7-WidthRow, 19.7, 12, lastPosition - 1, lastPosition - 10, false, ContentAlignment.MiddleRight);

            WidthRow = ArialNormal.TextWidth(8, hData.RazemSlownie()[0].Lewa + hData.RazemSlownie()[0].Prawa);
            lastPosition = CreateTable(hData.RazemSlownie(), 19.7 - WidthRow, 19.7, 8, lastPosition, lastPosition - 10, false, ContentAlignment.MiddleRight);

            RamkiEnd(1.3, lastPosition - 1.4, lastPosition - 5, 9, 7, DictionaryMain.labelPodpisWystawiania);

            RamkiEnd(12, lastPosition - 1.4, lastPosition - 5, 19.7, 7, DictionaryMain.labelPodpisOdbierania);

            document.CreateFile();

            return;
        }

        private static void DefineFontResources()
        {
            String arialFontName = "Arial";
            String timesNewRomanFontName = "Times New Roman";

            ArialNormal = PdfFont.CreatePdfFont(document, arialFontName, FontStyle.Regular, true);
            ArialBold = PdfFont.CreatePdfFont(document, arialFontName, FontStyle.Bold, true);
            ArialItalic = PdfFont.CreatePdfFont(document, arialFontName, FontStyle.Italic, true);
            ArialBoldItalic = PdfFont.CreatePdfFont(document, arialFontName, FontStyle.Bold | FontStyle.Italic, true);
            TimesNormal = PdfFont.CreatePdfFont(document, timesNewRomanFontName, FontStyle.Regular, true);
            return;
        }

        private static void IdFaktury()
        {
            Contents.SaveGraphicsState();
            
            Contents.Translate(1.3, 21.0);
            
            const Double Width = 60;
            const Double Height = 7;
            const Double FontSize = 40;
            TextBox txtF = new TextBox(Width, 0);
            TextBox txtNr = new TextBox(Width, 0);

            txtF.AddText(ArialNormal, FontSize, DictionaryMain.labelNrFaktury);
            txtNr.AddText(ArialBold, FontSize, SingleFakturaProperty.Singleton.Naglowek.NumerFaktury);
            
            Double PosY =  Height;
            Contents.DrawText(0.0, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtF);
            PosY = Height;
            Double sizeLabel = ArialNormal.TextWidth(FontSize, DictionaryMain.labelNrFaktury) + 1;
            Contents.DrawText(sizeLabel, ref PosY, 0, 0, 0.015, 0.05, TextBoxJustify.Left, txtNr);

            Contents.RestoreGraphicsState();
            return;
        }


        private double CreateTable(List<DaneTabela> dt, double left, double right, double fontSize, double top, double bottom, bool withHeader, ContentAlignment ca)
        {
            double LastRowPosition;
            Double LEFT = left;
            Double TOP = top;
            Double BOTTOM = bottom;
            Double RIGHT = right;
            Double FONT_SIZE = fontSize;
            const Double MARGIN_HOR = 0.04;
            const Double MARGIN_VER = 0.04;
            
            Double colWidthTitle = ArialNormal.TextWidth(FONT_SIZE, dt[0].Lewa) + 2.0 * MARGIN_HOR;
            Double colWidthDetail = ArialNormal.TextWidth(FONT_SIZE, dt[0].Prawa) + 2.0 * MARGIN_HOR;
            
            PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Double[] array = new Double[] { colWidthTitle, colWidthDetail };
            Table.SetColumnWidth(array);
            
            Table.Borders.ClearAllBorders();
            
            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);
            
            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.White;
            Table.DefaultHeaderStyle.Alignment = ca;


            Table.DefaultCellStyle.Margin = Margin;
            if (withHeader)
            {
                Table.Header[0].Style.FontSize = 12;
                Table.Header[0].Style.Font = ArialBold;
                if (dt.Count > 0)
                {
                    Table.Header[0].Value = dt[0].Lewa;
                    Table.Header[1].Value = dt[0].Prawa;
                    Table.Header[0].Style.FontSize = 12;
                    Table.Header[0].Style.Font = ArialBold;
                    Table.Header[1].Style.FontSize = 12;
                    Table.Header[1].Style.Font = ArialBold;
                }

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
            
            Contents.SaveGraphicsState();
            
            Contents.RestoreGraphicsState();
            return LastRowPosition;
        }

        private double TabelaDaneFaktura(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE)
        {
            double positionLast;
            const Double MARGIN_HOR = 0.04;
            const Double MARGIN_VER = 0.04;
            const Double FRAME_WIDTH = 0.015;
            
            PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Double[] array = new Double[] { 1, 10, 1.5, 2.5, 2.5, 3.5, 3, 3, 3 };
            Table.SetColumnWidth(array);
            
            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);

           
            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);
            
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
            
            foreach (var item in  SingleFakturaProperty.Singleton.GetListDt())// hData.GetListTable())
            {
                Table.Cell[0].Value = item.LpTabela;
                Table.Cell[1].Value = item.OpisTabela;
                Table.Cell[2].Value = item.Rodzajilosc;
                Table.Cell[3].Value = item.Ilosc;
                Table.Cell[4].Value = item.CenaNetto;

                Table.Cell[5].Value = item.WartoscNetto;
                Table.Cell[6].Value = item.StawkaVat;
                Table.Cell[7].Value = item.KwotaVat;
                Table.Cell[8].Value = item.WartoscBrutto;
                Table.DrawRow();
            }

            positionLast = Table.RowPosition[Table.RowNumber]- Table.RowHeight; ;
            
            Table.Close();

            Contents.SaveGraphicsState();            
            Contents.RestoreGraphicsState();
            return positionLast;
        }

        private double RamkiEnd(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE, string tekst)
        {
            double positionLast;
            const Double MARGIN_HOR = 0.04;
            const Double MARGIN_VER = 0.04;
            const Double FRAME_WIDTH = 0.015;

            PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Table.SetColumnWidth(new Double[] { 12 });

            Table.Borders.ClearAllBorders();
            Table.Borders.SetFrame(FRAME_WIDTH);

            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);
                 
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

            Contents.SaveGraphicsState();
            Contents.RestoreGraphicsState();
            return positionLast;
        }

        private double Summary(double LEFT, double TOP, double BOTTOM, double RIGHT, double FONT_SIZE)
        {
            double positionLast;
            const Double MARGIN_HOR = 0.04;
            const Double MARGIN_VER = 0.04;
            const Double FRAME_WIDTH = 0.015;
            

            PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Double[] array = new Double[] { 3.5, 3, 3, 3 };
            Table.SetColumnWidth(array);

            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);


            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);

            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = ArialBold;
            Table.DefaultHeaderStyle.MultiLineText = true;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;

            Table.Header[0].Value = hData.GetSum()[0].WartoscNetto;
            Table.Header[1].Value = hData.GetSum()[0].StawkaVat;
            Table.Header[2].Value = hData.GetSum()[0].KwotaVat;
            Table.Header[3].Value = hData.GetSum()[0].WartoscBrutto;

            Table.DefaultCellStyle.Margin = Margin;

            for (int i = 0; i < array.Length; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }

            int z = 0;
            foreach (var item in hData.GetSum())
            {
                if(z==0)
                {
                    z++;
                    continue;
                }
                Table.Cell[0].Value = item.WartoscNetto;
                Table.Cell[1].Value = item.StawkaVat;
                Table.Cell[2].Value = item.KwotaVat;
                Table.Cell[3].Value = item.WartoscBrutto;
                Table.DrawRow();
            }

            positionLast = Table.RowPosition[Table.RowNumber] - Table.RowHeight; ;

            Table.Close();

            Contents.SaveGraphicsState();
            Contents.RestoreGraphicsState();
            return positionLast;
        }

        private static void RazemWTym(double LEFT, double TOP, double BOTTOM, double FONT_SIZE)
        {
            PdfTable TableLeft = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT - 1.5, BOTTOM, LEFT, TOP)
            };

            TableLeft.SetColumnWidth(new Double[] { 2.5 });
            TableLeft.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleRight;
            TableLeft.DefaultHeaderStyle.BackgroundColor = Color.Transparent;
            TableLeft.Header[0].Value = DictionaryMain.summaRazem;
            TableLeft.Cell[0].Style.Alignment = ContentAlignment.MiddleRight;
            TableLeft.Cell[0].Value = DictionaryMain.summaWTym;
            TableLeft.DrawRow();
            Contents.SaveGraphicsState();
            Contents.RestoreGraphicsState();
        }
    }    
}
