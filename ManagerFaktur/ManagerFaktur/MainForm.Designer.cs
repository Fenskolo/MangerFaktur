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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.mainMenu = new System.Windows.Forms.ToolStrip();
            this.Ustawienia = new System.Windows.Forms.ToolStripButton();
            this.tCB = new System.Windows.Forms.ToolStripComboBox();
            this.uListView = new Infragistics.Win.UltraWinListView.UltraListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.WBrowser = new System.Windows.Forms.WebBrowser();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.uTxt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uTxt)).BeginInit();
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
            this.uListView.ItemActivated += new Infragistics.Win.UltraWinListView.ItemActivatedEventHandler(this.uListView_ItemActivated);
            this.uListView.ItemDoubleClick += new Infragistics.Win.UltraWinListView.ItemDoubleClickEventHandler(this.uListView_ItemDoubleClick);
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
            this.splitContainer1.Panel2.Controls.Add(this.WBrowser);
            this.splitContainer1.Size = new System.Drawing.Size(948, 346);
            this.splitContainer1.SplitterDistance = 587;
            this.splitContainer1.TabIndex = 3;
            // 
            // WBrowser
            // 
            this.WBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBrowser.Location = new System.Drawing.Point(0, 0);
            this.WBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WBrowser.Name = "WBrowser";
            this.WBrowser.Size = new System.Drawing.Size(357, 346);
            this.WBrowser.TabIndex = 1;
            this.WBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WBrowser_DocumentCompleted);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(201, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // uTxt
            // 
            this.uTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.Image = global::ManagerFaktur.Properties.Resources.right_button;
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            this.uTxt.ButtonsRight.Add(editorButton1);
            this.uTxt.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.WindowsVista;
            this.uTxt.Location = new System.Drawing.Point(730, 0);
            this.uTxt.Name = "uTxt";
            this.uTxt.NullText = "Mail do";
            this.uTxt.Size = new System.Drawing.Size(206, 21);
            this.uTxt.TabIndex = 5;
            this.uTxt.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.uTxt_EditorButtonClick);
            // 
            // MF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 371);
            this.Controls.Add(this.uTxt);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.Name = "MF";
            this.Text = "Manager Faktur";
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainMenu;
        private System.Windows.Forms.ToolStripButton Ustawienia;
        private Infragistics.Win.UltraWinListView.UltraListView uListView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        public System.Windows.Forms.WebBrowser WBrowser;
        private System.Windows.Forms.ToolStripComboBox tCB;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor uTxt;
    }
}

