namespace DVLD.Licenses.Controls
{
    partial class ctrlDriverLicenses
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbDriverLicenses = new System.Windows.Forms.GroupBox();
            this.tcDriverLicenses = new System.Windows.Forms.TabControl();
            this.tpLocalLicenses = new System.Windows.Forms.TabPage();
            this.lblLocalLicensesRecordsCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLocalLicensesHistory = new System.Windows.Forms.DataGridView();
            this.tpInternationalLicanses = new System.Windows.Forms.TabPage();
            this.lblInternationalLicensesRecordsCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvInternationalLicensesHistory = new System.Windows.Forms.DataGridView();
            this.cmsInternationalLicensesHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsLocalLicensesHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.gbDriverLicenses.SuspendLayout();
            this.tcDriverLicenses.SuspendLayout();
            this.tpLocalLicenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicensesHistory)).BeginInit();
            this.tpInternationalLicanses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalLicensesHistory)).BeginInit();
            this.cmsInternationalLicensesHistory.SuspendLayout();
            this.cmsLocalLicensesHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDriverLicenses
            // 
            this.gbDriverLicenses.Controls.Add(this.tcDriverLicenses);
            this.gbDriverLicenses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDriverLicenses.Location = new System.Drawing.Point(3, 3);
            this.gbDriverLicenses.Name = "gbDriverLicenses";
            this.gbDriverLicenses.Size = new System.Drawing.Size(1113, 259);
            this.gbDriverLicenses.TabIndex = 0;
            this.gbDriverLicenses.TabStop = false;
            this.gbDriverLicenses.Text = "Driver Licenses";
            // 
            // tcDriverLicenses
            // 
            this.tcDriverLicenses.Controls.Add(this.tpLocalLicenses);
            this.tcDriverLicenses.Controls.Add(this.tpInternationalLicanses);
            this.tcDriverLicenses.Location = new System.Drawing.Point(6, 34);
            this.tcDriverLicenses.Name = "tcDriverLicenses";
            this.tcDriverLicenses.SelectedIndex = 0;
            this.tcDriverLicenses.Size = new System.Drawing.Size(1101, 219);
            this.tcDriverLicenses.TabIndex = 0;
            // 
            // tpLocalLicenses
            // 
            this.tpLocalLicenses.Controls.Add(this.lblLocalLicensesRecordsCount);
            this.tpLocalLicenses.Controls.Add(this.label2);
            this.tpLocalLicenses.Controls.Add(this.label1);
            this.tpLocalLicenses.Controls.Add(this.dgvLocalLicensesHistory);
            this.tpLocalLicenses.Location = new System.Drawing.Point(4, 29);
            this.tpLocalLicenses.Name = "tpLocalLicenses";
            this.tpLocalLicenses.Padding = new System.Windows.Forms.Padding(3);
            this.tpLocalLicenses.Size = new System.Drawing.Size(1093, 186);
            this.tpLocalLicenses.TabIndex = 0;
            this.tpLocalLicenses.Text = "Local";
            this.tpLocalLicenses.UseVisualStyleBackColor = true;
            // 
            // lblLocalLicensesRecordsCount
            // 
            this.lblLocalLicensesRecordsCount.AutoSize = true;
            this.lblLocalLicensesRecordsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalLicensesRecordsCount.ForeColor = System.Drawing.Color.Brown;
            this.lblLocalLicensesRecordsCount.Location = new System.Drawing.Point(119, 158);
            this.lblLocalLicensesRecordsCount.Name = "lblLocalLicensesRecordsCount";
            this.lblLocalLicensesRecordsCount.Size = new System.Drawing.Size(49, 20);
            this.lblLocalLicensesRecordsCount.TabIndex = 26;
            this.lblLocalLicensesRecordsCount.Text = "? ? ?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "# Records :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 20);
            this.label1.TabIndex = 24;
            this.label1.Text = "Local Licenses History :";
            // 
            // dgvLocalLicensesHistory
            // 
            this.dgvLocalLicensesHistory.AllowUserToAddRows = false;
            this.dgvLocalLicensesHistory.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvLocalLicensesHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLocalLicensesHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLocalLicensesHistory.ColumnHeadersHeight = 25;
            this.dgvLocalLicensesHistory.ContextMenuStrip = this.cmsLocalLicensesHistory;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLocalLicensesHistory.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLocalLicensesHistory.EnableHeadersVisualStyles = false;
            this.dgvLocalLicensesHistory.Location = new System.Drawing.Point(16, 36);
            this.dgvLocalLicensesHistory.Name = "dgvLocalLicensesHistory";
            this.dgvLocalLicensesHistory.ReadOnly = true;
            this.dgvLocalLicensesHistory.RowTemplate.Height = 20;
            this.dgvLocalLicensesHistory.Size = new System.Drawing.Size(1061, 119);
            this.dgvLocalLicensesHistory.TabIndex = 23;
            // 
            // tpInternationalLicanses
            // 
            this.tpInternationalLicanses.Controls.Add(this.lblInternationalLicensesRecordsCount);
            this.tpInternationalLicanses.Controls.Add(this.label4);
            this.tpInternationalLicanses.Controls.Add(this.label5);
            this.tpInternationalLicanses.Controls.Add(this.dgvInternationalLicensesHistory);
            this.tpInternationalLicanses.Location = new System.Drawing.Point(4, 29);
            this.tpInternationalLicanses.Name = "tpInternationalLicanses";
            this.tpInternationalLicanses.Padding = new System.Windows.Forms.Padding(3);
            this.tpInternationalLicanses.Size = new System.Drawing.Size(1093, 186);
            this.tpInternationalLicanses.TabIndex = 1;
            this.tpInternationalLicanses.Text = "International";
            this.tpInternationalLicanses.UseVisualStyleBackColor = true;
            // 
            // lblInternationalLicensesRecordsCount
            // 
            this.lblInternationalLicensesRecordsCount.AutoSize = true;
            this.lblInternationalLicensesRecordsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInternationalLicensesRecordsCount.ForeColor = System.Drawing.Color.Brown;
            this.lblInternationalLicensesRecordsCount.Location = new System.Drawing.Point(119, 158);
            this.lblInternationalLicensesRecordsCount.Name = "lblInternationalLicensesRecordsCount";
            this.lblInternationalLicensesRecordsCount.Size = new System.Drawing.Size(49, 20);
            this.lblInternationalLicensesRecordsCount.TabIndex = 30;
            this.lblInternationalLicensesRecordsCount.Text = "? ? ?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 29;
            this.label4.Text = "# Records :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 20);
            this.label5.TabIndex = 28;
            this.label5.Text = "International Licenses History :";
            // 
            // dgvInternationalLicensesHistory
            // 
            this.dgvInternationalLicensesHistory.AllowUserToAddRows = false;
            this.dgvInternationalLicensesHistory.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvInternationalLicensesHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInternationalLicensesHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvInternationalLicensesHistory.ColumnHeadersHeight = 25;
            this.dgvInternationalLicensesHistory.ContextMenuStrip = this.cmsInternationalLicensesHistory;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInternationalLicensesHistory.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvInternationalLicensesHistory.EnableHeadersVisualStyles = false;
            this.dgvInternationalLicensesHistory.Location = new System.Drawing.Point(16, 36);
            this.dgvInternationalLicensesHistory.Name = "dgvInternationalLicensesHistory";
            this.dgvInternationalLicensesHistory.ReadOnly = true;
            this.dgvInternationalLicensesHistory.RowTemplate.Height = 20;
            this.dgvInternationalLicensesHistory.Size = new System.Drawing.Size(1061, 119);
            this.dgvInternationalLicensesHistory.TabIndex = 27;
            // 
            // cmsInternationalLicensesHistory
            // 
            this.cmsInternationalLicensesHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory});
            this.cmsInternationalLicensesHistory.Name = "cmsInternationalLicensesHistory";
            this.cmsInternationalLicensesHistory.Size = new System.Drawing.Size(186, 42);
            // 
            // showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory
            // 
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.Image = global::DVLD.Properties.Resources.License_View_32;
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.Name = "showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory";
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.Size = new System.Drawing.Size(185, 38);
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.Text = "Show License Info";
            this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory.Click += new System.EventHandler(this.showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory_Click);
            // 
            // cmsLocalLicensesHistory
            // 
            this.cmsLocalLicensesHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory});
            this.cmsLocalLicensesHistory.Name = "cmsLocalLicensesHistory";
            this.cmsLocalLicensesHistory.Size = new System.Drawing.Size(186, 42);
            // 
            // showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory
            // 
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.Image = global::DVLD.Properties.Resources.License_View_32;
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.Name = "showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory";
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.Size = new System.Drawing.Size(185, 38);
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.Text = "Show License Info";
            this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory.Click += new System.EventHandler(this.showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory_Click);
            // 
            // ctrlDriverLicenses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDriverLicenses);
            this.Name = "ctrlDriverLicenses";
            this.Size = new System.Drawing.Size(1119, 268);
            this.gbDriverLicenses.ResumeLayout(false);
            this.tcDriverLicenses.ResumeLayout(false);
            this.tpLocalLicenses.ResumeLayout(false);
            this.tpLocalLicenses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalLicensesHistory)).EndInit();
            this.tpInternationalLicanses.ResumeLayout(false);
            this.tpInternationalLicanses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInternationalLicensesHistory)).EndInit();
            this.cmsInternationalLicensesHistory.ResumeLayout(false);
            this.cmsLocalLicensesHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDriverLicenses;
        private System.Windows.Forms.TabControl tcDriverLicenses;
        private System.Windows.Forms.TabPage tpLocalLicenses;
        private System.Windows.Forms.TabPage tpInternationalLicanses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvLocalLicensesHistory;
        private System.Windows.Forms.Label lblLocalLicensesRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInternationalLicensesRecordsCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvInternationalLicensesHistory;
        private System.Windows.Forms.ContextMenuStrip cmsInternationalLicensesHistory;
        private System.Windows.Forms.ToolStripMenuItem showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory;
        private System.Windows.Forms.ContextMenuStrip cmsLocalLicensesHistory;
        private System.Windows.Forms.ToolStripMenuItem showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory;
    }
}
