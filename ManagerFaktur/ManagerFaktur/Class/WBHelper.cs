﻿using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Windows.Forms;

namespace ManagerFaktur
{
    public class WBHelper
    {
        private readonly WebBrowser wb;

        public string TempFileName { get; set; }

        private delegate void ConvertDocumentDelegate(string fileName);

        public WBHelper(WebBrowser _wb)
        {
            wb = _wb;
        }

        public void LoadDocument(string fileName)
        {
            ConvertDocumentDelegate del = new ConvertDocumentDelegate(ConvertDocument);
            del.BeginInvoke(fileName, DocumentConversionComplete, null);
        }

        private void ConvertDocument(string fileName)
        {
            object m = System.Reflection.Missing.Value;
            object oldFileName = fileName;
            object readOnly = false;
            Microsoft.Office.Interop.Word.Application ac = null;

            try
            {
                // First, create a new Microsoft.Office.Interop.Word.ApplicationClass.
                ac = new Microsoft.Office.Interop.Word.Application();

                // Now we open the document.
                Document doc = ac.Documents.Open(ref oldFileName, ref m, ref readOnly,
                    ref m, ref m, ref m, ref m, ref m, ref m, ref m,
                     ref m, ref m, ref m, ref m, ref m, ref m);

                // Create a temp file to save the HTML file to. 
                TempFileName = GetTempFile("html");

                // Cast these items to object.  The methods we're calling 
                // only take object types in their method parameters. 
                object newFileName = TempFileName;

                // We will be saving this file as HTML format. 
                object fileType = WdSaveFormat.wdFormatHTML;

                // Save the file. 
                doc.SaveAs(ref newFileName, ref fileType,
                    ref m, ref m, ref m, ref m, ref m, ref m, ref m,
                    ref m, ref m, ref m, ref m, ref m, ref m, ref m);

            }
            finally
            {
                // Make sure we close the application class. 
                if (ac != null)
                {
                    ac.Quit(ref readOnly, ref m, ref m);
                }
            }
        }

        private void DocumentConversionComplete(IAsyncResult result)
        {
            wb.Navigate(TempFileName);
        }

        private string GetTempFile(string extension)
        {
            string tempPath = Path.GetTempPath();
            string extensionPath = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            return Path.Combine(tempPath, extensionPath);
        }
    }
}
