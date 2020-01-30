﻿/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	AnnotAction
//	Annotation action classes 
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

namespace PdfFileWriter
{
    /// <summary>
    ///     File attachement icon
    /// </summary>
    public enum FileAttachIcon
    {
        /// <summary>
        ///     Graph
        /// </summary>
        Graph,

        /// <summary>
        ///     Paperclip
        /// </summary>
        Paperclip,

        /// <summary>
        ///     PushPin (default)
        /// </summary>
        PushPin,

        /// <summary>
        ///     Tag
        /// </summary>
        Tag,

        /// <summary>
        ///     no icon
        /// </summary>
        NoIcon
    }

    /// <summary>
    ///     Annotation action base class
    /// </summary>
    public class AnnotAction
    {
        internal AnnotAction
        (
            string Subtype
        )
        {
            this.Subtype = Subtype;
        }

        /// <summary>
        ///     Gets the PDF PdfAnnotation object subtype
        /// </summary>
        public string Subtype { get; internal set; }

        internal virtual bool IsEqual
        (
            AnnotAction Other
        )
        {
            throw new ApplicationException("AnnotAction IsEqual not implemented");
        }

        internal static bool IsEqual
        (
            AnnotAction One,
            AnnotAction Two
        )
        {
            if (One == null && Two == null)
            {
                return true;
            }

            if (One == null && Two != null || One != null && Two == null || One.GetType() != Two.GetType())
            {
                return false;
            }

            return One.IsEqual(Two);
        }
    }

    /// <summary>
    ///     Web link annotation action
    /// </summary>
    public class AnnotWebLink : AnnotAction
    {
        /// <summary>
        ///     Web link constructor
        /// </summary>
        /// <param name="WebLinkStr">Web link string</param>
        public AnnotWebLink
        (
            string WebLinkStr
        ) : base("/Link")
        {
            this.WebLinkStr = WebLinkStr;
        }

        /// <summary>
        ///     Gets or sets web link string
        /// </summary>
        public string WebLinkStr { get; set; }

        internal override bool IsEqual
        (
            AnnotAction Other
        )
        {
            return WebLinkStr == ((AnnotWebLink) Other).WebLinkStr;
        }
    }

    /// <summary>
    ///     Link to location marker within the document
    /// </summary>
    public class AnnotLinkAction : AnnotAction
    {
        /// <summary>
        ///     Go to annotation action constructor
        /// </summary>
        /// <param name="LocMarkerName">Location marker name</param>
        public AnnotLinkAction
        (
            string LocMarkerName
        ) : base("/Link")
        {
            this.LocMarkerName = LocMarkerName;
        }

        /// <summary>
        ///     Gets or sets the location marker name
        /// </summary>
        public string LocMarkerName { get; set; }

        internal override bool IsEqual
        (
            AnnotAction Other
        )
        {
            return LocMarkerName == ((AnnotLinkAction) Other).LocMarkerName;
        }
    }

    /// <summary>
    ///     Display video or play sound class
    /// </summary>
    public class AnnotDisplayMedia : AnnotAction
    {
        /// <summary>
        ///     Display media annotation action constructor
        /// </summary>
        /// <param name="DisplayMedia">PdfDisplayMedia</param>
        public AnnotDisplayMedia
        (
            PdfDisplayMedia DisplayMedia
        ) : base("/Screen")
        {
            this.DisplayMedia = DisplayMedia;
        }

        /// <summary>
        ///     Gets or sets PdfDisplayMedia object
        /// </summary>
        public PdfDisplayMedia DisplayMedia { get; set; }

        internal override bool IsEqual
        (
            AnnotAction Other
        )
        {
            return DisplayMedia.MediaFile.FileName == ((AnnotDisplayMedia) Other).DisplayMedia.MediaFile.FileName;
        }
    }

    /// <summary>
    ///     Save or view embedded file
    /// </summary>
    public class AnnotFileAttachment : AnnotAction
    {
        /// <summary>
        ///     Gets or sets associated icon
        /// </summary>
        public FileAttachIcon Icon;

        /// <summary>
        ///     File attachement constructor
        /// </summary>
        /// <param name="EmbeddedFile">Embedded file</param>
        /// <param name="Icon">Icon enumeration</param>
        public AnnotFileAttachment
        (
            PdfEmbeddedFile EmbeddedFile,
            FileAttachIcon Icon
        ) : base("/FileAttachment")
        {
            this.EmbeddedFile = EmbeddedFile;
            this.Icon = Icon;
        }

        /// <summary>
        ///     File attachement constructor (no icon)
        /// </summary>
        /// <param name="EmbeddedFile">Embedded file</param>
        public AnnotFileAttachment
        (
            PdfEmbeddedFile EmbeddedFile
        ) : base("/FileAttachment")
        {
            this.EmbeddedFile = EmbeddedFile;
            Icon = FileAttachIcon.NoIcon;
        }

        /// <summary>
        ///     Gets or sets embedded file
        /// </summary>
        public PdfEmbeddedFile EmbeddedFile { get; set; }

        internal override bool IsEqual
        (
            AnnotAction Other
        )
        {
            return EmbeddedFile.FileName == ((AnnotFileAttachment) Other).EmbeddedFile.FileName &&
                   Icon == ((AnnotFileAttachment) Other).Icon;
        }
    }
}