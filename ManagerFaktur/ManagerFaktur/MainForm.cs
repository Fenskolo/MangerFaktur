using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerFaktur
{
    public partial class MF : Form
    {
        public MF()
        {
            InitializeComponent();
            UtxtE.Value = Settings.Instance.DefWorkPath;
            uCView.DataSource = Enum.GetNames(typeof(UltraListViewStyle));
            uCView.DataBind();
            uCView.SelectedRow = uCView?.Rows[0];

            PropertyListView();
            temp();
        }

        private void PropertyListView()
        {
            UltraListViewSubItemColumn colFileSize = this.uListView.SubItemColumns.Add("FileSize");
            UltraListViewSubItemColumn colFileType = this.uListView.SubItemColumns.Add("FileType");
            UltraListViewSubItemColumn colDateModified = this.uListView.SubItemColumns.Add("DateModified");
            colFileSize.DataType = typeof(int);
            colFileSize.Format = "#,###,##0 KB";
            colFileSize.SubItemAppearance.TextHAlign = HAlign.Right;
            colFileType.DataType = typeof(string);
            colDateModified.DataType = typeof(DateTime);
            colFileType.Text = "Type";
            colDateModified.Text = "Date Modified";
            string shortDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string shortTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            colDateModified.Format = string.Format("{0} {1}", shortDateFormat, shortTimeFormat);
            this.uListView.MainColumn.DataType = typeof(string);
            this.uListView.MainColumn.Text = "Name";
        }

        private void Ustawienia_Click(object sender, EventArgs e)
        {
            var pG = new Property();
            pG.ShowDialog();
        }

        private void MF_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Instance.Serialze();
        }

        private void UtxtE_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            using (FolderBrowserDialog fd = new FolderBrowserDialog())
            {
                if(fd.ShowDialog()==DialogResult.OK)
                {
                    Settings.Instance.DefWorkPath = fd.SelectedPath;
                    UtxtE.Value = Settings.Instance.DefWorkPath;
                }
            }
        }

        private void temp()
        {
            

            Infragistics.Win.Appearance appPdf = this.uListView.Appearances.Add("pdf");
            appPdf.Image = Properties.Resources.pdf;
            Infragistics.Win.Appearance appWord = this.uListView.Appearances.Add("Word");
            appWord.Image = Properties.Resources.word;

            DirectoryInfo cDriveInfo = new DirectoryInfo(Settings.Instance.DefWorkPath);


            //Infragistics.Win.Appearance appFolder = this.uListView.Appearances.Add("folder");
            //appFolder.Image = Properties.Resources.folder;
            //DirectoryInfo[] directories = cDriveInfo.GetDirectories();
            //for (int i = 0; i < directories.Length; i++)
            //{
            //    DirectoryInfo directoryInfo = directories[i];

            //    UltraListViewItem item = this.uListView.Items.Add(directoryInfo.FullName, directoryInfo.Name);
            //    item.SubItems["FileType"].Value = "File Folder";
            //    item.SubItems["DateModified"].Value = directoryInfo.LastWriteTime;
            //    item.Appearance = this.uListView.Appearances["folder"];
            //}

            FileInfo[] files = cDriveInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = files[i];

                if(!Settings.Instance.ListExtenstion.Contains(fileInfo.Extension.ToUpper()))
                {
                    continue;
                }

                UltraListViewItem item = this.uListView.Items.Add(fileInfo.FullName, fileInfo.Name);
                item.SubItems["FileSize"].Value = fileInfo.Length / 1024;
                item.SubItems["FileType"].Value = "File";
                item.SubItems["DateModified"].Value = fileInfo.LastWriteTime;

                if(fileInfo.Extension.ToUpper() == ".PDF" )
                {
                    item.Appearance= appPdf;
                }
                else
                {
                    item.Appearance = appWord;
                }
            }
            
            this.uListView.View = UltraListViewStyle.Details;
        }

        private void uCView_ValueChanged(object sender, EventArgs e)
        {
            this.uListView.View = (UltraListViewStyle)Enum.Parse(typeof(UltraListViewStyle), uCView.SelectedRow.Cells[0].Value.ToString());
        }

        private void uListView_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            Process.Start(e.Item.Key);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var x in uListView.Items)
            {
                if(Convert.ToBoolean(x.CheckState))
                {
                    MessageBox.Show(x.Key);
                }
            }
        }
    }
}
