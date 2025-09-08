namespace CprWavExtractor
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnBrowseIn;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnBrowseOut;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.CheckBox chkUseEOF;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblIn;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnBrowseIn = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowseOut = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.chkUseEOF = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblIn = new System.Windows.Forms.Label();
            this.lblOut = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.AllowDrop = true;
            this.txtInput.Location = new System.Drawing.Point(16, 32);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(480, 23);
            this.txtInput.TabIndex = 0;
            this.txtInput.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInput_DragEnter);
            this.txtInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInput_DragDrop);
            // 
            // btnBrowseIn
            // 
            this.btnBrowseIn.Location = new System.Drawing.Point(504, 31);
            this.btnBrowseIn.Name = "btnBrowseIn";
            this.btnBrowseIn.Size = new System.Drawing.Size(75, 25);
            this.btnBrowseIn.TabIndex = 1;
            this.btnBrowseIn.Text = "Browse…";
            this.btnBrowseIn.UseVisualStyleBackColor = true;
            this.btnBrowseIn.Click += new System.EventHandler(this.btnBrowseIn_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(16, 86);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(480, 23);
            this.txtOutput.TabIndex = 2;
            // 
            // btnBrowseOut
            // 
            this.btnBrowseOut.Location = new System.Drawing.Point(504, 85);
            this.btnBrowseOut.Name = "btnBrowseOut";
            this.btnBrowseOut.Size = new System.Drawing.Size(75, 25);
            this.btnBrowseOut.TabIndex = 3;
            this.btnBrowseOut.Text = "Save As…";
            this.btnBrowseOut.UseVisualStyleBackColor = true;
            this.btnBrowseOut.Click += new System.EventHandler(this.btnBrowseOut_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(16, 124);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(90, 30);
            this.btnExtract.TabIndex = 4;
            this.btnExtract.Text = "Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // chkUseEOF
            // 
            this.chkUseEOF.AutoSize = true;
            this.chkUseEOF.Location = new System.Drawing.Point(120, 130);
            this.chkUseEOF.Name = "chkUseEOF";
            this.chkUseEOF.Size = new System.Drawing.Size(222, 19);
            this.chkUseEOF.TabIndex = 5;
            this.chkUseEOF.Text = "If size looks wrong, cut to end of file";
            this.chkUseEOF.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 190);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(563, 190);
            this.txtLog.TabIndex = 7;
            // 
            // lblIn
            // 
            this.lblIn.AutoSize = true;
            this.lblIn.Location = new System.Drawing.Point(16, 12);
            this.lblIn.Name = "lblIn";
            this.lblIn.Size = new System.Drawing.Size(111, 15);
            this.lblIn.TabIndex = 8;
            this.lblIn.Text = "Cubase Project .cpr";
            // 
            // lblOut
            // 
            this.lblOut.AutoSize = true;
            this.lblOut.Location = new System.Drawing.Point(16, 66);
            this.lblOut.Name = "lblOut";
            this.lblOut.Size = new System.Drawing.Size(122, 15);
            this.lblOut.TabIndex = 9;
            this.lblOut.Text = "Output WAV file path";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(16, 163);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(563, 20);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Idle";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnExtract;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 394);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblOut);
            this.Controls.Add(this.lblIn);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.chkUseEOF);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnBrowseOut);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnBrowseIn);
            this.Controls.Add(this.txtInput);
            this.MinimumSize = new System.Drawing.Size(611, 433);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CPR → WAV Extractor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
