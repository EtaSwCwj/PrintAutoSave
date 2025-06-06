namespace PrintAutoSave
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
            this.components = new System.ComponentModel.Container();
            this.buttonTest = new System.Windows.Forms.Button();
            this.timerAutoSave = new System.Windows.Forms.Timer(this.components);
            this.labelTickTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(227, 114);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(168, 44);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "button1";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.button1_Click);
            // 
            // timerAutoSave
            // 
            this.timerAutoSave.Interval = 1000;
            this.timerAutoSave.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelTickTime
            // 
            this.labelTickTime.AutoSize = true;
            this.labelTickTime.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTickTime.Location = new System.Drawing.Point(73, 119);
            this.labelTickTime.Name = "labelTickTime";
            this.labelTickTime.Size = new System.Drawing.Size(89, 28);
            this.labelTickTime.TabIndex = 1;
            this.labelTickTime.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelTickTime);
            this.Controls.Add(this.buttonTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.Windows.Forms.Label labelTickTime;
    }
}

