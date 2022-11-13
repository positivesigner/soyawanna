
namespace YouTubeChannel
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.YTDownloadBgw = new System.ComponentModel.BackgroundWorker();
            this.DownloadLbl = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "YouTube Channel";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(180, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(215, 26);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(102, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 48);
            this.button1.TabIndex = 2;
            this.button1.Text = "Export Video List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // YTDownloadBgw
            // 
            this.YTDownloadBgw.WorkerReportsProgress = true;
            this.YTDownloadBgw.WorkerSupportsCancellation = true;
            this.YTDownloadBgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.YTDownloadBgw_DoWork);
            this.YTDownloadBgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.YTDownloadBgw_ProgressChanged);
            this.YTDownloadBgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.YTDownloadBgw_RunWorkerCompleted);
            // 
            // DownloadLbl
            // 
            this.DownloadLbl.AutoSize = true;
            this.DownloadLbl.Location = new System.Drawing.Point(20, 157);
            this.DownloadLbl.Name = "DownloadLbl";
            this.DownloadLbl.Size = new System.Drawing.Size(177, 20);
            this.DownloadLbl.TabIndex = 3;
            this.DownloadLbl.Text = "Download Status: None";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(320, 92);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(143, 48);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.DownloadLbl);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker YTDownloadBgw;
        private System.Windows.Forms.Label DownloadLbl;
        private System.Windows.Forms.Button CancelBtn;
    }
}

