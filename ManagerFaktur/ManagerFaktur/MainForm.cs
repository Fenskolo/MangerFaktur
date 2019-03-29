using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infragistics.Win.UltraWinEditors;
using System.Linq;

namespace ManagerFaktur
{
    public partial class MF : Form
    {
        private WBHelper wb;
        private ExplorerHelper eh;
        private MailSender.MS ms;

        public MF()
        {
            InitializeComponent();
            wb = new WBHelper(WBrowser);
            eh = new ExplorerHelper(uListView);
            ms = new MailSender.MS();            
        }

        private void MF_Load(object sender, EventArgs e)
        {
            tCB.Items.AddRange(Enum.GetNames(typeof(UltraListViewStyle)));
            tCB.SelectedIndex = 0;
            var x =Directory.GetDirectories(Settings.Instance.WorkPath, "*", SearchOption.AllDirectories).AsEnumerable()
                .Where(f=> f.Contains(DateTime.Now.Year.ToString()) || f.Contains((DateTime.Now.Year -1).ToString())).ToDictionary(h=>h,
                z=> z.Split('\\').Last());
            uComboPath.DataSource = x;
            if (x.Count > 0)
            {
                uComboPath.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            }
            PropertyListView();
            eh.LoadExplorer(Settings.Instance.WorkPath);
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

            UltraListViewSubItemColumn colOkres = this.uListView.SubItemColumns.Add("Okres");
            UltraListViewSubItemColumn colSymbol = this.uListView.SubItemColumns.Add("Symbol");
            colOkres.DataType = typeof(DateTime);
            colSymbol.DataType = typeof(string);
            colSymbol.Text = "Symbol";
            colOkres.Text = "Okres";
        }

        private void Ustawienia_Click(object sender, EventArgs e)
        {
            var pG = new Property();
            pG.ShowDialog();
        }

        private void MF_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Czy napewno chcesz zakończyć pracę z programem?", typeof(Program).Assembly.GetName().Name, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Settings.Instance.Serialze();
            }
        }  

        private void UListView_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            Process.Start(e.Item.Key);
        }
            
        private void UListView_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            if(e.Item?.Key == null)
            {
                return;
            }
            if (e.Item.Key.ToUpper().Contains(".PDF"))
            {
                WBrowser.Navigate(e.Item.Key);
            }
            else
            {               
                wb.LoadDocument(e.Item.Key);
            }                
        }

        private void WBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (wb.TempFileName != string.Empty && !e.Url.ToString().ToUpper().Contains(".PDF") && e.Url.ToString() != "about:blank")
            {
                System.Threading.Thread.Sleep(100);
                File.Delete(wb.TempFileName);
                wb.TempFileName = string.Empty;
            }
        }  
        
        private void RefreshExplorer()
        {
            uListView.Items.Clear();
            eh.LoadExplorer(Settings.Instance.WorkPath);
        }

        private void TCB_TextChanged(object sender, EventArgs e)
        {
            this.uListView.View = (UltraListViewStyle)Enum.Parse(typeof(UltraListViewStyle), tCB.SelectedItem.ToString());
        }

        private void UTxt_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (e.Button.Key == "rightB")
            {
                string MailTo = MailSettings.Ins.MailTo;
                string message = MailSettings.Ins.Message;
                string subject = MailSettings.Ins.Subject;
                List<string> atach = new List<string>();

                foreach (var item in uListView.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        atach.Add(item.Key);
                    }
                }

                MailSettings.Ins.ListAtach = atach;

                SendMailTask(MailTo, MailSettings.Ins.ListAtach, message, subject);

                uTxt.Value = null;
            }
            else if((e.Button.Key == "leftB"))
            {
                MailProperty mp = new MailProperty();
                mp.ShowDialog();
            }
        }

        private void SendMailTask(string MailTo, List<string> atach, string message, string subject)
        {
            Task.Run(() =>
            {
                uTxt.ButtonsLeft[0].Enabled = false;
                if (ms.SendMail(Settings.Instance.Login, Settings.Instance.Password, Settings.Instance.From, MailTo, atach, message, subject))
                {
                    Logs.Log.SerializeLog();
                    Logs.Log.MyInstance = null;
                    MailSettings.Ins.MyInstance = null;
                    MessageBox.Show("Wysyłka maila powiodła się");
                }
                else
                {
                    MessageBox.Show("coś poszło nie tak");
                }
                uTxt.ButtonsLeft[0].Enabled = true;
            });
        }

        private void uBtnMove_Click(object sender, EventArgs e)
        {
            foreach(var x in uListView.Items)
            {
                if (x.CheckState == CheckState.Checked && x.SubItems["Okres"]?.Value != null)
                {
                    var dt = (DateTime)x.SubItems["Okres"].Value;
                    dt = (DateTime)x.SubItems["Okres"].Value == new DateTime() ? DateTime.Now : dt;
                    string month = dt.Month.ToString();
                    month = month.Length == 1 ? "0" + month : month;
                    string year = dt.Year.ToString();
                    string MonthYear = month + "-" + year;
                    string destDirectory = Path.Combine(Settings.Instance.DestPath, MonthYear);

                    if (!Directory.Exists(destDirectory))
                    {
                        Directory.CreateDirectory(destDirectory);
                    }

                    string symbol = (string)x.SubItems["Symbol"].Value;

                    symbol = string.IsNullOrEmpty(symbol) ? "Faktura" : symbol;

                    int nrFaktury = 1;


                    string fileName = $"{Settings.Instance.FileNameDest} {symbol} {MonthYear}.pdf";
                    if (symbol == "Faktura")
                    {
                        fileName = $"{Settings.Instance.FileNameDest} {symbol} {nrFaktury} {MonthYear}.pdf";
                        nrFaktury++;
                    }

                    bool notExists = false;
                    while (!notExists)
                    {
                        if (File.Exists(Path.Combine(destDirectory, fileName)))
                        {
                            fileName = $"{Settings.Instance.FileNameDest} {symbol} {nrFaktury} {MonthYear}.pdf";
                            nrFaktury++;
                        }
                        else
                        {
                            notExists = true;
                        }
                    }

                    WBrowser.Navigate("about:blank");
                    while (WBrowser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }

                    PairFiles pf = new PairFiles()
                    {
                        Old = x.Key.ToString(),
                        News = Path.Combine(destDirectory, fileName)
                    };
                    
                    Logs.Log.FileOperation.Add(pf);
                    File.Move(x.Key.ToString(), Path.Combine(destDirectory, fileName));
                }
            }

            Logs.Log.SerializeLog();
            Logs.Log.MyInstance = null;
            MailSettings.Ins.MyInstance = null;
            RefreshExplorer();
        }

        private void uBtnShowTxt_Click(object sender, EventArgs e)
        {
            var tekst = eh.ExtractTextFromPdf(WBrowser.Url.ToString());
            var txt = new TxtFromPdf(tekst);
            txt.ShowDialog();
            
        }

        private void UDTEditor_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            foreach(var x in uListView.Items)
            {
                if(x.CheckState==CheckState.Checked)
                {
                    x.SubItems["Okres"].Value = (DateTime)uDTEditor.Value;
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshExplorer();
        }

        private void UComboPath_ValueChanged(object sender, EventArgs e)
        {
            var x = uComboPath.SelectedRow;

            uListView.Items.Clear();
            eh.LoadExplorer(x.GetCellValue("Key").ToString());           
        }
    }
}
