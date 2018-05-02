namespace ManagerFaktur
{
    partial class MailProperty
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
            this.propMail = new System.Windows.Forms.PropertyGrid();
            this.btnUstaw = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propMail
            // 
            this.propMail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propMail.Location = new System.Drawing.Point(1, 2);
            this.propMail.Name = "propMail";
            this.propMail.Size = new System.Drawing.Size(409, 220);
            this.propMail.TabIndex = 0;
            // 
            // btnUstaw
            // 
            this.btnUstaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUstaw.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnUstaw.Location = new System.Drawing.Point(335, 228);
            this.btnUstaw.Name = "btnUstaw";
            this.btnUstaw.Size = new System.Drawing.Size(75, 23);
            this.btnUstaw.TabIndex = 1;
            this.btnUstaw.Text = "Ustaw";
            this.btnUstaw.UseVisualStyleBackColor = true;
            this.btnUstaw.Click += new System.EventHandler(this.btnUstaw_Click);
            // 
            // MailProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 258);
            this.Controls.Add(this.btnUstaw);
            this.Controls.Add(this.propMail);
            this.Name = "MailProperty";
            this.Text = "MailProp";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propMail;
        private System.Windows.Forms.Button btnUstaw;
    }
}