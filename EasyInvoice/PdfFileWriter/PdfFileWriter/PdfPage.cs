/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfPage
//	PDF page class. An indirect PDF object.
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
using System.Text;

namespace PdfFileWriter
{
    /// <summary>
    ///     PDF page class
    /// </summary>
    /// <remarks>
    ///     PDF page class represent one page in the document.
    /// </remarks>
    public class PdfPage : PdfObject
    {
        internal List<PdfContents> ContentsArray;
        internal double Height; // in points
        internal double Width; // in points

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <remarks>
        ///     Page size is taken from PdfDocument
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
        (
            PdfDocument Document
        ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            Width = Document.PageSize.Width;
            Height = Document.PageSize.Height;
            ConstructorHelper();
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="PageSize">Paper size for this page</param>
        /// <remarks>
        ///     PageSize override the default page size
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
        (
            PdfDocument Document,
            SizeD PageSize
        ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            Width = ScaleFactor * PageSize.Width;
            Height = ScaleFactor * PageSize.Height;
            ConstructorHelper();
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="PaperType">Paper type</param>
        /// <param name="Landscape">If Lanscape is true, width and height are swapped.</param>
        /// <remarks>
        ///     PaperType and orientation override the default page size.
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
        (
            PdfDocument Document,
            PaperType PaperType,
            bool Landscape
        ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            // get standard paper size
            Width = PdfDocument.PaperTypeSize[(int) PaperType].Width;
            Height = PdfDocument.PaperTypeSize[(int) PaperType].Height;

            // for landscape swap width and height
            if (Landscape)
            {
                var Temp = Width;
                Width = Height;
                Height = Temp;
            }

            // exit
            ConstructorHelper();
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="Width">Page width</param>
        /// <param name="Height">Page height</param>
        /// <remarks>
        ///     Width and Height override the default page size
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
        (
            PdfDocument Document,
            double Width,
            double Height
        ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            this.Width = ScaleFactor * Width;
            this.Height = ScaleFactor * Height;
            ConstructorHelper();
        }

        /// <summary>
        ///     Clone Constructor
        /// </summary>
        /// <param name="Page">Existing page object</param>
        public PdfPage
        (
            PdfPage Page
        ) : base(Page.Document, ObjectType.Dictionary, "/Page")
        {
            Width = Page.Width;
            Height = Page.Height;
            ConstructorHelper();
        }

        ////////////////////////////////////////////////////////////////////
        // Constructor common method
        ////////////////////////////////////////////////////////////////////

        private void ConstructorHelper()
        {
            // add page to parent array of pages
            Document.PageArray.Add(this);

            // link page to parent
            Dictionary.AddIndirectReference("/Parent", Document.PagesObject);

            // add page size in points
            Dictionary.AddFormat("/MediaBox", "[0 0 {0} {1}]", Round(Width), Round(Height));

            // exit
        }

        /// <summary>
        ///     Page size
        /// </summary>
        /// <returns>Page size</returns>
        /// <remarks>
        ///     Page size in user units of measure. If Width is less than height
        ///     orientation is portrait. Otherwise orientation is landscape.
        /// </remarks>
        public SizeD PageSize()
        {
            return new SizeD(Width / ScaleFactor, Height / ScaleFactor);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Add existing contents to page
        /// </summary>
        /// <param name="Contents">Contents object</param>
        ////////////////////////////////////////////////////////////////////
        public void AddContents
        (
            PdfContents Contents
        )
        {
            // set page contents flag
            Contents.PageContents = true;

            // add content to content array
            if (ContentsArray == null)
            {
                ContentsArray = new List<PdfContents>();
            }

            ContentsArray.Add(Contents);

            // exit
        }

        /// <summary>
        ///     Gets the current contents of this page
        /// </summary>
        /// <returns>Page's current contents</returns>
        public PdfContents GetCurrentContents()
        {
            return ContentsArray == null || ContentsArray.Count == 0 ? null : ContentsArray[ContentsArray.Count - 1];
        }

        /// <summary>
        ///     Add annotation action
        /// </summary>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <param name="AnnotAction">Annotation action derived class</param>
        public void AddAnnotation
        (
            PdfRectangle AnnotRect,
            AnnotAction AnnotAction
        )
        {
            if (AnnotAction.GetType() == typeof(AnnotLinkAction))
            {
                AddLinkAction(((AnnotLinkAction) AnnotAction).LocMarkerName, AnnotRect);
            }
            else
            {
                new PdfAnnotation(this, AnnotRect, AnnotAction);
            }
        }

        internal void AddAnnotInternal
        (
            PdfRectangle AnnotRect,
            AnnotAction AnnotAction
        )
        {
            if (AnnotAction.GetType() == typeof(AnnotLinkAction))
            {
                AddLinkAction(((AnnotLinkAction) AnnotAction).LocMarkerName, AnnotRect);
            }
            else
            {
                if (AnnotAction.GetType() == typeof(AnnotFileAttachment))
                {
                    ((AnnotFileAttachment) AnnotAction).Icon = FileAttachIcon.NoIcon;
                }

                new PdfAnnotation(this, AnnotRect, AnnotAction);
            }
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Add weblink to this page
        /// </summary>
        /// <param name="LeftAbsPos">Left position of weblink area</param>
        /// <param name="BottomAbsPos">Bottom position of weblink area</param>
        /// <param name="RightAbsPos">Right position of weblink area</param>
        /// <param name="TopAbsPos">Top position of weblink area</param>
        /// <param name="WebLinkStr">Hyperlink string</param>
        /// <returns>PdfAnnotation object</returns>
        /// <remarks>
        ///     <para>
        ///         The four position arguments are in relation to the
        ///         bottom left corner of the paper.
        ///         If web link is empty, ignore this call.
        ///     </para>
        ///     <para>
        ///         For more information go to
        ///         <a
        ///             href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#WeblinkSupport">
        ///             2.7
        ///             Web Link Support
        ///         </a>
        ///     </para>
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfAnnotation AddWebLink
        (
            double LeftAbsPos,
            double BottomAbsPos,
            double RightAbsPos,
            double TopAbsPos,
            string WebLinkStr
        )
        {
            if (string.IsNullOrWhiteSpace(WebLinkStr))
            {
                return null;
            }

            return AddWebLink(new PdfRectangle(LeftAbsPos, BottomAbsPos, RightAbsPos, TopAbsPos), WebLinkStr);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Add weblink to this page
        /// </summary>
        /// <param name="AnnotRect">Weblink area</param>
        /// <param name="WebLinkStr">Hyperlink string</param>
        /// <returns>PdfAnnotation object</returns>
        /// <remarks>
        ///     <para>
        ///         The four position arguments are in relation to the
        ///         bottom left corner of the paper.
        ///         If web link is empty, ignore this call.
        ///     </para>
        ///     <para>
        ///         For more information go to
        ///         <a
        ///             href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#WeblinkSupport">
        ///             2.7
        ///             Web Link Support
        ///         </a>
        ///     </para>
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfAnnotation AddWebLink
        (
            PdfRectangle AnnotRect,
            string WebLinkStr
        )
        {
            if (string.IsNullOrWhiteSpace(WebLinkStr))
            {
                return null;
            }

            return new PdfAnnotation(this, AnnotRect, new AnnotWebLink(WebLinkStr));
        }

        /// <summary>
        ///     Add location marker
        /// </summary>
        /// <param name="LocMarkerName">Location marker name</param>
        /// <param name="Scope">Location marker scope</param>
        /// <param name="FitArg">PDF reader display control</param>
        /// <param name="SideArg">Optional dimensions for FitArg control</param>
        public void AddLocationMarker
        (
            string LocMarkerName,
            LocMarkerScope Scope,
            DestFit FitArg,
            params double[] SideArg
        )
        {
            LocationMarker.Create(LocMarkerName, this, Scope, FitArg, SideArg);
        }

        /// <summary>
        ///     Add go to action
        /// </summary>
        /// <param name="LocMarkerName">Destination name</param>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <returns>PdfAnnotation object</returns>
        public PdfAnnotation AddLinkAction
        (
            string LocMarkerName,
            PdfRectangle AnnotRect
        )
        {
            return new PdfAnnotation(this, AnnotRect, new AnnotLinkAction(LocMarkerName));
        }

        /// <summary>
        ///     Add rendering screen action to page
        /// </summary>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <param name="DisplayMedia">Display media object</param>
        /// <returns>PdfAnnotation</returns>
        public PdfAnnotation AddScreenAction
        (
            PdfRectangle AnnotRect,
            PdfDisplayMedia DisplayMedia
        )
        {
            return new PdfAnnotation(this, AnnotRect, new AnnotDisplayMedia(DisplayMedia));
        }

        /// <summary>
        ///     Add annotation file attachement with icon
        /// </summary>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <param name="EmbeddedFile">Embedded file</param>
        /// <param name="Icon">Icon</param>
        /// <returns>PdfAnnotation</returns>
        /// <remarks>
        ///     The AnnotRect is the icon rectangle area. To access the file
        ///     the user has to right click on the icon.
        /// </remarks>
        public PdfAnnotation AddFileAttachment
        (
            PdfRectangle AnnotRect,
            PdfEmbeddedFile EmbeddedFile,
            FileAttachIcon Icon
        )
        {
            return new PdfAnnotation(this, AnnotRect, new AnnotFileAttachment(EmbeddedFile, Icon));
        }

        /// <summary>
        ///     Add annotation file attachement with no icon
        /// </summary>
        /// <param name="AnnotRect">Annotation rectangle</param>
        /// <param name="EmbeddedFile">Embedded file</param>
        /// <returns>PdfAnnotation</returns>
        /// <remarks>
        ///     The AnnotRect is any area on the page. When the user right click this
        ///     area a floating menu will be displayed.
        /// </remarks>
        public PdfAnnotation AddFileAttachment
        (
            PdfRectangle AnnotRect,
            PdfEmbeddedFile EmbeddedFile
        )
        {
            return new PdfAnnotation(this, AnnotRect, new AnnotFileAttachment(EmbeddedFile, FileAttachIcon.NoIcon));
        }

        ////////////////////////////////////////////////////////////////////
        // Write object to PDF file
        ////////////////////////////////////////////////////////////////////

        internal override void WriteObjectToPdfFile()
        {
            // we have at least one contents object
            if (ContentsArray != null)
            {
                // page has one contents object
                if (ContentsArray.Count == 1)
                {
                    Dictionary.AddFormat("/Contents", "[{0} 0 R]", ContentsArray[0].ObjectNumber);
                    Dictionary.Add("/Resources", BuildResourcesDictionary(ContentsArray[0].ResObjects, true));
                }

                // page is made of multiple contents
                else
                {
                    // contents dictionary entry
                    var ContentsStr = new StringBuilder("[");

                    // build contents dictionary entry
                    foreach (var Contents in ContentsArray)
                    {
                        ContentsStr.AppendFormat("{0} 0 R ", Contents.ObjectNumber);
                    }

                    // add terminating bracket
                    ContentsStr.Length--;
                    ContentsStr.Append(']');
                    Dictionary.Add("/Contents", ContentsStr.ToString());

                    // resources array of all contents objects
                    var ResObjects = new List<PdfObject>();

                    // loop for all contents objects
                    foreach (var Contents in ContentsArray)
                        // make sure we have resources
                    {
                        if (Contents.ResObjects != null)
                            // loop for resources within this contents object
                        {
                            foreach (var ResObject in Contents.ResObjects)
                            {
                                // check if we have it already
                                var Ptr = ResObjects.BinarySearch(ResObject);
                                if (Ptr < 0)
                                {
                                    ResObjects.Insert(~Ptr, ResObject);
                                }
                            }
                        }
                    }

                    // save to dictionary
                    Dictionary.Add("/Resources", BuildResourcesDictionary(ResObjects, true));
                }
            }

            // call PdfObject routine
            base.WriteObjectToPdfFile();

            // exit
        }
    }
}