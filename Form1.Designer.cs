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
        private System.Windows.Forms.TextBox txtLog;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnBrowseIn = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowseOut = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.AllowDrop = true;
            this.txtInput.Location = new System.Drawing.Point(16, 20);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(483, 22);
            this.txtInput.TabIndex = 0;
            this.txtInput.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInput_DragDrop);
            this.txtInput.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInput_DragEnter);
            // 
            // btnBrowseIn
            // 
            this.btnBrowseIn.Location = new System.Drawing.Point(505, 19);
            this.btnBrowseIn.Name = "btnBrowseIn";
            this.btnBrowseIn.Size = new System.Drawing.Size(151, 25);
            this.btnBrowseIn.TabIndex = 1;
            this.btnBrowseIn.Text = "Select Project…";
            this.btnBrowseIn.UseVisualStyleBackColor = true;
            this.btnBrowseIn.Click += new System.EventHandler(this.btnBrowseIn_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(16, 60);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(483, 22);
            this.txtOutput.TabIndex = 2;
            // 
            // btnBrowseOut
            // 
            this.btnBrowseOut.Location = new System.Drawing.Point(505, 60);
            this.btnBrowseOut.Name = "btnBrowseOut";
            this.btnBrowseOut.Size = new System.Drawing.Size(151, 25);
            this.btnBrowseOut.TabIndex = 3;
            this.btnBrowseOut.Text = "New Destination…";
            this.btnBrowseOut.UseVisualStyleBackColor = true;
            this.btnBrowseOut.Click += new System.EventHandler(this.btnBrowseOut_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(16, 99);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(90, 30);
            this.btnExtract.TabIndex = 4;
            this.btnExtract.Text = "Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(16, 144);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(640, 124);
            this.txtLog.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(120, 105);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(536, 24);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Idle";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnExtract;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 285);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnBrowseOut);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnBrowseIn);
            this.Controls.Add(this.txtInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cubase Nuendo Preview Extractor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
