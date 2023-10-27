namespace B站视频历史评论删除工具
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.midText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonDel = new System.Windows.Forms.Button();
            this.rpidList = new System.Windows.Forms.ListBox();
            this.textComments = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cookieText = new System.Windows.Forms.TextBox();
            this.cookieLable = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.maxTextbox = new System.Windows.Forms.TextBox();
            this.maxSetLabel = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.logButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(147, 544);
            this.listBox1.TabIndex = 1;
            this.listBox1.DoubleClick += new System.EventHandler(this.SelectOidGetComments);
            // 
            // midText
            // 
            this.midText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.midText.Location = new System.Drawing.Point(91, 37);
            this.midText.MaxLength = 8;
            this.midText.Name = "midText";
            this.midText.Size = new System.Drawing.Size(88, 14);
            this.midText.TabIndex = 8;
            this.midText.Text = "21772791";
            this.midText.WordWrap = false;
            this.midText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyInputNumeral);
            this.midText.Leave += new System.EventHandler(this.MouseFocusLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(38, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "mid：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(117, 106);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(257, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 50);
            this.button2.TabIndex = 10;
            this.button2.Text = "最近一月的所有评论。";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.GetHistoryRecord);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainer1.Location = new System.Drawing.Point(491, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonDel);
            this.splitContainer1.Panel2.Controls.Add(this.rpidList);
            this.splitContainer1.Panel2.Controls.Add(this.textComments);
            this.splitContainer1.Size = new System.Drawing.Size(521, 585);
            this.splitContainer1.SplitterDistance = 147;
            this.splitContainer1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(16, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "视频OID号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDel
            // 
            this.buttonDel.Enabled = false;
            this.buttonDel.Location = new System.Drawing.Point(283, 12);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(75, 23);
            this.buttonDel.TabIndex = 10;
            this.buttonDel.Text = "删除";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // rpidList
            // 
            this.rpidList.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpidList.FormattingEnabled = true;
            this.rpidList.ItemHeight = 12;
            this.rpidList.Location = new System.Drawing.Point(0, 0);
            this.rpidList.Name = "rpidList";
            this.rpidList.Size = new System.Drawing.Size(132, 585);
            this.rpidList.TabIndex = 9;
            this.rpidList.DataSourceChanged += new System.EventHandler(this.DataSoureChange);
            this.rpidList.DoubleClick += new System.EventHandler(this.SelectRpid);
            // 
            // textComments
            // 
            this.textComments.Location = new System.Drawing.Point(138, 41);
            this.textComments.Multiline = true;
            this.textComments.Name = "textComments";
            this.textComments.Size = new System.Drawing.Size(232, 544);
            this.textComments.TabIndex = 8;
            this.textComments.TextChanged += new System.EventHandler(this.TextComment);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.TopRight;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(6, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(204, 16);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "评论数超过最大值的视频不判断。";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cookieText
            // 
            this.cookieText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cookieText.Location = new System.Drawing.Point(91, 67);
            this.cookieText.Name = "cookieText";
            this.cookieText.Size = new System.Drawing.Size(88, 14);
            this.cookieText.TabIndex = 23;
            this.cookieText.WordWrap = false;
            this.cookieText.Leave += new System.EventHandler(this.MouseFocusLeave);
            // 
            // cookieLable
            // 
            this.cookieLable.AutoSize = true;
            this.cookieLable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cookieLable.Location = new System.Drawing.Point(14, 66);
            this.cookieLable.Name = "cookieLable";
            this.cookieLable.Size = new System.Drawing.Size(71, 16);
            this.cookieLable.TabIndex = 22;
            this.cookieLable.Text = "cookie：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cookieText);
            this.groupBox1.Controls.Add(this.midText);
            this.groupBox1.Controls.Add(this.cookieLable);
            this.groupBox1.Location = new System.Drawing.Point(135, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 110);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "个人信息";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Highlight;
            this.groupBox2.Controls.Add(this.maxTextbox);
            this.groupBox2.Controls.Add(this.maxSetLabel);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(12, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 65);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // maxTextbox
            // 
            this.maxTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maxTextbox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.maxTextbox.Location = new System.Drawing.Point(53, 17);
            this.maxTextbox.Name = "maxTextbox";
            this.maxTextbox.Size = new System.Drawing.Size(60, 16);
            this.maxTextbox.TabIndex = 26;
            this.maxTextbox.Text = "0";
            this.maxTextbox.WordWrap = false;
            this.maxTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyInputNumeral);
            this.maxTextbox.Leave += new System.EventHandler(this.MouseFocusLeave);
            // 
            // maxSetLabel
            // 
            this.maxSetLabel.AutoSize = true;
            this.maxSetLabel.Location = new System.Drawing.Point(6, 17);
            this.maxSetLabel.Name = "maxSetLabel";
            this.maxSetLabel.Size = new System.Drawing.Size(41, 12);
            this.maxSetLabel.TabIndex = 1;
            this.maxSetLabel.Text = "最大值";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 297);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(485, 288);
            this.richTextBox1.TabIndex = 26;
            this.richTextBox1.Text = "";
            // 
            // logButton
            // 
            this.logButton.Location = new System.Drawing.Point(257, 193);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(75, 23);
            this.logButton.TabIndex = 27;
            this.logButton.Text = "log保存路径";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(152, 223);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 585);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox midText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textComments;
        private System.Windows.Forms.ListBox rpidList;
        private System.Windows.Forms.TextBox cookieText;
        private System.Windows.Forms.Label cookieLable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox maxTextbox;
        private System.Windows.Forms.Label maxSetLabel;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button logButton;
        private System.Windows.Forms.Button button1;
    }
}

