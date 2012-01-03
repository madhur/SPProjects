namespace MySiteWebPartsPropertyChanger
{
    partial class Main
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblMySiteUrl = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lstWebParts = new System.Windows.Forms.ListBox();
            this.dgProperties = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.lblErrors = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Site Url";
            // 
            // lblMySiteUrl
            // 
            this.lblMySiteUrl.AutoSize = true;
            this.lblMySiteUrl.Location = new System.Drawing.Point(101, 25);
            this.lblMySiteUrl.Name = "lblMySiteUrl";
            this.lblMySiteUrl.Size = new System.Drawing.Size(0, 13);
            this.lblMySiteUrl.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(238, 481);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(179, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Propogate Changes to All Users";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(492, 481);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lstWebParts
            // 
            this.lstWebParts.FormattingEnabled = true;
            this.lstWebParts.Location = new System.Drawing.Point(25, 83);
            this.lstWebParts.Name = "lstWebParts";
            this.lstWebParts.Size = new System.Drawing.Size(161, 381);
            this.lstWebParts.TabIndex = 4;
            this.lstWebParts.SelectedIndexChanged += new System.EventHandler(this.lstWebParts_SelectedIndexChanged_1);
            // 
            // dgProperties
            // 
            this.dgProperties.AllowUserToAddRows = false;
            this.dgProperties.AllowUserToDeleteRows = false;
            this.dgProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProperties.Location = new System.Drawing.Point(238, 83);
            this.dgProperties.Name = "dgProperties";
            this.dgProperties.Size = new System.Drawing.Size(395, 375);
            this.dgProperties.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "WebParts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Properties";
            // 
            // prgBar
            // 
            this.prgBar.Location = new System.Drawing.Point(218, 511);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(395, 23);
            this.prgBar.TabIndex = 9;
            this.prgBar.Visible = false;
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(238, 511);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(0, 13);
            this.lblErrors.TabIndex = 10;
            this.lblErrors.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 539);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.prgBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgProperties);
            this.Controls.Add(this.lstWebParts);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblMySiteUrl);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "MySite WebParts Property Changer";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgProperties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMySiteUrl;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ListBox lstWebParts;
        private System.Windows.Forms.DataGridView dgProperties;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.Label lblErrors;
    }
}

