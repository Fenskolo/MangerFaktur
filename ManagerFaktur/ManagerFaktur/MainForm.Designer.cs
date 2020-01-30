namespace ManagerFaktur
{
    partial class MF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(MF));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 =
                new Infragistics.Win.UltraWinEditors.EditorButton("leftB");
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 =
                new Infragistics.Win.UltraWinEditors.EditorButton("rightB");
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 =
                new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.mainMenu = new System.Windows.Forms.ToolStrip();
            this.Ustawienia = new System.Windows.Forms.ToolStripButton();
            this.tCB = new System.Windows.Forms.ToolStripComboBox();
            this.uListView = new Infragistics.Win.UltraWinListView.UltraListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.uBtnShowTxt = new Infragistics.Win.Misc.UltraButton();
            this.WBrowser = new System.Windows.Forms.WebBrowser();
            this.uTxt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.uBtnMove = new Infragistics.Win.Misc.UltraButton();
            this.uDTEditor = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.uComboPath = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.uListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.uTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.uDTEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.uComboPath)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.Ustawienia,
                this.tCB
            });
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1106, 25);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "toolStrip1";
            // 
            // Ustawienia
            // 
            this.Ustawienia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Ustawienia.Image = ((System.Drawing.Image) (resources.GetObject("Ustawienia.Image")));
            this.Ustawienia.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Ustawienia.Name = "Ustawienia";
            this.Ustawienia.Size = new System.Drawing.Size(68, 22);
            this.Ustawienia.Text = "Ustawienia";
            this.Ustawienia.Click += new System.EventHandler(this.Ustawienia_Click);
            // 
            // tCB
            // 
            this.tCB.Name = "tCB";
            this.tCB.Size = new System.Drawing.Size(140, 25);
            this.tCB.TextChanged += new System.EventHandler(this.TCB_TextChanged);
            // 
            // uListView
            // 
            this.uListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uListView.Location = new System.Drawing.Point(0, 0);
            this.uListView.Name = "uListView";
            this.uListView.Size = new System.Drawing.Size(684, 403);
            this.uListView.TabIndex = 2;
            this.uListView.Text = "ultraListView1";
            this.uListView.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.uListView.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.uListView.ItemActivated +=
                new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.UListView_ItemActivated);
            this.uListView.ItemDoubleClick +=
                new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.UListView_ItemDoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.uListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.uBtnShowTxt);
            this.splitContainer1.Panel2.Controls.Add(this.WBrowser);
            this.splitContainer1.Size = new System.Drawing.Size(1106, 403);
            this.splitContainer1.SplitterDistance = 684;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // uBtnShowTxt
            // 
            this.uBtnShowTxt.Anchor =
                ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.uBtnShowTxt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.uBtnShowTxt.Location = new System.Drawing.Point(0, 373);
            this.uBtnShowTxt.Name = "uBtnShowTxt";
            this.uBtnShowTxt.Size = new System.Drawing.Size(414, 27);
            this.uBtnShowTxt.TabIndex = 2;
            this.uBtnShowTxt.Text = "Zobacz Tekst";
            this.uBtnShowTxt.Click += new System.EventHandler(this.UBtnShowTxt_Click);
            // 
            // WBrowser
            // 
            this.WBrowser.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.WBrowser.Location = new System.Drawing.Point(0, 0);
            this.WBrowser.MinimumSize = new System.Drawing.Size(23, 23);
            this.WBrowser.Name = "WBrowser";
            this.WBrowser.Size = new System.Drawing.Size(417, 373);
            this.WBrowser.TabIndex = 1;
            this.WBrowser.DocumentCompleted +=
                new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WBrowser_DocumentCompleted);
            // 
            // uTxt
            // 
            this.uTxt.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            editorButton1.Key = "leftB";
            this.uTxt.ButtonsLeft.Add(editorButton1);
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            editorButton2.Key = "rightB";
            this.uTxt.ButtonsRight.Add(editorButton2);
            this.uTxt.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.uTxt.Location = new System.Drawing.Point(852, 0);
            this.uTxt.Name = "uTxt";
            this.uTxt.NullText = "Mail do";
            this.uTxt.Size = new System.Drawing.Size(240, 25);
            this.uTxt.TabIndex = 5;
            this.uTxt.EditorButtonClick +=
                new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.UTxt_EditorButtonClick);
            // 
            // uBtnMove
            // 
            this.uBtnMove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.uBtnMove.Location = new System.Drawing.Point(358, 2);
            this.uBtnMove.Name = "uBtnMove";
            this.uBtnMove.Size = new System.Drawing.Size(100, 27);
            this.uBtnMove.TabIndex = 6;
            this.uBtnMove.Text = "Przenieś Pliki";
            this.uBtnMove.Click += new System.EventHandler(this.UBtnMove_Click);
            // 
            // uDTEditor
            // 
            this.uDTEditor.ButtonsLeft.Add(editorButton3);
            this.uDTEditor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2013;
            this.uDTEditor.FormatProvider = new System.Globalization.CultureInfo("pl-PL");
            this.uDTEditor.Location = new System.Drawing.Point(465, 2);
            this.uDTEditor.Name = "uDTEditor";
            this.uDTEditor.Size = new System.Drawing.Size(168, 25);
            this.uDTEditor.TabIndex = 7;
            this.uDTEditor.EditorButtonClick +=
                new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.UDTEditor_EditorButtonClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.btnRefresh.Location = new System.Drawing.Point(251, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 27);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Odśwież";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // uComboPath
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.uComboPath.DisplayLayout.Appearance = appearance1;
            this.uComboPath.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.uComboPath.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.uComboPath.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.uComboPath.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.uComboPath.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.uComboPath.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.uComboPath.DisplayLayout.MaxColScrollRegions = 1;
            this.uComboPath.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uComboPath.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.uComboPath.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.uComboPath.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.uComboPath.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.uComboPath.DisplayLayout.Override.CellAppearance = appearance8;
            this.uComboPath.DisplayLayout.Override.CellClickAction =
                Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.uComboPath.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.uComboPath.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.uComboPath.DisplayLayout.Override.HeaderClickAction =
                Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.uComboPath.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.uComboPath.DisplayLayout.Override.RowAppearance = appearance11;
            this.uComboPath.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.uComboPath.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.uComboPath.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uComboPath.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uComboPath.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uComboPath.Location = new System.Drawing.Point(642, 0);
            this.uComboPath.Name = "uComboPath";
            this.uComboPath.Size = new System.Drawing.Size(203, 26);
            this.uComboPath.TabIndex = 9;
            this.uComboPath.ValueChanged += new System.EventHandler(this.UComboPath_ValueChanged);
            // 
            // MF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 428);
            this.Controls.Add(this.uComboPath);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.uDTEditor);
            this.Controls.Add(this.uBtnMove);
            this.Controls.Add(this.uTxt);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "MF";
            this.Text = "Manager Faktur";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MF_FormClosing);
            this.Load += new System.EventHandler(this.MF_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.uListView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.uTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.uDTEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.uComboPath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip mainMenu;
        private System.Windows.Forms.ToolStripButton Ustawienia;
        private Infragistics.Win.UltraWinListView.UltraListView uListView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripComboBox tCB;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor uTxt;
        private Infragistics.Win.Misc.UltraButton uBtnMove;
        private Infragistics.Win.Misc.UltraButton uBtnShowTxt;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor uDTEditor;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.UltraWinGrid.UltraCombo uComboPath;
        private System.Windows.Forms.WebBrowser WBrowser;
    }
}

