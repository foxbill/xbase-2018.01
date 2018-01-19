namespace xbase.host
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtRegistCode = new System.Windows.Forms.TextBox();
            this.btnGetRegisterString = new System.Windows.Forms.Button();
            this.btnTestAccreditString = new System.Windows.Forms.Button();
            this.txtAccreditCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtRegistCode
            // 
            this.txtRegistCode.Location = new System.Drawing.Point(167, 31);
            this.txtRegistCode.Multiline = true;
            this.txtRegistCode.Name = "txtRegistCode";
            this.txtRegistCode.Size = new System.Drawing.Size(450, 94);
            this.txtRegistCode.TabIndex = 0;
            // 
            // btnGetRegisterString
            // 
            this.btnGetRegisterString.Location = new System.Drawing.Point(12, 3);
            this.btnGetRegisterString.Name = "btnGetRegisterString";
            this.btnGetRegisterString.Size = new System.Drawing.Size(147, 22);
            this.btnGetRegisterString.TabIndex = 3;
            this.btnGetRegisterString.Text = "获取注册字符串";
            this.btnGetRegisterString.UseVisualStyleBackColor = true;
            this.btnGetRegisterString.Click += new System.EventHandler(this.btnGetRegisterString_Click);
            // 
            // btnTestAccreditString
            // 
            this.btnTestAccreditString.Location = new System.Drawing.Point(11, 141);
            this.btnTestAccreditString.Name = "btnTestAccreditString";
            this.btnTestAccreditString.Size = new System.Drawing.Size(147, 22);
            this.btnTestAccreditString.TabIndex = 4;
            this.btnTestAccreditString.Text = "输入授权字符串";
            this.btnTestAccreditString.UseVisualStyleBackColor = true;
            this.btnTestAccreditString.Click += new System.EventHandler(this.btnTestAccreditString_Click);
            // 
            // txtAccreditCode
            // 
            this.txtAccreditCode.Location = new System.Drawing.Point(164, 169);
            this.txtAccreditCode.Multiline = true;
            this.txtAccreditCode.Name = "txtAccreditCode";
            this.txtAccreditCode.Size = new System.Drawing.Size(452, 106);
            this.txtAccreditCode.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(443, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "授权字符串(请将从xBase注册网站获得的授权码粘贴到此框，并点击输入授权字符)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "注册字符串(请将此字符串复制到xBase注册网站，获取授权码)";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(542, 362);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtMsg.Location = new System.Drawing.Point(164, 292);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(451, 49);
            this.txtMsg.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 397);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAccreditCode);
            this.Controls.Add(this.btnTestAccreditString);
            this.Controls.Add(this.btnGetRegisterString);
            this.Controls.Add(this.txtRegistCode);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xbase 系统注册";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRegistCode;
        private System.Windows.Forms.Button btnGetRegisterString;
        private System.Windows.Forms.Button btnTestAccreditString;
        private System.Windows.Forms.TextBox txtAccreditCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtMsg;
    }
}

