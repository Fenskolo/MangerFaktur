﻿/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfExtGState
//	External graphics state dictionary.
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

namespace PdfFileWriter
{
    internal class PdfExtGState : PdfObject, IComparable<PdfExtGState>
    {
        internal string Key;
        internal string Value;

        // search constructor
        internal PdfExtGState
        (
            string Key,
            string Value
        )
        {
            // save value
            this.Key = Key;
            this.Value = Value;

            // exit
        }

        // object constructor
        internal PdfExtGState
        (
            PdfDocument Document,
            string Key,
            string Value
        ) : base(Document, ObjectType.Dictionary, "/ExtGState")
        {
            // save value
            this.Key = Key;
            this.Value = Value;

            // create resource code
            ResourceCode = Document.GenerateResourceNumber('G');
        }

        /// <summary>
        ///     Compare two PdfExtGState objects.
        /// </summary>
        /// <param name="Other">Other object.</param>
        /// <returns>Compare result.</returns>
        public int CompareTo
        (
            PdfExtGState Other
        )
        {
            var Cmp = string.Compare(Key, Other.Key);
            if (Cmp != 0)
            {
                return Cmp;
            }

            return string.Compare(Value, Other.Value);
        }

        internal static PdfExtGState CreateExtGState
        (
            PdfDocument Document,
            string Key,
            string Value
        )
        {
            if (Document.ExtGStateArray == null)
            {
                Document.ExtGStateArray = new List<PdfExtGState>();
            }

            // search list for a duplicate
            var Index = Document.ExtGStateArray.BinarySearch(new PdfExtGState(Key, Value));

            // this value is a duplicate
            if (Index >= 0)
            {
                return Document.ExtGStateArray[Index];
            }

            // new blend object
            var ExtGState = new PdfExtGState(Document, Key, Value);

            // save new string in array
            Document.ExtGStateArray.Insert(~Index, ExtGState);

            // update dictionary
            ExtGState.Dictionary.Add(Key, Value);

            // exit
            return ExtGState;
        }
    }
}