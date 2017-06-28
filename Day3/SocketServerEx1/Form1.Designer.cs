namespace SocketServerEx1
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
			this.txtPortNo = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
			this.lblTemp = new System.Windows.Forms.Label();
			this.lblHumidity = new System.Windows.Forms.Label();
			this.rdoOn = new System.Windows.Forms.RadioButton();
			this.rdoOff = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtPortNo
			// 
			this.txtPortNo.Location = new System.Drawing.Point(70, 23);
			this.txtPortNo.Name = "txtPortNo";
			this.txtPortNo.Size = new System.Drawing.Size(100, 21);
			this.txtPortNo.TabIndex = 0;
			this.txtPortNo.Text = "11000";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Port No.";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(176, 23);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(88, 23);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "서비스 시작";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// richTextBoxLog
			// 
			this.richTextBoxLog.Location = new System.Drawing.Point(15, 50);
			this.richTextBoxLog.Name = "richTextBoxLog";
			this.richTextBoxLog.Size = new System.Drawing.Size(249, 183);
			this.richTextBoxLog.TabIndex = 4;
			this.richTextBoxLog.Text = "";
			// 
			// lblTemp
			// 
			this.lblTemp.AutoSize = true;
			this.lblTemp.Location = new System.Drawing.Point(15, 30);
			this.lblTemp.Name = "lblTemp";
			this.lblTemp.Size = new System.Drawing.Size(65, 12);
			this.lblTemp.TabIndex = 5;
			this.lblTemp.Text = "온도: 0.0도";
			// 
			// lblHumidity
			// 
			this.lblHumidity.AutoSize = true;
			this.lblHumidity.Location = new System.Drawing.Point(15, 55);
			this.lblHumidity.Name = "lblHumidity";
			this.lblHumidity.Size = new System.Drawing.Size(63, 12);
			this.lblHumidity.TabIndex = 6;
			this.lblHumidity.Text = "습도: 0.0%";
			// 
			// rdoOn
			// 
			this.rdoOn.AutoSize = true;
			this.rdoOn.Location = new System.Drawing.Point(17, 79);
			this.rdoOn.Name = "rdoOn";
			this.rdoOn.Size = new System.Drawing.Size(79, 16);
			this.rdoOn.TabIndex = 8;
			this.rdoOn.TabStop = true;
			this.rdoOn.Text = "Power On";
			this.rdoOn.UseVisualStyleBackColor = true;
			// 
			// rdoOff
			// 
			this.rdoOff.AutoSize = true;
			this.rdoOff.Location = new System.Drawing.Point(102, 79);
			this.rdoOff.Name = "rdoOff";
			this.rdoOff.Size = new System.Drawing.Size(78, 16);
			this.rdoOff.TabIndex = 9;
			this.rdoOff.TabStop = true;
			this.rdoOff.Text = "Power Off";
			this.rdoOff.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblHumidity);
			this.groupBox1.Controls.Add(this.rdoOff);
			this.groupBox1.Controls.Add(this.lblTemp);
			this.groupBox1.Controls.Add(this.rdoOn);
			this.groupBox1.Location = new System.Drawing.Point(15, 239);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(249, 116);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Realtime Device Info";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 388);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.richTextBoxLog);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPortNo);
			this.Name = "Form1";
			this.Text = "Socket Server";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPortNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label lblTemp;
        private System.Windows.Forms.Label lblHumidity;
        private System.Windows.Forms.RadioButton rdoOn;
        private System.Windows.Forms.RadioButton rdoOff;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

