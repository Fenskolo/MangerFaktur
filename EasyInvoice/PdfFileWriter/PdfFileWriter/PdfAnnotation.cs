/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfAnnotation
//	PDF Annotation class. 
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

using System.Collections.Generic;

namespace PdfFileWriter
{
    /// <summary>
    ///     PDF Annotation class
    /// </summary>
    public class PdfAnnotation : PdfObject
    {
        internal AnnotAction AnnotAction;
        internal PdfPage AnnotPage;
        internal PdfRectangle AnnotRect;

        /// <summary>
        ///     PdfAnnotation constructor
        /// </summary>
        /// <param name="AnnotPage">Page object</param>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <param name="AnnotAction">Annotation action</param>
        internal PdfAnnotation
        (
            PdfPage AnnotPage,
            PdfRectangle AnnotRect,
            AnnotAction AnnotAction
        ) : base(AnnotPage.Document)
        {
            // save arguments
            this.AnnotPage = AnnotPage;
            this.AnnotRect = AnnotRect;
            this.AnnotAction = AnnotAction;

            // annotation subtype
            Dictionary.Add("/Subtype", AnnotAction.Subtype);

            // area rectangle on the page
            Dictionary.AddRectangle("/Rect", AnnotRect);

            // annotation flags. value of 4 = Bit position 3 print
            Dictionary.Add("/F", "4");

            // border style dictionary. If /BS with /W 0 is not specified, the annotation will have a thin border
            Dictionary.Add("/BS", "<</W 0>>");

            // web link
            if (AnnotAction.GetType() == typeof(AnnotWebLink))
            {
                Dictionary.AddIndirectReference("/A",
                    PdfWebLink.AddWebLink(Document, ((AnnotWebLink) AnnotAction).WebLinkStr));
            }

            // jump to destination
            else if (AnnotAction.GetType() == typeof(AnnotLinkAction))
            {
                if (Document.LinkAnnotArray == null) Document.LinkAnnotArray = new List<PdfAnnotation>();

                Document.LinkAnnotArray.Add(this);
            }

            // display video or play sound
            else if (AnnotAction.GetType() == typeof(AnnotDisplayMedia))
            {
                var DisplayMedia = ((AnnotDisplayMedia) AnnotAction).DisplayMedia;

                // action reference dictionary
                Dictionary.AddIndirectReference("/A", DisplayMedia);

                // add page reference
                Dictionary.AddIndirectReference("/P", AnnotPage);

                // add annotation reference
                DisplayMedia.Dictionary.AddIndirectReference("/AN", this);
            }

            // file attachment
            else if (AnnotAction.GetType() == typeof(AnnotFileAttachment))
            {
                // add file attachment reference
                var File = (AnnotFileAttachment) AnnotAction;
                Dictionary.AddIndirectReference("/FS", File.EmbeddedFile);

                if (File.Icon != FileAttachIcon.NoIcon)
                {
                    // icon
                    Dictionary.AddName("/Name", File.Icon.ToString());
                }
                else
                {
                    // no icon
                    var XObject = new PdfXObject(Document, AnnotRect.Right, AnnotRect.Top);
                    Dictionary.AddFormat("/AP", "<</N {0} 0 R>>", XObject.ObjectNumber);
                }
            }

            // add annotation object to page dictionary
            var KeyValue = AnnotPage.Dictionary.GetValue("/Annots");
            if (KeyValue == null)
                AnnotPage.Dictionary.AddFormat("/Annots", "[{0} 0 R]", ObjectNumber);

            else
                AnnotPage.Dictionary.Add("/Annots",
                    ((string) KeyValue.Value).Replace("]", string.Format(" {0} 0 R]", ObjectNumber)));

            // exit
        }

        /// <summary>
        ///     Gets a copy of the annotation rectangle
        /// </summary>
        public PdfRectangle AnnotationRect => new PdfRectangle(AnnotRect);

        /// <summary>
        ///     Activate annotation when page becomes visible.
        /// </summary>
        /// <param name="Activate">Activate or not-activate annotation.</param>
        public void ActivateActionWhenPageIsVisible
        (
            bool Activate
        )
        {
            // applicable to screen action
            if (AnnotAction.GetType() == typeof(AnnotDisplayMedia))
            {
                // play video when page becomes visible
                if (Activate)
                    Dictionary.AddFormat("/AA", "<</PV {0} 0 R>>",
                        ((AnnotDisplayMedia) AnnotAction).DisplayMedia.ObjectNumber);
                else
                    Dictionary.Remove("/AA");
            }
        }

        /// <summary>
        ///     Display border around annotation rectangle.
        /// </summary>
        /// <param name="BorderWidth">Border width</param>
        public void DisplayBorder
        (
            double BorderWidth
        )
        {
            // see page 611 section 8.4
            Dictionary.AddFormat("/BS", "<</W {0}>>", ToPt(BorderWidth));
        }

        /// <summary>
        ///     Annotation rectangle appearance
        /// </summary>
        /// <param name="AppearanceDixtionary">PDF X Object</param>
        public void Appearance
        (
            PdfXObject AppearanceDixtionary
        )
        {
            Dictionary.AddFormat("/AP", "<</N {0} 0 R>>", AppearanceDixtionary.ObjectNumber);
        }
    }
}