namespace LMS511LaserApplication
{
    partial class LMS511Form
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ResultText = new System.Windows.Forms.RichTextBox();
            this.StatusText = new System.Windows.Forms.RichTextBox();
            this.clearStateBtn = new System.Windows.Forms.Button();
            this.clearResultBtn = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.ReBootBtn = new System.Windows.Forms.Button();
            this.stopDriverBtn = new System.Windows.Forms.Button();
            this.startDriverBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ResultText
            // 
            this.ResultText.Location = new System.Drawing.Point(3, 89);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(495, 533);
            this.ResultText.TabIndex = 4;
            this.ResultText.Text = "";
            this.toolTip1.SetToolTip(this.ResultText, "Scan results text window");
            // 
            // StatusText
            // 
            this.StatusText.Location = new System.Drawing.Point(504, 89);
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(495, 533);
            this.StatusText.TabIndex = 7;
            this.StatusText.Text = "";
            this.toolTip1.SetToolTip(this.StatusText, "Status text window");
            // 
            // clearStateBtn
            // 
            this.clearStateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.clearStateBtn.Image = global::LMS511LaserApplication.Properties.Resources.clear;
            this.clearStateBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearStateBtn.Location = new System.Drawing.Point(504, 628);
            this.clearStateBtn.Name = "clearStateBtn";
            this.clearStateBtn.Size = new System.Drawing.Size(75, 40);
            this.clearStateBtn.TabIndex = 9;
            this.clearStateBtn.Text = "Clear";
            this.clearStateBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.clearStateBtn, "Clear  state window");
            this.clearStateBtn.UseVisualStyleBackColor = true;
            this.clearStateBtn.Click += new System.EventHandler(this.clearStateBtn_Click);
            // 
            // clearResultBtn
            // 
            this.clearResultBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.clearResultBtn.Image = global::LMS511LaserApplication.Properties.Resources.clear;
            this.clearResultBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.clearResultBtn.Location = new System.Drawing.Point(3, 628);
            this.clearResultBtn.Name = "clearResultBtn";
            this.clearResultBtn.Size = new System.Drawing.Size(75, 40);
            this.clearResultBtn.TabIndex = 5;
            this.clearResultBtn.Text = "Clear";
            this.clearResultBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.clearResultBtn, "Clear results window");
            this.clearResultBtn.UseVisualStyleBackColor = true;
            this.clearResultBtn.Click += new System.EventHandler(this.clearResultBtn_Click);
            // 
            // ExitBtn
            // 
            this.ExitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ExitBtn.Image = global::LMS511LaserApplication.Properties.Resources.exit;
            this.ExitBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExitBtn.Location = new System.Drawing.Point(921, 628);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(75, 40);
            this.ExitBtn.TabIndex = 3;
            this.ExitBtn.Text = "Exit";
            this.ExitBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.ExitBtn, "Exit from the application");
            this.ExitBtn.UseVisualStyleBackColor = true;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click);
            // 
            // ReBootBtn
            // 
            this.ReBootBtn.Enabled = false;
            this.ReBootBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ReBootBtn.Image = global::LMS511LaserApplication.Properties.Resources.restart;
            this.ReBootBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ReBootBtn.Location = new System.Drawing.Point(195, 12);
            this.ReBootBtn.Name = "ReBootBtn";
            this.ReBootBtn.Size = new System.Drawing.Size(103, 40);
            this.ReBootBtn.TabIndex = 2;
            this.ReBootBtn.Text = "ReBoot";
            this.ReBootBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.ReBootBtn, "Re start laser");
            this.ReBootBtn.UseVisualStyleBackColor = true;
            this.ReBootBtn.Click += new System.EventHandler(this.ReBootBtn_Click);
            // 
            // stopDriverBtn
            // 
            this.stopDriverBtn.Enabled = false;
            this.stopDriverBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stopDriverBtn.Image = global::LMS511LaserApplication.Properties.Resources.stop;
            this.stopDriverBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stopDriverBtn.Location = new System.Drawing.Point(104, 12);
            this.stopDriverBtn.Name = "stopDriverBtn";
            this.stopDriverBtn.Size = new System.Drawing.Size(75, 40);
            this.stopDriverBtn.TabIndex = 1;
            this.stopDriverBtn.Text = "Stop";
            this.stopDriverBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.stopDriverBtn, "Stop laser");
            this.stopDriverBtn.UseVisualStyleBackColor = true;
            this.stopDriverBtn.Click += new System.EventHandler(this.stopDriverBtn_Click);
            // 
            // startDriverBtn
            // 
            this.startDriverBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startDriverBtn.Image = global::LMS511LaserApplication.Properties.Resources.start;
            this.startDriverBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startDriverBtn.Location = new System.Drawing.Point(12, 12);
            this.startDriverBtn.Name = "startDriverBtn";
            this.startDriverBtn.Size = new System.Drawing.Size(75, 40);
            this.startDriverBtn.TabIndex = 0;
            this.startDriverBtn.Text = "Start";
            this.startDriverBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip1.SetToolTip(this.startDriverBtn, "Start laser");
            this.startDriverBtn.UseVisualStyleBackColor = true;
            this.startDriverBtn.Click += new System.EventHandler(this.startDriverBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Scan:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(501, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "State:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LMS511LaserApplication.Properties.Resources.logosick;
            this.pictureBox1.Location = new System.Drawing.Point(822, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 82);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // LMS511Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 680);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.clearStateBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StatusText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearResultBtn);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.ReBootBtn);
            this.Controls.Add(this.stopDriverBtn);
            this.Controls.Add(this.startDriverBtn);
            this.MaximizeBox = false;
            this.Name = "LMS511Form";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SICK LMS511 Laser  Tester";
            this.Load += new System.EventHandler(this.LMS511Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button startDriverBtn;
        private System.Windows.Forms.Button stopDriverBtn;
        private System.Windows.Forms.Button ReBootBtn;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.RichTextBox ResultText;
        private System.Windows.Forms.Button clearResultBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox StatusText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clearStateBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

