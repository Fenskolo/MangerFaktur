﻿/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfEncryption
//	Support for AES-128 encryption
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
using System.IO;
using System.Security.Cryptography;

namespace PdfFileWriter
{
    /// <summary>
    ///     Encryption type enumeration
    /// </summary>
    public enum EncryptionType
    {
        /// <summary>
        ///     AES 128 bits
        /// </summary>
        Aes128,

        /// <summary>
        ///     Standard 128 bits
        /// </summary>
        Standard128
    }

    /// <summary>
    ///     PDF reader permission flags enumeration
    /// </summary>
    /// <remarks>
    ///     PDF reference manual version 1.7 Table 3.20
    /// </remarks>
    public enum Permission
    {
        /// <summary>
        ///     No permission flags
        /// </summary>
        None = 0,

        /// <summary>
        ///     Low quality print (bit 3)
        /// </summary>
        LowQalityPrint = 4, // bit 3

        /// <summary>
        ///     Modify contents (bit 4)
        /// </summary>
        ModifyContents = 8, // bit 4

        /// <summary>
        ///     Extract contents (bit 5)
        /// </summary>
        ExtractContents = 0x10, // bit 5

        /// <summary>
        ///     Annotation (bit 6)
        /// </summary>
        Annotation = 0x20, // bit 6

        /// <summary>
        ///     Interactive (bit 9)
        /// </summary>
        Interactive = 0x100, // bit 9

        /// <summary>
        ///     Accessibility (bit 10)
        /// </summary>
        Accessibility = 0x200, // bit 10

        /// <summary>
        ///     Assemble document (bit 11)
        /// </summary>
        AssembleDoc = 0x400, // bit 11

        /// <summary>
        ///     Print (bit 12 plus bit 3)
        /// </summary>
        Print = 0x804, // bit 12 + bit 3

        /// <summary>
        ///     All permission bits
        /// </summary>
        All = 0xf3c // bits 3, 4, 5, 6, 9, 10, 11, 12
    }

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    ///     PDF encryption class
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         For more information go to
    ///         <a
    ///             href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#EncryptionSupport">
    ///             2.6
    ///             Encryption Support
    ///         </a>
    ///     </para>
    /// </remarks>
    ////////////////////////////////////////////////////////////////////
    public class PdfEncryption : PdfObject, IDisposable
    {
        private const int PermissionBase = unchecked((int) 0xfffff0c0);

        private static readonly byte[] PasswordPad =
        {
            0x28, 0xBF, 0x4E, 0x5E, 0x4E, 0x75, 0x8A, 0x41,
            0x64, 0x00, 0x4E, 0x56, 0xFF, 0xFA, 0x01, 0x08,
            0x2E, 0x2E, 0x00, 0xB6, 0xD0, 0x68, 0x3E, 0x80,
            0x2F, 0x0C, 0xA9, 0xFE, 0x64, 0x53, 0x69, 0x7A
        };

        private static readonly byte[] Salt = {0x73, 0x41, 0x6c, 0x54};
        internal AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
        internal EncryptionType EncryptionType;
        internal byte[] MasterKey;
        internal MD5 MD5 = MD5.Create();

        internal int Permissions;

        ////////////////////////////////////////////////////////////////////
        // Encryption Constructor
        ////////////////////////////////////////////////////////////////////

        internal PdfEncryption
        (
            PdfDocument Document,
            string UserPassword,
            string OwnerPassword,
            Permission UserPermissions,
            EncryptionType EncryptionType
        ) : base(Document)
        {
            // Notes:
            // The PDF File Writer library supports AES 128 encryption and standard 128 encryption.
            // The library does not strip leading or trailing white space. They are part of the password.
            // EncriptMetadata is assumed to be true (this libraray does not use metadata).
            // Embeded Files Only is assumed to be false (this library does not have embeded files).

            // remove all unused bits and add all bits that must be one
            Permissions = ((int) UserPermissions & (int) Permission.All) | PermissionBase;
            Dictionary.AddInteger("/P", Permissions);

            // convert user string password to byte array
            var UserBinaryPassword = ProcessPassword(UserPassword);

            // convert owner string password to byte array
            if (string.IsNullOrEmpty(OwnerPassword))
            {
                OwnerPassword = BitConverter.ToUInt64(PdfDocument.RandomByteArray(8), 0).ToString();
            }

            var OwnerBinaryPassword = ProcessPassword(OwnerPassword);

            // calculate owner key for crypto dictionary
            var OwnerKey = CreateOwnerKey(UserBinaryPassword, OwnerBinaryPassword);

            // create master key and user key
            CreateMasterKey(UserBinaryPassword, OwnerKey);
            var UserKey = CreateUserKey();

            // build dictionary
            Dictionary.Add("/Filter", "/Standard");
            Dictionary.Add("/Length", "128");
            Dictionary.Add("/O", Document.ByteArrayToPdfHexString(OwnerKey));
            Dictionary.Add("/U", Document.ByteArrayToPdfHexString(UserKey));

            // encryption type
            this.EncryptionType = EncryptionType;
            if (EncryptionType == EncryptionType.Aes128)
            {
                Dictionary.Add("/R", "4");
                Dictionary.Add("/V", "4");
                Dictionary.Add("/StrF", "/StdCF");
                Dictionary.Add("/StmF", "/StdCF");
                Dictionary.Add("/CF", "<</StdCF<</Length 16/AuthEvent/DocOpen/CFM/AESV2>>>>");
            }
            else
            {
                Dictionary.Add("/R", "3");
                Dictionary.Add("/V", "2");
            }

            // add encryption to trailer dictionary
            Document.TrailerDict.AddIndirectReference("/Encrypt", this);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Dispose unmanaged resources
        /// </summary>
        ////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            if (AES != null)
            {
                AES.Clear();
                // NOTE: AES.Dispose() is valid for .NET 4.0 and later.
                // In other words visual studio 2010 and later.
                // If you compile this source with older versions of VS
                // remove this call at your risk.
                AES.Dispose();
                AES = null;
            }

            if (MD5 != null)
            {
                MD5.Clear();
                MD5 = null;
            }
        }

        ////////////////////////////////////////////////////////////////////
        // Encrypt byte array
        ////////////////////////////////////////////////////////////////////

        internal byte[] EncryptByteArray
        (
            int ObjectNumber,
            byte[] PlainText
        )
        {
            // create encryption key
            var EncryptionKey = CreateEncryptionKey(ObjectNumber);
            byte[] CipherText;

            if (EncryptionType == EncryptionType.Aes128)
            {
                MemoryStream OutputStream = null;
                CryptoStream CryptoStream = null;

                // generate new initialization vector IV 
                AES.GenerateIV();

                // create cipher text buffer including initialization vector
                var CipherTextLen = (PlainText.Length & 0x7ffffff0) + 16;
                CipherText = new byte[CipherTextLen + 16];
                Array.Copy(AES.IV, 0, CipherText, 0, 16);

                // set encryption key and key length
                AES.Key = EncryptionKey;

                // Create the streams used for encryption.
                OutputStream = new MemoryStream();
                CryptoStream = new CryptoStream(OutputStream, AES.CreateEncryptor(), CryptoStreamMode.Write);

                // write plain text byte array
                CryptoStream.Write(PlainText, 0, PlainText.Length);

                // encrypt plain text to cipher text
                CryptoStream.FlushFinalBlock();

                // get the result
                OutputStream.Seek(0, SeekOrigin.Begin);
                OutputStream.Read(CipherText, 16, CipherTextLen);

                // release resources
                CryptoStream.Clear();
                OutputStream.Close();
            }
            else
            {
                CipherText = (byte[]) PlainText.Clone();
                EncryptRC4(EncryptionKey, CipherText);
            }

            // return result
            return CipherText;
        }

        ////////////////////////////////////////////////////////////////////
        // Process Password
        ////////////////////////////////////////////////////////////////////

        private byte[] ProcessPassword
        (
            string StringPassword
        )
        {
            // no user password
            if (string.IsNullOrEmpty(StringPassword))
            {
                return (byte[]) PasswordPad.Clone();
            }

            // convert password to byte array
            var BinaryPassword = new byte[32];
            var IndexEnd = Math.Min(StringPassword.Length, 32);
            for (var Index = 0; Index < IndexEnd; Index++)
            {
                var PWChar = StringPassword[Index];
                if (PWChar > 255)
                {
                    throw new ApplicationException("Owner or user Password has invalid character (allowed 0-255)");
                }

                BinaryPassword[Index] = (byte) PWChar;
            }

            // if user password is shorter than 32 bytes, add padding			
            if (IndexEnd < 32)
            {
                Array.Copy(PasswordPad, 0, BinaryPassword, IndexEnd, 32 - IndexEnd);
            }

            // return password
            return BinaryPassword;
        }

        ////////////////////////////////////////////////////////////////////
        // Create owner key
        ////////////////////////////////////////////////////////////////////

        private byte[] CreateOwnerKey
        (
            byte[] UserBinaryPassword,
            byte[] OwnerBinaryPassword
        )
        {
            // create hash array for owner password
            var OwnerHash = MD5.ComputeHash(OwnerBinaryPassword);

            // loop 50 times creating hash of a hash
            for (var Index = 0; Index < 50; Index++)
            {
                OwnerHash = MD5.ComputeHash(OwnerHash);
            }

            var ownerKey = (byte[]) UserBinaryPassword.Clone();
            var TempKey = new byte[16];
            for (var Index = 0; Index < 20; Index++)
            {
                for (var Tindex = 0; Tindex < 16; Tindex++)
                {
                    TempKey[Tindex] = (byte) (OwnerHash[Tindex] ^ Index);
                }

                EncryptRC4(TempKey, ownerKey);
            }

            // return encryption key
            return ownerKey;
        }

        ////////////////////////////////////////////////////////////////////
        // Create master key
        ////////////////////////////////////////////////////////////////////

        private void CreateMasterKey
        (
            byte[] UserBinaryPassword,
            byte[] OwnerKey
        )
        {
            // input byte array for MD5 hash function
            var HashInput = new byte[UserBinaryPassword.Length + OwnerKey.Length + 4 + Document.DocumentID.Length];
            var Ptr = 0;
            Array.Copy(UserBinaryPassword, 0, HashInput, Ptr, UserBinaryPassword.Length);
            Ptr += UserBinaryPassword.Length;
            Array.Copy(OwnerKey, 0, HashInput, Ptr, OwnerKey.Length);
            Ptr += OwnerKey.Length;
            HashInput[Ptr++] = (byte) Permissions;
            HashInput[Ptr++] = (byte) (Permissions >> 8);
            HashInput[Ptr++] = (byte) (Permissions >> 16);
            HashInput[Ptr++] = (byte) (Permissions >> 24);
            Array.Copy(Document.DocumentID, 0, HashInput, Ptr, Document.DocumentID.Length);
            MasterKey = MD5.ComputeHash(HashInput);

            // loop 50 times creating hash of a hash
            for (var Index = 0; Index < 50; Index++)
            {
                MasterKey = MD5.ComputeHash(MasterKey);
            }

            // exit
        }

        ////////////////////////////////////////////////////////////////////
        // Create user key
        ////////////////////////////////////////////////////////////////////

        private byte[] CreateUserKey()
        {
            // input byte array for MD5 hash function
            var HashInput = new byte[PasswordPad.Length + Document.DocumentID.Length];
            Array.Copy(PasswordPad, 0, HashInput, 0, PasswordPad.Length);
            Array.Copy(Document.DocumentID, 0, HashInput, PasswordPad.Length, Document.DocumentID.Length);
            var UserKey = MD5.ComputeHash(HashInput);
            var TempKey = new byte[16];

            for (var Index = 0; Index < 20; Index++)
            {
                for (var Tindex = 0; Tindex < 16; Tindex++)
                {
                    TempKey[Tindex] = (byte) (MasterKey[Tindex] ^ Index);
                }

                EncryptRC4(TempKey, UserKey);
            }

            Array.Resize(ref UserKey, 32);
            return UserKey;
        }

        ////////////////////////////////////////////////////////////////////
        // Create encryption key
        ////////////////////////////////////////////////////////////////////

        private byte[] CreateEncryptionKey
        (
            int ObjectNumber
        )
        {
            var HashInput =
                new byte[MasterKey.Length + 5 + (EncryptionType == EncryptionType.Aes128 ? Salt.Length : 0)];
            var Ptr = 0;
            Array.Copy(MasterKey, 0, HashInput, Ptr, MasterKey.Length);
            Ptr += MasterKey.Length;
            HashInput[Ptr++] = (byte) ObjectNumber;
            HashInput[Ptr++] = (byte) (ObjectNumber >> 8);
            HashInput[Ptr++] = (byte) (ObjectNumber >> 16);
            HashInput[Ptr++] = 0; // Generation is always zero for this library
            HashInput[Ptr++] = 0; // Generation is always zero for this library
            if (EncryptionType == EncryptionType.Aes128)
            {
                Array.Copy(Salt, 0, HashInput, Ptr, Salt.Length);
            }

            var EncryptionKey = MD5.ComputeHash(HashInput);
            if (EncryptionKey.Length > 16)
            {
                Array.Resize(ref EncryptionKey, 16);
            }

            return EncryptionKey;
        }

        ////////////////////////////////////////////////////////////////////
        // RC4 Encryption
        ////////////////////////////////////////////////////////////////////

        private void EncryptRC4
        (
            byte[] Key,
            byte[] Data
        )
        {
            var State = new byte[256];
            for (var Index = 0; Index < 256; Index++)
            {
                State[Index] = (byte) Index;
            }

            var Index1 = 0;
            var Index2 = 0;
            for (var Index = 0; Index < 256; Index++)
            {
                Index2 = (Key[Index1] + State[Index] + Index2) & 255;
                var tmp = State[Index];
                State[Index] = State[Index2];
                State[Index2] = tmp;
                Index1 = (Index1 + 1) % Key.Length;
            }

            var x = 0;
            var y = 0;
            for (var Index = 0; Index < Data.Length; Index++)
            {
                x = (x + 1) & 255;
                y = (State[x] + y) & 255;
                var tmp = State[x];
                State[x] = State[y];
                State[y] = tmp;
                Data[Index] = (byte) (Data[Index] ^ State[(State[x] + State[y]) & 255]);
            }
        }
    }
}