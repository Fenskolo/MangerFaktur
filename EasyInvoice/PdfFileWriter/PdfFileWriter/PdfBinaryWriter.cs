/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfBinaryWriter
//	Extension to standard C# BinaryWriter class.
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

using System.IO;
using System.Text;

namespace PdfFileWriter
{
    /// <summary>
    ///     PDF binary writer class
    /// </summary>
    /// <remarks>
    ///     Extends .NET BinaryWriter class.
    /// </remarks>
    public class PdfBinaryWriter : BinaryWriter
    {
        /// <summary>
        ///     PDF binary writer constructor
        /// </summary>
        /// <param name="Stream">File or memory stream</param>
        public PdfBinaryWriter
        (
            Stream Stream
        ) : base(Stream, Encoding.UTF8)
        {
        }

        /// <summary>
        ///     Write String.
        /// </summary>
        /// <param name="Str">Input string</param>
        /// <remarks>
        ///     Convert each character from two bytes to one byte.
        /// </remarks>
        public void WriteString
        (
            string Str
        )
        {
            // byte array
            var ByteArray = new byte[Str.Length];

            // convert content from string to binary
            // do not use Encoding.ASCII.GetBytes(...)
            for (var Index = 0; Index < ByteArray.Length; Index++)
            {
                ByteArray[Index] = (byte) Str[Index];
            }

            // write to pdf file
            Write(ByteArray);
        }

        /// <summary>
        ///     Write StringBuilder.
        /// </summary>
        /// <param name="Str">String builder input</param>
        /// <remarks>
        ///     Convert each character from two bytes to one byte.
        /// </remarks>
        public void WriteString
        (
            StringBuilder Str
        )
        {
            // byte array
            var ByteArray = new byte[Str.Length];

            // convert content from string to binary
            // do not use Encoding.ASCII.GetBytes(...)
            for (var Index = 0; Index < ByteArray.Length; Index++)
            {
                ByteArray[Index] = (byte) Str[Index];
            }

            // write to pdf file
            Write(ByteArray);
        }

        /// <summary>
        ///     Combine format string with write string.
        /// </summary>
        /// <param name="FormatStr">Standard format string</param>
        /// <param name="List">Array of objects</param>
        public void WriteFormat
        (
            string FormatStr,
            params object[] List
        )
        {
            var Str = string.Format(FormatStr, List);

            // byte array
            var ByteArray = new byte[Str.Length];

            // convert content from string to binary
            // do not use Encoding.ASCII.GetBytes(...)
            for (var Index = 0; Index < ByteArray.Length; Index++)
            {
                ByteArray[Index] = (byte) Str[Index];
            }

            // write to pdf file
            Write(ByteArray);
        }
    }
}