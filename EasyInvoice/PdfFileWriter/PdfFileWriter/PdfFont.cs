/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfFont
//	PDF Font resource.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2016 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace PdfFileWriter
{
    /////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     PDF font descriptor flags enumeration
    /// </summary>
    /////////////////////////////////////////////////////////////////////
    public enum PdfFontFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0,

        /// <summary>
        ///     Fixed pitch font
        /// </summary>
        FixedPitch = 1,

        /// <summary>
        ///     Serif font
        /// </summary>
        Serif = 1 << 1,

        /// <summary>
        ///     Symbolic font
        /// </summary>
        Symbolic = 1 << 2,

        /// <summary>
        ///     Script font
        /// </summary>
        Script = 1 << 3,

        /// <summary>
        ///     Non-symbolic font
        /// </summary>
        Nonsymbolic = 1 << 5,

        /// <summary>
        ///     Italic font
        /// </summary>
        Italic = 1 << 6,

        /// <summary>
        ///     All cap font
        /// </summary>
        AllCap = 1 << 16,

        /// <summary>
        ///     Small cap font
        /// </summary>
        SmallCap = 1 << 17,

        /// <summary>
        ///     Force bold font
        /// </summary>
        ForceBold = 1 << 18
    }

    /////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     Kerning adjustment class
    /// </summary>
    /// <remarks>
    ///     Text position adjustment for TJ operator.
    ///     The adjustment is for a font height of one point.
    ///     Mainly used for font kerning.
    /// </remarks>
    /////////////////////////////////////////////////////////////////////
    public class KerningAdjust
    {
        /// <summary>
        ///     Kerning adjustment constructor
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="Adjust">Adjustment</param>
        public KerningAdjust
        (
            string Text,
            double Adjust
        )
        {
            this.Text = Text;
            this.Adjust = Adjust;
        }

        /// <summary>
        ///     Gets or sets Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Gets or sets adjustment
        /// </summary>
        /// <remarks>
        ///     Adjustment units are in PDF design unit. To convert to user units: Adjust * FontSize / (1000.0 * ScaleFactor)
        /// </remarks>
        public double Adjust { get; set; }
    }

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     PDF font class
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         For more information go to
    ///         <a
    ///             href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#LanguageSupport">
    ///             2.3
    ///             Language Support
    ///         </a>
    ///     </para>
    ///     <para>
    ///         <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#FontResources">
    ///             For
    ///             example of defining font resources see 3.2. Font Resources
    ///         </a>
    ///     </para>
    /// </remarks>
    ////////////////////////////////////////////////////////////////////
    public class PdfFont : PdfObject, IDisposable, IComparable<PdfFont>
    {
        internal CharInfo[][] CharInfoArray;
        internal bool[] CharInfoBlockEmpty;
        internal int DesignCapHeight;
        internal Font DesignFont;
        internal int DesignFontWeight;
        internal int DesignHeight;
        internal int DesignItalicAngle;
        internal int DesignStrikeoutPosition;
        internal int DesignStrikeoutWidth;
        internal int DesignSubscriptPosition;
        internal int DesignSubscriptSize;
        internal int DesignSuperscriptPosition;
        internal int DesignSuperscriptSize;
        internal int DesignUnderlinePosition;
        internal int DesignUnderlineWidth;
        internal bool EmbeddedFont;
        internal FontApi FontApi;
        internal FontFamily FontFamily;
        internal string FontFamilyName;
        internal PdfFontFlags FontFlags;
        internal bool FontResCodeUsed; // 0-255
        internal bool FontResGlyphUsed; // 255-0xffff
        internal FontStyle FontStyle;
        internal PdfObject GlyphIndexFont;
        internal int NewGlyphIndex;
        internal int PdfAscent;
        internal int PdfDescent;
        internal int PdfLineSpacing;
        internal string ResourceCodeGlyph; // resource code automatically generated by the program
        internal bool SymbolicFont;
        internal CharInfo UndefinedCharInfo;

        // for search only
        private PdfFont
        (
            string FontFamilyName, // font family name
            FontStyle FontStyle, // font style (Regular, Bold, Italic or Bold | Italic
            bool EmbeddedFont = true // embed font in PDF document file
        )
        {
            // save parameters
            this.FontFamilyName = FontFamilyName;
            this.FontStyle = FontStyle;
            this.EmbeddedFont = EmbeddedFont;
        }

        private PdfFont
        (
            PdfDocument Document, // PDF document main object
            string FontFamilyName, // font family name
            FontStyle FontStyle, // font style (Regular, Bold, Italic or Bold | Italic
            bool EmbeddedFont = true // embed font in PDF document file
        ) : base(Document, ObjectType.Dictionary, "/Font")
        {
            // save parameters
            this.FontFamilyName = FontFamilyName;
            this.FontStyle = FontStyle;
            this.EmbeddedFont = EmbeddedFont;

            // font style cannot be underline or strikeout
            if ((FontStyle & (FontStyle.Underline | FontStyle.Strikeout)) != 0)
            {
                throw new ApplicationException("Font resource cannot have underline or strikeout");
            }

            // create two resource codes
            ResourceCode = Document.GenerateResourceNumber('F');
            ResourceCodeGlyph = Document.GenerateResourceNumber('F');

            // initialize new glyph index to 3
            NewGlyphIndex = 3;

            // get font family structure
            FontFamily = new FontFamily(FontFamilyName);

            // test font style availability
            if (!FontFamily.IsStyleAvailable(FontStyle))
            {
                throw new ApplicationException("Font style not available for font family");
            }

            // design height
            DesignHeight = FontFamily.GetEmHeight(FontStyle);

            // Ascent, descent and line spacing for a one point font
            PdfAscent = FontFamily.GetCellAscent(FontStyle);
            PdfDescent = FontFamily.GetCellDescent(FontStyle); // positive number
            PdfLineSpacing = FontFamily.GetLineSpacing(FontStyle);

            // create design font
            DesignFont = new Font(FontFamily, DesignHeight, FontStyle, GraphicsUnit.Pixel);

            // create windows sdk font info object
            FontApi = new FontApi(DesignFont, DesignHeight);

            // create empty array of character information
            CharInfoArray = new CharInfo[256][];
            CharInfoBlockEmpty = new bool[256];

            // get undefined character info
            UndefinedCharInfo = FontApi.GetGlyphMetricsApiByGlyphIndex(0);
            UndefinedCharInfo.NewGlyphIndex = 0;

            // get outline text metrics structure
            var OTM = FontApi.GetOutlineTextMetricsApi();

            // make sure we have true type font and not device font
            if ((OTM.otmTextMetric.tmPitchAndFamily & 0xe) != 6)
            {
                throw new ApplicationException("Font must be True Type and vector");
            }

            // PDF font flags
            FontFlags = 0;
            if ((OTM.otmfsSelection & 1) != 0)
            {
                FontFlags |= PdfFontFlags.Italic;
            }

            // roman font is a serif font
            if (OTM.otmTextMetric.tmPitchAndFamily >> 4 == 1)
            {
                FontFlags |= PdfFontFlags.Serif;
            }

            if (OTM.otmTextMetric.tmPitchAndFamily >> 4 == 4)
            {
                FontFlags |= PdfFontFlags.Script;
            }

            // #define SYMBOL_CHARSET 2
            if (OTM.otmTextMetric.tmCharSet == 2)
            {
                FontFlags |= PdfFontFlags.Symbolic;
                SymbolicFont = true;
            }
            else
            {
                FontFlags |= PdfFontFlags.Nonsymbolic;
                SymbolicFont = false;
            }

            // #define TMPF_FIXED_PITCH 0x01 (Note very carefully that those meanings are the opposite of what the constant name implies.)
            if ((OTM.otmTextMetric.tmPitchAndFamily & 1) == 0)
            {
                FontFlags |= PdfFontFlags.FixedPitch;
            }

            // strikeout
            DesignStrikeoutPosition = OTM.otmsStrikeoutPosition;
            DesignStrikeoutWidth = (int) OTM.otmsStrikeoutSize;

            // underline
            DesignUnderlinePosition = OTM.otmsUnderscorePosition;
            DesignUnderlineWidth = OTM.otmsUnderscoreSize;

            // subscript
            DesignSubscriptSize = OTM.otmptSubscriptSize.Y;
            DesignSubscriptPosition = OTM.otmptSubscriptOffset.Y;

            // superscript
            DesignSuperscriptSize = OTM.otmptSuperscriptSize.Y;
            DesignSuperscriptPosition = OTM.otmptSuperscriptOffset.Y;

            // italic angle is 10th of a degree
            DesignItalicAngle = OTM.otmItalicAngle;
            DesignFontWeight = OTM.otmTextMetric.tmWeight;

            DesignCapHeight = FontApi.GetGlyphMetricsApiByCode('M').DesignBBoxTop;

            // exit
        }

        internal int PdfLeading => PdfLineSpacing - PdfAscent - PdfDescent;

        /// <summary>
        ///     Compage PDF font objects
        /// </summary>
        /// <param name="Other">Other PDFFont</param>
        /// <returns>Compare result</returns>
        public int CompareTo
        (
            PdfFont Other
        )
        {
            var Cmp = string.Compare(FontFamilyName, Other.FontFamilyName, true);
            if (Cmp != 0)
            {
                return Cmp;
            }

            Cmp = FontStyle - Other.FontStyle;
            if (Cmp != 0)
            {
                return Cmp;
            }

            return (EmbeddedFont ? 1 : 0) - (Other.EmbeddedFont ? 1 : 0);
        }

        /// <summary>
        ///     Dispose FontApi
        /// </summary>
        public void Dispose()
        {
            if (FontApi != null)
            {
                FontApi.Dispose();
                FontApi = null;
            }
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     PDF Font resource constructor
        /// </summary>
        /// <param name="Document">Document object</param>
        /// <param name="FontFamilyName">Font family name</param>
        /// <param name="FontStyle">Font style</param>
        /// <param name="EmbeddedFont">Embedded font</param>
        /// <returns>PdfFont resource</returns>
        /// <remarks>
        ///     The returned result is either a new PdfFont or an
        ///     existing one with the same properties.
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public static PdfFont CreatePdfFont
        (
            PdfDocument Document, // PDF document main object
            string FontFamilyName, // font family name
            FontStyle FontStyle, // font style (Regular, Bold, Italic or Bold | Italic
            bool EmbeddedFont = true // embed font in PDF document file
        )
        {
            if (Document.FontArray == null)
            {
                Document.FontArray = new List<PdfFont>();
            }

            var Index = Document.FontArray.BinarySearch(new PdfFont(FontFamilyName, FontStyle, EmbeddedFont));
            if (Index >= 0)
            {
                return Document.FontArray[Index];
            }

            var NewFont = new PdfFont(Document, FontFamilyName, FontStyle, EmbeddedFont);
            Document.FontArray.Insert(~Index, NewFont);
            return NewFont;
        }

        ////////////////////////////////////////////////////////////////////
        // Create glyph index font object on first usage
        ////////////////////////////////////////////////////////////////////

        internal void CreateGlyphIndexFont()
        {
            GlyphIndexFont = new PdfObject(Document, ObjectType.Dictionary, "/Font");
            FontResGlyphUsed = true;
        }

        /// <summary>
        ///     Get character information
        /// </summary>
        /// <param name="CharValue">Character value</param>
        /// <returns>Character information class</returns>
        public CharInfo GetCharInfo
        (
            int CharValue
        )
        {
            // no support for control characters 
            if (CharValue < ' ' || CharValue > '~' && CharValue < 160 || CharValue > 0xffff)
            {
                throw new ApplicationException("No support for control characters 0-31 or 127-159");
            }

            // split input character
            var RowIndex = CharValue >> 8;
            var ColIndex = CharValue & 255;

            // define row if required
            if (CharInfoArray[RowIndex] == null)
            {
                // we know that this block is empty
                if (CharInfoBlockEmpty[RowIndex])
                {
                    return UndefinedCharInfo;
                }

                // get block array
                var Block = FontApi.GetGlyphMetricsApi(CharValue);
                if (Block == null)
                {
                    CharInfoBlockEmpty[RowIndex] = true;
                    return UndefinedCharInfo;
                }

                // save block
                CharInfoArray[RowIndex] = Block;
            }

            // get charater info
            var Info = CharInfoArray[RowIndex][ColIndex];

            // undefined
            if (Info == null)
            {
                return UndefinedCharInfo;
            }

            // character info
            return Info;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font units to user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="Value">Design value</param>
        /// <returns>Design value in user units</returns>
        ////////////////////////////////////////////////////////////////////
        public double FontDesignToUserUnits
        (
            double FontSize,
            int Value
        )
        {
            return Value * FontSize / (DesignHeight * ScaleFactor);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font design units to PDF design units
        /// </summary>
        /// <param name="Value">Font design value</param>
        /// <returns>PDF dictionary value</returns>
        ////////////////////////////////////////////////////////////////////
        public double FontDesignToPdfUnits
        (
            int Value
        )
        {
            return 1000.0 * Value / DesignHeight;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Line spacing in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Line spacing</returns>
        ////////////////////////////////////////////////////////////////////
        public double LineSpacing
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, PdfLineSpacing);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font ascent in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Font ascent</returns>
        ////////////////////////////////////////////////////////////////////
        public double Ascent
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, PdfAscent);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font ascent in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Font ascent plus half of internal leading.</returns>
        ////////////////////////////////////////////////////////////////////
        public double AscentPlusLeading
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, PdfAscent + (PdfLeading + 1) / 2);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font descent in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Font descent</returns>
        ////////////////////////////////////////////////////////////////////
        public double Descent
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, PdfDescent);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Font descent in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Font descent plus half of internal leading.</returns>
        ////////////////////////////////////////////////////////////////////
        public double DescentPlusLeading
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, PdfDescent + PdfLeading / 2);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Capital M height in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Capital M height</returns>
        ////////////////////////////////////////////////////////////////////
        public double CapHeight
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignCapHeight);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Strikeout position in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Strikeout position</returns>
        ////////////////////////////////////////////////////////////////////
        public double StrikeoutPosition
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignStrikeoutPosition);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Strikeout width in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Strikeout line width.</returns>
        ////////////////////////////////////////////////////////////////////
        public double StrikeoutWidth
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignStrikeoutWidth);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Underline position in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Underline position</returns>
        ////////////////////////////////////////////////////////////////////
        public double UnderlinePosition
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignUnderlinePosition);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Underline width in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Underline line width.</returns>
        ////////////////////////////////////////////////////////////////////
        public double UnderlineWidth
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignUnderlineWidth);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Subscript position in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Subscript position</returns>
        ////////////////////////////////////////////////////////////////////
        public double SubscriptPosition
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignSubscriptPosition);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Subscript character size in points
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Subscript font size</returns>
        ////////////////////////////////////////////////////////////////////
        public double SubscriptSize
        (
            double FontSize
        )
        {
            // note: font size is in always points
            return FontSize * DesignSubscriptSize / DesignHeight;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Superscript character position
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Superscript position</returns>
        ////////////////////////////////////////////////////////////////////
        public double SuperscriptPosition
        (
            double FontSize
        )
        {
            return FontDesignToUserUnits(FontSize, DesignSuperscriptPosition);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Superscript character size in points
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <returns>Superscript font size</returns>
        ////////////////////////////////////////////////////////////////////
        public double SuperscriptSize
        (
            double FontSize
        )
        {
            // note: font size is in always points
            return FontSize * DesignSuperscriptSize / DesignHeight;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Character width in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="CharValue">Character code</param>
        /// <returns>Character width</returns>
        ////////////////////////////////////////////////////////////////////
        public double CharWidth
        (
            double FontSize,
            char CharValue
        )
        {
            return FontDesignToUserUnits(FontSize, GetCharInfo(CharValue).DesignWidth);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Character width in user units
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="DrawStyle">Draw style</param>
        /// <param name="CharValue">Character code</param>
        /// <returns>Character width</returns>
        ////////////////////////////////////////////////////////////////////
        public double CharWidth
        (
            double FontSize,
            DrawStyle DrawStyle,
            char CharValue
        )
        {
            // character style is not superscript or subscript
            if ((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0)
            {
                return FontDesignToUserUnits(FontSize, GetCharInfo(CharValue).DesignWidth);
            }

            // superscript
            if ((DrawStyle & DrawStyle.Superscript) != 0)
            {
                return FontDesignToUserUnits(SubscriptSize(FontSize), GetCharInfo(CharValue).DesignWidth);
            }

            // subscript
            return FontDesignToUserUnits(SuperscriptSize(FontSize), GetCharInfo(CharValue).DesignWidth);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Character bounding box in user coordinate units.
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="CharValue">Character</param>
        /// <returns>Bounding box</returns>
        ////////////////////////////////////////////////////////////////////
        public PdfRectangle CharBoundingBox
        (
            double FontSize,
            char CharValue
        )
        {
            // get character info
            var CharInfo = GetCharInfo(CharValue);

            // convert to user coordinate units
            var Factor = FontSize / (DesignHeight * ScaleFactor);
            return new PdfRectangle(Factor * CharInfo.DesignBBoxLeft, Factor * CharInfo.DesignBBoxBottom,
                Factor * CharInfo.DesignBBoxRight, Factor * CharInfo.DesignBBoxTop);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Text width
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="Text">Text</param>
        /// <returns>Width</returns>
        ////////////////////////////////////////////////////////////////////
        public double TextWidth
        (
            double FontSize,
            string Text
        )
        {
            // text width
            var Width = 0;
            foreach (var CharValue in Text)
            {
                Width += GetCharInfo(CharValue).DesignWidth;
            }

            // to user unit of measure
            return FontDesignToUserUnits(FontSize, Width);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Word spacing to stretch text to given width
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="ReqWidth">Required width</param>
        /// <param name="WordSpacing">Output word spacing</param>
        /// <param name="CharSpacing">Output character spacing</param>
        /// <param name="Text">Text</param>
        /// <returns>True-done, False-not done.</returns>
        ////////////////////////////////////////////////////////////////////
        public bool TextFitToWidth
        (
            double FontSize,
            double ReqWidth,
            out double WordSpacing,
            out double CharSpacing,
            string Text
        )
        {
            WordSpacing = 0;
            CharSpacing = 0;
            if (Text == null || Text.Length < 2)
            {
                return false;
            }

            var Width = 0;
            var SpaceCount = 0;
            foreach (var CharValue in Text)
            {
                // character width
                Width += GetCharInfo(CharValue).DesignWidth;

                // space count
                if (CharValue == ' ')
                {
                    SpaceCount++;
                }
            }

            // to user unit of measure
            var UserUnitsWidth = FontDesignToUserUnits(FontSize, Width);

            // extra spacing required
            var ExtraSpace = ReqWidth - UserUnitsWidth;

            // string is too wide
            if (ExtraSpace < -Document.Epsilon)
            {
                return false;
            }

            // string is just right
            if (ExtraSpace < Document.Epsilon)
            {
                return true;
            }

            // String does not have any spacesS
            if (SpaceCount == 0)
            {
                CharSpacing = ExtraSpace / (Text.Length - 1);
                return true;
            }

            // extra space per word
            WordSpacing = ExtraSpace / SpaceCount;

            // extra space is equal or less than one blank
            if (WordSpacing <= FontDesignToUserUnits(FontSize, GetCharInfo(' ').DesignWidth))
            {
                return true;
            }

            // extra space is larger that one blank
            // increase character and word spacing
            CharSpacing = ExtraSpace / (10 * SpaceCount + Text.Length - 1);
            WordSpacing = 10 * CharSpacing;
            return true;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Text bounding box in user coordinate units.
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="Text">Text</param>
        /// <returns>Bounding box</returns>
        ////////////////////////////////////////////////////////////////////
        public PdfRectangle TextBoundingBox
        (
            double FontSize,
            string Text
        )
        {
            if (string.IsNullOrEmpty(Text))
            {
                return null;
            }

            // initialize result box to first character
            var FirstChar = GetCharInfo(Text[0]);
            var Left = FirstChar.DesignBBoxLeft;
            var Bottom = FirstChar.DesignBBoxBottom;
            var Right = FirstChar.DesignBBoxRight;
            var Top = FirstChar.DesignBBoxTop;
            var Width = FirstChar.DesignWidth;

            // more than one character
            if (Text.Length > 1)
            {
                // loop from second character
                for (var Index = 1; Index < Text.Length; Index++)
                {
                    // get bounding box for current character
                    var Info = GetCharInfo(Text[Index]);

                    // update bottom
                    if (Info.DesignBBoxBottom < Bottom)
                    {
                        Bottom = Info.DesignBBoxBottom;
                    }

                    // update top
                    if (Info.DesignBBoxTop > Top)
                    {
                        Top = Info.DesignBBoxTop;
                    }

                    // accumulate width
                    Width += Info.DesignWidth;
                }

                // last character
                var LastChar = GetCharInfo(Text[Text.Length - 1]);
                Right = Width - LastChar.DesignWidth + LastChar.DesignBBoxRight;
            }

            // convert to user coordinate units
            var Factor = FontSize / (DesignHeight * ScaleFactor);
            return new PdfRectangle(Factor * Left, Factor * Bottom, Factor * Right, Factor * Top);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Text Kerning
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Kerning adjustment pairs</returns>
        ////////////////////////////////////////////////////////////////////
        public KerningAdjust[] TextKerning
        (
            string Text
        )
        {
            // string is empty or one character
            if (string.IsNullOrEmpty(Text) || Text.Length == 1)
            {
                return null;
            }

            // find first and last characters of the text
            int First = Text[0];
            int Last = Text[0];
            foreach (var Chr in Text)
            {
                if (Chr < First)
                {
                    First = Chr;
                }
                else if (Chr > Last)
                {
                    Last = Chr;
                }
            }

            // get kerning information
            var KP = FontApi.GetKerningPairsApi(First, Last);

            // no kerning info available for this font or for this range
            if (KP == null)
            {
                return null;
            }

            // prepare a list of kerning adjustments
            var KA = new List<KerningAdjust>();

            // look for pairs with adjustments
            var Ptr1 = 0;
            for (var Ptr2 = 1; Ptr2 < Text.Length; Ptr2++)
            {
                // search for a pair of characters
                var Index = Array.BinarySearch(KP, new WinKerningPair(Text[Ptr2 - 1], Text[Ptr2]));

                // not kerning information for this pair
                if (Index < 0)
                {
                    continue;
                }

                // add kerning adjustment in PDF font units (windows design units divided by windows font design height)
                KA.Add(new KerningAdjust(Text.Substring(Ptr1, Ptr2 - Ptr1),
                    FontDesignToPdfUnits(KP[Index].KernAmount)));

                // adjust pointer
                Ptr1 = Ptr2;
            }

            // list is empty
            if (KA.Count == 0)
            {
                return null;
            }

            // add last
            KA.Add(new KerningAdjust(Text.Substring(Ptr1, Text.Length - Ptr1), 0));

            // exit
            return KA.ToArray();
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Text kerning width
        /// </summary>
        /// <param name="FontSize">Font size</param>
        /// <param name="KerningArray">Kerning array</param>
        /// <returns>Width</returns>
        ////////////////////////////////////////////////////////////////////
        public double TextKerningWidth
        (
            double FontSize, // in points
            KerningAdjust[] KerningArray
        )
        {
            // text is null or empty
            if (KerningArray == null || KerningArray.Length == 0)
            {
                return 0;
            }

            // total width
            double Width = 0;

            // draw text
            var LastStr = KerningArray.Length - 1;
            for (var Index = 0; Index < LastStr; Index++)
            {
                var KA = KerningArray[Index];
                Width += TextWidth(FontSize, KA.Text) + KA.Adjust * FontSize / (1000.0 * ScaleFactor);
            }

            // last string
            Width += TextWidth(FontSize, KerningArray[LastStr].Text);
            return Width;
        }

        ////////////////////////////////////////////////////////////////////
        // Write object to PDF file
        ////////////////////////////////////////////////////////////////////

        internal override void WriteObjectToPdfFile()
        {
            // pdf font name
            var PdfFontName = new StringBuilder("/");

            // for embedded font add 6 alpha characters prefix
            if (EmbeddedFont)
            {
                PdfFontName.Append("PFWAAA+");
                var Ptr1 = 6;
                for (var Ptr2 = ResourceCode.Length - 1; Ptr2 >= 0 && char.IsDigit(ResourceCode[Ptr2]); Ptr2--)
                {
                    PdfFontName[Ptr1--] = (char) (ResourceCode[Ptr2] + ('A' - '0'));
                }
            }

            // PDF readers are not happy with space in font name
            PdfFontName.Append(FontFamily.Name.Replace(" ", "#20"));

            // font name
            if ((DesignFont.Style & FontStyle.Bold) != 0)
            {
                if ((DesignFont.Style & FontStyle.Italic) != 0)
                {
                    PdfFontName.Append(",BoldItalic");
                }
                else
                {
                    PdfFontName.Append(",Bold");
                }
            }
            else if ((DesignFont.Style & FontStyle.Italic) != 0)
            {
                PdfFontName.Append(",Italic");
            }

            // we have one byte characters 
            if (FontResCodeUsed)
            {
                CharCodeToPdfFile(PdfFontName.ToString());
            }

            // we have two bytes characters 
            if (FontResGlyphUsed)
            {
                GlyphIndexToPdfFile(PdfFontName.ToString());
            }

            // dispose resources
            Dispose();
        }

        ////////////////////////////////////////////////////////////////////
        // Write character code oject to PDF file
        ////////////////////////////////////////////////////////////////////

        internal void CharCodeToPdfFile
        (
            string PdfFontName
        )
        {
            // look for first and last character
            int FirstChar;
            int LastChar;
            for (FirstChar = 0;
                FirstChar < 256 && (CharInfoArray[0][FirstChar] == null || !CharInfoArray[0][FirstChar].ActiveChar);
                FirstChar++)
            {
                ;
            }

            if (FirstChar == 256)
            {
                return;
            }

            for (LastChar = 255;
                CharInfoArray[0][LastChar] == null || !CharInfoArray[0][LastChar].ActiveChar;
                LastChar--)
            {
                ;
            }

            // add items to dictionary
            Dictionary.Add("/Subtype", "/TrueType");
            Dictionary.Add("/BaseFont", PdfFontName);

            // add first and last characters
            Dictionary.AddInteger("/FirstChar", FirstChar);
            Dictionary.AddInteger("/LastChar", LastChar);

            // create font descriptor
            Dictionary.AddIndirectReference("/FontDescriptor",
                CharCodeFontDescriptor(PdfFontName, FirstChar, LastChar));

            // create width object array
            Dictionary.AddIndirectReference("/Widths", CharCodeFontWidthArray(FirstChar, LastChar));

            // set encoding
            Dictionary.Add("/Encoding", "/WinAnsiEncoding");

            // call base write PdfObject to file method
            base.WriteObjectToPdfFile();

            // exit
        }

        ////////////////////////////////////////////////////////////////////
        // Character code font descriptor
        ////////////////////////////////////////////////////////////////////

        private PdfObject CharCodeFontDescriptor
        (
            string PdfFontName,
            int FirstChar,
            int LastChar
        )
        {
            // create font descriptor
            var FontDescriptor = FontDescriptorCommon(PdfFontName);

            // build bounding box and calculate maximum width
            var Left = int.MaxValue;
            var Bottom = int.MaxValue;
            var Right = int.MinValue;
            var Top = int.MinValue;
            var MaxWidth = int.MinValue;
            for (var Index = FirstChar; Index <= LastChar; Index++)
            {
                // shortcut
                var CharInfo = CharInfoArray[0][Index];

                // not used
                if (CharInfo == null || !CharInfo.ActiveChar)
                {
                    continue;
                }

                // bounding box
                if (CharInfo.DesignBBoxLeft < Left)
                {
                    Left = CharInfo.DesignBBoxLeft;
                }

                if (CharInfo.DesignBBoxBottom < Bottom)
                {
                    Bottom = CharInfo.DesignBBoxBottom;
                }

                if (CharInfo.DesignBBoxRight > Right)
                {
                    Right = CharInfo.DesignBBoxRight;
                }

                if (CharInfo.DesignBBoxTop > Top)
                {
                    Top = CharInfo.DesignBBoxTop;
                }

                // max width
                if (CharInfo.DesignWidth > MaxWidth)
                {
                    MaxWidth = CharInfo.DesignWidth;
                }
            }

            // add to font descriptor array
            FontDescriptor.Dictionary.AddReal("/MaxWidth", FontDesignToPdfUnits(MaxWidth));
            FontDescriptor.Dictionary.AddFormat("/FontBBox", "[{0} {1} {2} {3}]",
                FontDesignToPdfUnits(Left), FontDesignToPdfUnits(Bottom), FontDesignToPdfUnits(Right),
                FontDesignToPdfUnits(Top));

            // create font file
            if (EmbeddedFont)
            {
                // create font file stream
                var EmbeddedFontObj = new PdfFontFile(this, FirstChar, LastChar);

                // add link to font object
                FontDescriptor.Dictionary.AddIndirectReference("/FontFile2", EmbeddedFontObj);
            }

            // output font descriptor
            FontDescriptor.WriteObjectToPdfFile();

            // return with reference to font descriptor
            return FontDescriptor;
        }

        ////////////////////////////////////////////////////////////////////
        // Character code font descriptor
        ////////////////////////////////////////////////////////////////////

        private PdfObject FontDescriptorCommon
        (
            string PdfFontName
        )
        {
            // create font descriptor
            var FontDescriptor = new PdfObject(Document, ObjectType.Dictionary, "/FontDescriptor");

            // font descriptor dictionary
            FontDescriptor.Dictionary.Add("/FontName", PdfFontName); // must be the same as BaseFont above
            FontDescriptor.Dictionary.AddInteger("/Flags", (int) FontFlags);
            FontDescriptor.Dictionary.AddReal("/ItalicAngle", DesignItalicAngle / 10.0);
            FontDescriptor.Dictionary.AddInteger("/FontWeight", DesignFontWeight);
            FontDescriptor.Dictionary.AddReal("/Leading", FontDesignToPdfUnits(PdfLeading));
            FontDescriptor.Dictionary.AddReal("/Ascent", FontDesignToPdfUnits(PdfAscent));
            FontDescriptor.Dictionary.AddReal("/Descent", FontDesignToPdfUnits(-PdfDescent));

            // alphabetic (non symbolic) fonts
            if ((FontFlags & PdfFontFlags.Symbolic) == 0)
            {
                // character info for small x
                var CharInfo = FontApi.GetGlyphMetricsApiByCode('x');
                FontDescriptor.Dictionary.AddReal("/XHeight", FontDesignToPdfUnits(CharInfo.DesignBBoxTop));
                FontDescriptor.Dictionary.AddReal("/AvgWidth", FontDesignToPdfUnits(CharInfo.DesignWidth));

                // character info for capital M
                CharInfo = FontApi.GetGlyphMetricsApiByCode('M');
                FontDescriptor.Dictionary.AddReal("/CapHeight", FontDesignToPdfUnits(CharInfo.DesignBBoxTop));
                FontDescriptor.Dictionary.AddReal("/StemV", StemV());
            }

            // return with reference to font descriptor
            return FontDescriptor;
        }

        ////////////////////////////////////////////////////////////////////
        // Character code font width array
        ////////////////////////////////////////////////////////////////////

        internal PdfObject CharCodeFontWidthArray
        (
            int FirstChar,
            int LastChar
        )
        {
            // create width object array
            var FontWidthArray = new PdfObject(Document, ObjectType.Other);

            FontWidthArray.ObjectValueList.Add((byte) '[');

            var EolLength = 100;
            for (var Index = FirstChar; Index <= LastChar; Index++)
            {
                // shortcut
                var CharInfo = CharInfoArray[0][Index];

                // add new line after a 100 character block
                if (FontWidthArray.ObjectValueList.Count > EolLength)
                {
                    FontWidthArray.ObjectValueList.Add((byte) '\n');
                    EolLength = FontWidthArray.ObjectValueList.Count + 100;
                }

                // not used
                if (CharInfo == null || !CharInfo.ActiveChar)
                {
                    FontWidthArray.ObjectValueAppend("0 ");
                }
                // used
                else
                    // add width to width array
                {
                    FontWidthArray.ObjectValueFormat("{0} ", (float) FontDesignToPdfUnits(CharInfo.DesignWidth));
                }
            }

            // terminate width array
            FontWidthArray.ObjectValueList[FontWidthArray.ObjectValueList.Count - 1] = (byte) ']';

            // output object to pdf file
            FontWidthArray.WriteObjectToPdfFile();

            // return reference to font width
            return FontWidthArray;
        }

        ////////////////////////////////////////////////////////////////////
        // Write glyph index font oject to PDF file
        ////////////////////////////////////////////////////////////////////

        internal void GlyphIndexToPdfFile
        (
            string PdfFontName
        )
        {
            // add items to dictionary
            GlyphIndexFont.Dictionary.Add("/Subtype", "/Type0");
            GlyphIndexFont.Dictionary.Add("/BaseFont", PdfFontName);
            GlyphIndexFont.Dictionary.Add("/Encoding", "/Identity-H");

            // create to unicode
            GlyphIndexFont.Dictionary.AddIndirectReference("/ToUnicode", GlyphIndexToUnicode());

            // create descended fonts object
            var DescendedFonts = new PdfObject(Document, ObjectType.Dictionary, "/Font");
            GlyphIndexFont.Dictionary.AddFormat("/DescendantFonts", "[{0} 0 R]", DescendedFonts.ObjectNumber);

            // add items to dictionary
            DescendedFonts.Dictionary.Add("/Subtype", "/CIDFontType2");
            DescendedFonts.Dictionary.Add("/BaseFont", PdfFontName);

            // add CIDSystem info
            var CIDSystemInfo = new PdfDictionary(DescendedFonts);
            DescendedFonts.Dictionary.AddDictionary("/CIDSystemInfo", CIDSystemInfo);
            CIDSystemInfo.AddPdfString("/Ordering", "Identity");
            CIDSystemInfo.AddPdfString("/Registry", "Adobe");
            CIDSystemInfo.AddInteger("/Supplement", 0);

            // create font descriptor
            DescendedFonts.Dictionary.AddIndirectReference("/FontDescriptor", GlyphIndexFontDescriptor(PdfFontName));

            // create character width array
            DescendedFonts.Dictionary.AddIndirectReference("/W", GlyphIndexWidthArray());

            // send glyph index font to output file
            GlyphIndexFont.WriteObjectToPdfFile();

            // exit
        }

        ////////////////////////////////////////////////////////////////////
        // Glyph index font descriptor
        ////////////////////////////////////////////////////////////////////

        private PdfObject GlyphIndexFontDescriptor
        (
            string PdfFontName
        )
        {
            // create font descriptor
            var FontDescriptor = FontDescriptorCommon(PdfFontName);

            // build bounding box and calculate maximum width
            var Undef = UndefinedCharInfo.ActiveChar;
            var Left = Undef ? UndefinedCharInfo.DesignBBoxLeft : int.MaxValue;
            var Bottom = Undef ? UndefinedCharInfo.DesignBBoxBottom : int.MaxValue;
            var Right = Undef ? UndefinedCharInfo.DesignBBoxRight : int.MinValue;
            var Top = Undef ? UndefinedCharInfo.DesignBBoxTop : int.MinValue;
            var MaxWidth = Undef ? UndefinedCharInfo.DesignWidth : int.MinValue;

            // look for all used characters
            for (var Row = 1; Row < 256; Row++)
            {
                var OneRow = CharInfoArray[Row];
                if (OneRow == null)
                {
                    continue;
                }

                for (var Col = 0; Col < 256; Col++)
                {
                    var CharInfo = OneRow[Col];
                    if (CharInfo == null || !CharInfo.ActiveChar)
                    {
                        continue;
                    }

                    // bounding box
                    if (CharInfo.DesignBBoxLeft < Left)
                    {
                        Left = CharInfo.DesignBBoxLeft;
                    }

                    if (CharInfo.DesignBBoxBottom < Bottom)
                    {
                        Bottom = CharInfo.DesignBBoxBottom;
                    }

                    if (CharInfo.DesignBBoxRight > Right)
                    {
                        Right = CharInfo.DesignBBoxRight;
                    }

                    if (CharInfo.DesignBBoxTop > Top)
                    {
                        Top = CharInfo.DesignBBoxTop;
                    }

                    // max width
                    if (CharInfo.DesignWidth > MaxWidth)
                    {
                        MaxWidth = CharInfo.DesignWidth;
                    }
                }
            }

            // add to font descriptor array
            FontDescriptor.Dictionary.AddReal("/MaxWidth", FontDesignToPdfUnits(MaxWidth));
            FontDescriptor.Dictionary.AddFormat("/FontBBox", "[{0} {1} {2} {3}]",
                FontDesignToPdfUnits(Left), FontDesignToPdfUnits(Bottom), FontDesignToPdfUnits(Right),
                FontDesignToPdfUnits(Top));

            // create font file
            if (EmbeddedFont)
            {
                // create font file stream
                var EmbeddedFontObj = new PdfFontFile(this, 0, 0);

                // add link to font object
                FontDescriptor.Dictionary.AddIndirectReference("/FontFile2", EmbeddedFontObj);
            }

            // send font descriptor to output file
            FontDescriptor.WriteObjectToPdfFile();

            // return reference to font descriptor
            return FontDescriptor;
        }

        ////////////////////////////////////////////////////////////////////
        // Glyph index to Unicode stream
        ////////////////////////////////////////////////////////////////////

        private PdfObject GlyphIndexToUnicode()
        {
            var Header = "/CIDInit /ProcSet findresource begin\n" +
                         "14 dict begin\n" +
                         "begincmap\n" +
                         "/CIDSystemInfo<</Registry(Adobe)/Ordering (UCS)/Supplement 0>>def\n" +
                         "/CMapName/Adobe-Identity-UCS def\n" +
                         "/CMapType 2 def\n" +
                         "1 begincodespacerange\n" +
                         "<0000><FFFF>\n" +
                         "endcodespacerange\n";

            var Trailer = "endcmap\n" +
                          "CMapName currentdict /CMap defineresource pop\n" +
                          "end\n" +
                          "end\n";

            // create array of glyph index to character code
            var RangeArray = new List<UnicodeRange>();

            // add one entry for undefined character
            if (UndefinedCharInfo.ActiveChar)
            {
                RangeArray.Add(new UnicodeRange(0, 0));
            }

            // look for all used characters
            for (var Row = 1; Row < 256; Row++)
            {
                var OneRow = CharInfoArray[Row];
                if (OneRow == null)
                {
                    continue;
                }

                for (var Col = 0; Col < 256; Col++)
                {
                    var CharInfo = OneRow[Col];
                    if (CharInfo == null || !CharInfo.ActiveChar)
                    {
                        continue;
                    }

                    RangeArray.Add(new UnicodeRange(CharInfo.NewGlyphIndex, CharInfo.CharCode));
                }
            }

            // sort by glyph index
            RangeArray.Sort();

            // look for ranges
            var Last = RangeArray[0];
            var Run = 1;
            for (var Index = 1; Index < RangeArray.Count;)
            {
                var Next = RangeArray[Index];

                // we have duplicate glyph index (i.e. space and non-breaking space)
                // remove the higher char code
                if (Next.GlyphStart == Last.GlyphStart)
                {
                    if (Next.CharCode < Last.CharCode)
                    {
                        Last.CharCode = Next.CharCode;
                    }

                    RangeArray.RemoveAt(Index);
                    continue;
                }

                // range is found
                if (Next.GlyphStart == Last.GlyphEnd + 1 && Next.CharCode == Last.CharCode + Run)
                {
                    Last.GlyphEnd++;
                    Run++;
                    RangeArray.RemoveAt(Index);
                    continue;
                }

                // start new range
                Last = Next;
                Run = 1;
                Index++;
            }

            // create ToUnicode stream object
            var ToUnicode = new PdfObject(Document, ObjectType.Stream);

            // ouput header
            ToUnicode.ObjectValueAppend(Header);

            // output ranges
            Run = 0;
            for (var Index = 0; Index < RangeArray.Count; Index++)
            {
                if (Run == 0)
                {
                    if (Index != 0)
                    {
                        ToUnicode.ObjectValueAppend("endbfrange\n");
                    }

                    Run = Math.Min(100, RangeArray.Count - Index);
                    ToUnicode.ObjectValueFormat("{0} beginbfrange\n", Run);
                }

                Run--;
                var Range = RangeArray[Index];
                var RangeStr = string.Format("<{0:x4}><{1:x4}><{2:x4}>\n", Range.GlyphStart, Range.GlyphEnd,
                    Range.CharCode);
                foreach (var Chr in RangeStr)
                {
                    ToUnicode.ObjectValueList.Add((byte) Chr);
                }
            }

            if (RangeArray.Count > 0)
            {
                ToUnicode.ObjectValueAppend("endbfrange\n");
            }

            // output trailer
            ToUnicode.ObjectValueAppend(Trailer);

            // send to output file
            ToUnicode.WriteObjectToPdfFile();

            // return reference to glyph index to unicode translation
            return ToUnicode;
        }

        ////////////////////////////////////////////////////////////////////
        // Glyph index to width array
        ////////////////////////////////////////////////////////////////////

        private PdfObject GlyphIndexWidthArray()
        {
            // create array of glyph index to character code
            var WidthArray = new List<GlyphWidth>();

            // add undefined glyph
            if (UndefinedCharInfo.ActiveChar)
            {
                WidthArray.Add(new GlyphWidth(0, UndefinedCharInfo.DesignWidth));
            }

            // look for all used characters
            for (var Row = 1; Row < 256; Row++)
            {
                var OneRow = CharInfoArray[Row];
                if (OneRow == null)
                {
                    continue;
                }

                for (var Col = 0; Col < 256; Col++)
                {
                    var CharInfo = OneRow[Col];
                    if (CharInfo == null || !CharInfo.ActiveChar)
                    {
                        continue;
                    }

                    WidthArray.Add(new GlyphWidth(CharInfo.NewGlyphIndex, CharInfo.DesignWidth));
                }
            }

            // sort by glyph index
            WidthArray.Sort();

            // create ToUnicode stream object
            var GlyphWidthArray = new PdfObject(Document, ObjectType.Other);

            // ouput header
            GlyphWidthArray.ObjectValueList.Add((byte) '[');

            // output ranges
            var LastIndex = WidthArray[0].GlyphIndex;
            double LastWidth = WidthArray[0].Width;
            ;
            var StartIndex = 0;
            var StartWidth = 0;
            GlyphWidth Item = null;
            for (var Index = 1; Index <= WidthArray.Count; Index++)
            {
                if (Index < WidthArray.Count)
                {
                    // shortcut
                    Item = WidthArray[Index];

                    // it is possible to have two entries with the save glyph index
                    if (Item.GlyphIndex == LastIndex)
                    {
                        WidthArray.RemoveAt(Index);
                        Index--;
                        continue;
                    }

                    // two consecutive glyphs 
                    if (Item.GlyphIndex == LastIndex + 1)
                    {
                        // two consecutive glyphs have the same width
                        if (Item.Width == LastWidth)
                        {
                            LastIndex++;
                            continue;
                        }

                        // width is not the same and the last group is too small
                        if (Index - StartWidth < 3)
                        {
                            StartWidth = Index;
                            LastIndex++;
                            continue;
                        }
                    }
                }

                // either glyphs are not consecutives
                // or 3 or more glyphs have the same width
                // for first case if there are less than 3 equal width eliminate equal block
                if (Index - StartWidth < 3)
                {
                    StartWidth = Index;
                }

                // output GlyphIndex [W W W] between StartIndex and StartWidth
                if (StartWidth > StartIndex)
                {
                    if (StartIndex != 0)
                    {
                        GlyphWidthArray.ObjectValueList.Add((byte) '\n');
                    }

                    GlyphWidthArray.ObjectValueFormat("{0}[{1}", WidthArray[StartIndex].GlyphIndex,
                        (float) FontDesignToPdfUnits(WidthArray[StartIndex].Width));
                    for (var Ptr = StartIndex + 1; Ptr < StartWidth; Ptr++)
                    {
                        GlyphWidthArray.ObjectValueList.Add((Ptr - StartIndex) % 12 == 11 ? (byte) '\n' : (byte) ' ');
                        GlyphWidthArray.ObjectValueFormat("{0}", (float) FontDesignToPdfUnits(WidthArray[Ptr].Width));
                    }

                    GlyphWidthArray.ObjectValueList.Add((byte) ']');
                }

                if (Index > StartWidth)
                {
                    if (StartWidth != 0)
                    {
                        GlyphWidthArray.ObjectValueList.Add((byte) '\n');
                    }

                    // output C(StartWidth) C(Index - 1) W
                    GlyphWidthArray.ObjectValueFormat("{0} {1} {2}",
                        WidthArray[StartWidth].GlyphIndex, WidthArray[Index - 1].GlyphIndex,
                        (float) FontDesignToPdfUnits(WidthArray[StartWidth].Width));
                }

                // exit the loop
                if (Index == WidthArray.Count)
                {
                    break;
                }

                // reset block
                LastIndex = Item.GlyphIndex;
                LastWidth = Item.Width;
                StartIndex = Index;
                StartWidth = Index;
            }

            // terminate width array
            GlyphWidthArray.ObjectValueList.Add((byte) ']');

            // send to output file
            GlyphWidthArray.WriteObjectToPdfFile();

            // return reference to glyph width array
            return GlyphWidthArray;
        }

        ////////////////////////////////////////////////////////////////////
        // Calculate StemV from capital I
        ////////////////////////////////////////////////////////////////////

        private double StemV()
        {
            // convert I to graphics path
            var GP = new GraphicsPath();
            GP.AddString("I", FontFamily, (int) DesignFont.Style, 1000, Point.Empty, StringFormat.GenericDefault);

            // center point of I
            var Rect = GP.GetBounds();
            var X = (int) ((Rect.Left + Rect.Right) / 2);
            var Y = (int) ((Rect.Bottom + Rect.Top) / 2);

            // bounding box converted to integer
            var LeftLimit = (int) Rect.Left;
            var RightLimit = (int) Rect.Right;

            // make sure we are within the I
            if (!GP.IsVisible(X, Y))
            {
                return RightLimit - LeftLimit;
            }

            // look for left edge
            int Left;
            for (Left = X - 1; Left >= LeftLimit && GP.IsVisible(Left, Y); Left--)
            {
                ;
            }

            // look for right edge
            int Right;
            for (Right = X + 1; Right < RightLimit && GP.IsVisible(Right, Y); Right++)
            {
                ;
            }

            // exit
            return Right - Left - 1;
        }
    }

    ////////////////////////////////////////////////////////////////////
    // Support class for glyph index to unicode translation
    ////////////////////////////////////////////////////////////////////

    internal class UnicodeRange : IComparable<UnicodeRange>
    {
        internal int CharCode;
        internal int GlyphEnd;
        internal int GlyphStart;

        internal UnicodeRange
        (
            int GlyphIndex,
            int CharCode
        )
        {
            GlyphStart = GlyphIndex;
            GlyphEnd = GlyphIndex;
            this.CharCode = CharCode;
        }

        public int CompareTo
        (
            UnicodeRange Other
        )
        {
            return GlyphStart - Other.GlyphStart;
        }
    }

    ////////////////////////////////////////////////////////////////////
    // Support class for glyph index to unicode translation
    ////////////////////////////////////////////////////////////////////

    internal class GlyphWidth : IComparable<GlyphWidth>
    {
        internal int GlyphIndex;
        internal int Width;

        internal GlyphWidth
        (
            int GlyphIndex,
            int Width
        )
        {
            this.GlyphIndex = GlyphIndex;
            this.Width = Width;
        }

        public int CompareTo
        (
            GlyphWidth Other
        )
        {
            return GlyphIndex - Other.GlyphIndex;
        }
    }
}