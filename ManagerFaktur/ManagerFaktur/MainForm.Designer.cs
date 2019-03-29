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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MF));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton("leftB");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton("rightB");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
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
            ((System.ComponentModel.ISupportInitialize)(this.uListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uTxt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uDTEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uComboPath)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Ustawienia,
            this.tCB});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(948, 25);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "toolStrip1";
            // 
            // Ustawienia
            // 
            this.Ustawienia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Ustawienia.Image = ((System.Drawing.Image)(resources.GetObject("Ustawienia.Image")));
            this.Ustawienia.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Ustawienia.Name = "Ustawienia";
            this.Ustawienia.Size = new System.Drawing.Size(68, 22);
            this.Ustawienia.Text = "Ustawienia";
            this.Ustawienia.Click += new System.EventHandler(this.Ustawienia_Click);
            // 
            // tCB
            // 
            this.tCB.Name = "tCB";
            this.tCB.Size = new System.Drawing.Size(121, 25);
            this.tCB.TextChanged += new System.EventHandler(this.TCB_TextChanged);
            // 
            // uListView
            // 
            this.uListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uListView.Location = new System.Drawing.Point(0, 0);
            this.uListView.Name = "uListView";
            this.uListView.Size = new System.Drawing.Size(587, 346);
            this.uListView.TabIndex = 2;
            this.uListView.Text = "ultraListView1";
            this.uListView.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.uListView.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.uListView.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.UListView_ItemActivated);
            this.uListView.ItemDoubleClick += new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.UListView_ItemDoubleClick);
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
            this.splitContainer1.Size = new System.Drawing.Size(948, 346);
            this.splitContainer1.SplitterDistance = 587;
            this.splitContainer1.TabIndex = 3;
            // 
            // uBtnShowTxt
            // 
            this.uBtnShowTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uBtnShowTxt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.uBtnShowTxt.Location = new System.Drawing.Point(0, 320);
            this.uBtnShowTxt.Name = "uBtnShowTxt";
            this.uBtnShowTxt.Size = new System.Drawing.Size(354, 23);
            this.uBtnShowTxt.TabIndex = 2;
            this.uBtnShowTxt.Text = "Zobacz Tekst";
            this.uBtnShowTxt.Click += new System.EventHandler(this.UBtnShowTxt_Click);
            // 
            // WBrowser
            // 
            this.WBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WBrowser.Location = new System.Drawing.Point(0, 0);
            this.WBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WBrowser.Name = "WBrowser";
            this.WBrowser.Size = new System.Drawing.Size(357, 320);
            this.WBrowser.TabIndex = 1;
            this.WBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WBrowser_DocumentCompleted);
            // 
            // uTxt
            // 
            this.uTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.Image = global::ManagerFaktur.Properties.Resources.settingsMail;
            editorButton1.Appearance = appearance1;
            editorButton1.Key = "leftB";
            this.uTxt.ButtonsLeft.Add(editorButton1);
            appearance2.Image = global::ManagerFaktur.Properties.Resources.right_button;
            editorButton2.Appearance = appearance2;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            editorButton2.Key = "rightB";
            this.uTxt.ButtonsRight.Add(editorButton2);
            this.uTxt.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.uTxt.Location = new System.Drawing.Point(730, 0);
            this.uTxt.Name = "uTxt";
            this.uTxt.NullText = "Mail do";
            this.uTxt.Size = new System.Drawing.Size(206, 21);
            this.uTxt.TabIndex = 5;
            this.uTxt.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.UTxt_EditorButtonClick);
            // 
            // uBtnMove
            // 
            this.uBtnMove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.uBtnMove.Location = new System.Drawing.Point(307, 2);
            this.uBtnMove.Name = "uBtnMove";
            this.uBtnMove.Size = new System.Drawing.Size(86, 23);
            this.uBtnMove.TabIndex = 6;
            this.uBtnMove.Text = "Przenieś Pliki";
            this.uBtnMove.Click += new System.EventHandler(this.UBtnMove_Click);
            // 
            // uDTEditor
            // 
            appearance3.Image = global::ManagerFaktur.Properties.Resources.right_button;
            editorButton3.Appearance = appearance3;
            this.uDTEditor.ButtonsLeft.Add(editorButton3);
            this.uDTEditor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2013;
            this.uDTEditor.FormatProvider = new System.Globalization.CultureInfo("pl-PL");
            this.uDTEditor.Location = new System.Drawing.Point(399, 2);
            this.uDTEditor.Name = "uDTEditor";
            this.uDTEditor.Size = new System.Drawing.Size(144, 21);
            this.uDTEditor.TabIndex = 7;
            this.uDTEditor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.UDTEditor_EditorButtonClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.btnRefresh.Location = new System.Drawing.Point(215, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 23);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Odśwież";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // uComboPath
            // 
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.uComboPath.DisplayLayout.Appearance = appearance4;
            this.uComboPath.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.uComboPath.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.uComboPath.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.GroupByBox.Appearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.uComboPath.DisplayLayout.GroupByBox.BandLabelAppearance = appearance6;
            this.uComboPath.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance7.BackColor2 = System.Drawing.SystemColors.Control;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.uComboPath.DisplayLayout.GroupByBox.PromptAppearance = appearance7;
            this.uComboPath.DisplayLayout.MaxColScrollRegions = 1;
            this.uComboPath.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uComboPath.DisplayLayout.Override.ActiveCellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.SystemColors.Highlight;
            appearance9.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.uComboPath.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            this.uComboPath.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.uComboPath.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.uComboPath.DisplayLayout.Override.CellAppearance = appearance11;
            this.uComboPath.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.uComboPath.DisplayLayout.Override.CellPadding = 0;
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            appearance12.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance12.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance12.BorderColor = System.Drawing.SystemColors.Window;
            this.uComboPath.DisplayLayout.Override.GroupByRowAppearance = appearance12;
            appearance13.TextHAlignAsString = "Left";
            this.uComboPath.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.uComboPath.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.uComboPath.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            this.uComboPath.DisplayLayout.Override.RowAppearance = appearance14;
            this.uComboPath.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.uComboPath.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.uComboPath.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uComboPath.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uComboPath.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uComboPath.Location = new System.Drawing.Point(550, 0);
            this.uComboPath.Name = "uComboPath";
            this.uComboPath.Size = new System.Drawing.Size(174, 22);
            this.uComboPath.TabIndex = 9;
            this.uComboPath.ValueChanged += new System.EventHandler(this.UComboPath_ValueChanged);
            // 
            // MF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 371);
            this.Controls.Add(this.uComboPath);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.uDTEditor);
            this.Controls.Add(this.uBtnMove);
            this.Controls.Add(this.uTxt);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MF";
            this.Text = "Manager Faktur";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MF_FormClosing);
            this.Load += new System.EventHandler(this.MF_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uListView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uTxt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uDTEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uComboPath)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainMenu;
        private System.Windows.Forms.ToolStripButton Ustawienia;
        private Infragistics.Win.UltraWinListView.UltraListView uListView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.WebBrowser WBrowser;
        private System.Windows.Forms.ToolStripComboBox tCB;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor uTxt;
        private Infragistics.Win.Misc.UltraButton uBtnMove;
        private Infragistics.Win.Misc.UltraButton uBtnShowTxt;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor uDTEditor;
        private Infragistics.Win.Misc.UltraButton btnRefresh;
        private Infragistics.Win.UltraWinGrid.UltraCombo uComboPath;
    }
}

