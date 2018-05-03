namespace ManagerFaktur
{
    partial class TxtFromPdf
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
            this.rTxt = new System.Windows.Forms.RichTextBox();
            this.uBtnClose = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // rTxt
            // 
            this.rTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTxt.Location = new System.Drawing.Point(2, 3);
            this.rTxt.Name = "rTxt";
            this.rTxt.Size = new System.Drawing.Size(485, 412);
            this.rTxt.TabIndex = 1;
            this.rTxt.Text = "";
            // 
            // uBtnClose
            // 
            this.uBtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uBtnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Windows8Button;
            this.uBtnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uBtnClose.Location = new System.Drawing.Point(2, 421);
            this.uBtnClose.Name = "uBtnClose";
            this.uBtnClose.Size = new System.Drawing.Size(485, 23);
            this.uBtnClose.TabIndex = 2;
            this.uBtnClose.Text = "Zamknij";
            // 
            // TxtFromPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 445);
            this.Controls.Add(this.uBtnClose);
            this.Controls.Add(this.rTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TxtFromPdf";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TxtFromPdf";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rTxt;
        private Infragistics.Win.Misc.UltraButton uBtnClose;
    }
}