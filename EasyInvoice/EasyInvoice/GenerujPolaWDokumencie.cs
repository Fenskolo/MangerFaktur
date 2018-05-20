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
        DaneNaglowek d = new DaneNaglowek();
        HelperData h = new HelperData();
        private static PdfFont ArialNormal;
        private static PdfFont ArialBold;
        private static PdfFont ArialItalic;
        private static PdfFont ArialBoldItalic;
        private static PdfFont TimesNormal;
        private static PdfFont SimHei;
        private static PdfFont Comic;
        private static PdfDocument document;
        private static PdfPage Page;
        private static PdfContents Contents;
        private static string numerFaktury1 = "Faktura 12345";
        
        public List<DaneUsluga> GetListTable()
        {
            List<DaneUsluga> list = new List<DaneUsluga>();
            DaneUsluga d = new DaneUsluga()
            {
                LpTabela = 1,
                OpisTabela = "Usługi Programistyczne",
                Rodzajilosc = "szt.",
                Ilosc = 160,
                CenaN = 10
            };
            d.WartoscN = d.Ilosc * d.CenaN;
            d.StawkaV = "23%";
            d.KwotaV = d.WartoscN * 0.23;
            d.WartoscB = d.KwotaV + d.WartoscN;
            list.Add(d);
            DaneUsluga d1 = new DaneUsluga()
            {
                LpTabela = 1,
                OpisTabela = "konsulktacje",
                Rodzajilosc = "szt.",
                Ilosc = 10,
                CenaN = 10
            };
            d1.WartoscN = d1.Ilosc * d1.CenaN;
            d1.StawkaV = "23%";
            d1.KwotaV = d1.WartoscN * 0.23;
            d1.WartoscB = d1.KwotaV + d1.WartoscN;

            list.Add(d1);

            return list;
        }

        public GenerujPolaWDokumencie(string FileName)
        {
            double position;
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

            FirstTable(d.GetNaglowekL(), 1.3, 10, 8, 27, 24, false);

            position = FirstTable(d.GetNaglowekR(), 10.3, 16, 8, 27, 25.5, false);

            position = FirstTable(h.GetSprzeNaby(), 1.3, 30, 8, position - 1.4, 20, true);

            position=TabelaDaneFaktura(1.3, position-1.4, 10.3, 19.7, 9);
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
            Comic = PdfFont.CreatePdfFont(document, "Comic Sans MS", FontStyle.Bold, true);
            SimHei = PdfFont.CreatePdfFont(document, "SimHei", FontStyle.Regular, true);
            return;
        }

        private static void IdFaktury()
        {
            Contents.SaveGraphicsState();
            
            Contents.Translate(1.3, 21.0);
            
            const Double Width = 60;
            const Double Height = 8;
            const Double FontSize = 40;
            TextBox customerContact = new TextBox(Width, 0);


            customerContact.AddText(ArialNormal, FontSize, numerFaktury1);

            Double PosY = Height;
            Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, TextBoxJustify.Left, customerContact);

            Contents.RestoreGraphicsState();
            return;
        }

        private double FirstTable(List<DaneTabela> dt, double left, double right, double fontSize, double top, double bottom, bool withHeader)
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
            Table.SetColumnWidth(new Double[] { colWidthTitle, colWidthDetail });
            
            Table.Borders.ClearAllBorders();
            
            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);
            
            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.White;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.MiddleLeft;


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
            // loop for all items
            //for (int i = 0; i < dt.GetLength(0); i++)
            //{
            //    for (int j = 0; j < dt.GetLength(1); j++)
            //    {
            //        if (withHeader && dt.in == 0)
            //        {
            //            continue;
            //        }

            //        Table.Cell[j].Value = dt[i, j];

            //    }

            //    Table.DrawRow();
            //}
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
            
            Double colWidthType = ArialNormal.TextWidth(FONT_SIZE, "Overseas Student Account") + 2.0 * MARGIN_HOR;
            Double colWidthCcy = ArialNormal.TextWidth(FONT_SIZE, "AUD") + 2.0 * MARGIN_HOR;
            Double colWidthLongNumber = ArialNormal.TextWidth(FONT_SIZE, "  International Transfer  ") + 2.0 * MARGIN_HOR;
            Double colWidthShortNumber = ArialNormal.TextWidth(FONT_SIZE, "  Domestic Transfer  ") + 2.0 * MARGIN_HOR;
            Double asasa = ArialNormal.TextWidth(FONT_SIZE, "  assadsa  ") + 2.0 * MARGIN_HOR;
            
            PdfTable Table = new PdfTable(Page, Contents, ArialNormal, FONT_SIZE)
            {
                TableArea = new PdfRectangle(LEFT, BOTTOM, RIGHT, TOP)
            };
            Table.SetColumnWidth(new Double[] { 1, 10, 1.5, 2.5, 2.5, 3.5, 3, 3, 3 });
            
            Table.Borders.ClearAllBorders();
            Table.Borders.SetAllBorders(FRAME_WIDTH, FRAME_WIDTH);

           
            PdfRectangle Margin = new PdfRectangle(MARGIN_HOR, MARGIN_VER);
            
            Table.DefaultHeaderStyle.Margin = Margin;
            Table.DefaultHeaderStyle.BackgroundColor = Color.LightGray;
            Table.DefaultHeaderStyle.Font = ArialBold;
            Table.DefaultHeaderStyle.MultiLineText = true;
            Table.DefaultHeaderStyle.Alignment = ContentAlignment.TopCenter;
           
            Table.Header[0].Value = "Lp.";
            Table.Header[1].Value = "Towar/ usługa";
            Table.Header[1].Style.Alignment = ContentAlignment.TopCenter;
            Table.Header[2].Value = "J.m.";
            Table.Header[3].Value = "Ilość";
            Table.Header[4].Value = "Cena Netto";
            Table.Header[5].Value = "Wartość netto";
            Table.Header[6].Value = "Stawka VAT";
            Table.Header[7].Value = "Kwota VAT";
            Table.Header[8].Value = "Wartość brutto";
            
            Table.DefaultCellStyle.Margin = Margin;
            
            for (int i = 0; i < 4; i++)
            {
                Table.Cell[i].Style = Table.CellStyle;
                Table.Cell[i].Style.MultiLineText = false;
                Table.Cell[i].Style.Alignment = ContentAlignment.MiddleCenter;
                Table.CellStyle.TextDrawStyle = DrawStyle.Superscript;
            }
            
            foreach (var item in GetListTable())
            {
                Table.Cell[0].Value = item.LpTabela;
                Table.Cell[1].Value = item.OpisTabela;
                Table.Cell[2].Value = item.Rodzajilosc;
                Table.Cell[3].Value = item.Ilosc;
                Table.Cell[4].Value = item.CenaN;

                Table.Cell[5].Value = item.WartoscN;
                Table.Cell[6].Value = item.StawkaV;
                Table.Cell[7].Value = item.KwotaV;
                Table.Cell[8].Value = item.WartoscB;
                Table.DrawRow();
            }

            positionLast = Table.RowPosition[Table.RowNumber]- Table.RowHeight; ;
            
            Table.Close();

            Contents.SaveGraphicsState();            
            Contents.RestoreGraphicsState();
            return positionLast;
        }
    }    
}
