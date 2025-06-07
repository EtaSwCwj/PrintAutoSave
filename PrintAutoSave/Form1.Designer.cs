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
            this.textBoxSaveInterval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowseFile = new System.Windows.Forms.Button();
            this.buttonBrowseFolder = new System.Windows.Forms.Button();
            this.labelSelectedFilePath = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelBrowseFolder = new System.Windows.Forms.Label();
            this.button1StartAndPause = new System.Windows.Forms.Button();
            this.labelHotkeyTitle = new System.Windows.Forms.Label();
            this.textBoxHotkey = new System.Windows.Forms.TextBox();
            this.buttonSetHotkey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(24, 54);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(168, 44);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "프로세스 선택";
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
            this.labelTickTime.Location = new System.Drawing.Point(19, 11);
            this.labelTickTime.Name = "labelTickTime";
            this.labelTickTime.Size = new System.Drawing.Size(96, 28);
            this.labelTickTime.TabIndex = 1;
            this.labelTickTime.Text = "타이밍";
            // 
            // textBoxSaveInterval
            // 
            this.textBoxSaveInterval.Location = new System.Drawing.Point(24, 153);
            this.textBoxSaveInterval.Name = "textBoxSaveInterval";
            this.textBoxSaveInterval.Size = new System.Drawing.Size(156, 25);
            this.textBoxSaveInterval.TabIndex = 2;
            this.textBoxSaveInterval.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(19, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "저장 인터벌(초)";
            // 
            // buttonBrowseFile
            // 
            this.buttonBrowseFile.Location = new System.Drawing.Point(24, 214);
            this.buttonBrowseFile.Name = "buttonBrowseFile";
            this.buttonBrowseFile.Size = new System.Drawing.Size(179, 44);
            this.buttonBrowseFile.TabIndex = 4;
            this.buttonBrowseFile.Text = "그림 파일 선택";
            this.buttonBrowseFile.UseVisualStyleBackColor = true;
            this.buttonBrowseFile.Click += new System.EventHandler(this.buttonBrowseFile_Click);
            // 
            // buttonBrowseFolder
            // 
            this.buttonBrowseFolder.Location = new System.Drawing.Point(24, 291);
            this.buttonBrowseFolder.Name = "buttonBrowseFolder";
            this.buttonBrowseFolder.Size = new System.Drawing.Size(179, 44);
            this.buttonBrowseFolder.TabIndex = 5;
            this.buttonBrowseFolder.Text = "이동 폴더 선택";
            this.buttonBrowseFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseFolder.Click += new System.EventHandler(this.buttonBrowseFolder_Click);
            // 
            // labelSelectedFilePath
            // 
            this.labelSelectedFilePath.AutoSize = true;
            this.labelSelectedFilePath.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSelectedFilePath.Location = new System.Drawing.Point(21, 194);
            this.labelSelectedFilePath.Name = "labelSelectedFilePath";
            this.labelSelectedFilePath.Size = new System.Drawing.Size(81, 17);
            this.labelSelectedFilePath.TabIndex = 6;
            this.labelSelectedFilePath.Text = "파일 주소";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStatus.Location = new System.Drawing.Point(566, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(211, 28);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "저장 인터벌(초)";
            // 
            // labelBrowseFolder
            // 
            this.labelBrowseFolder.AutoSize = true;
            this.labelBrowseFolder.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBrowseFolder.Location = new System.Drawing.Point(21, 271);
            this.labelBrowseFolder.Name = "labelBrowseFolder";
            this.labelBrowseFolder.Size = new System.Drawing.Size(81, 17);
            this.labelBrowseFolder.TabIndex = 8;
            this.labelBrowseFolder.Text = "폴더 주소";
            // 
            // button1StartAndPause
            // 
            this.button1StartAndPause.Location = new System.Drawing.Point(246, 54);
            this.button1StartAndPause.Name = "button1StartAndPause";
            this.button1StartAndPause.Size = new System.Drawing.Size(168, 44);
            this.button1StartAndPause.TabIndex = 9;
            this.button1StartAndPause.Text = "시작";
            this.button1StartAndPause.UseVisualStyleBackColor = true;
            this.button1StartAndPause.Click += new System.EventHandler(this.button1StartAndPause_Click);
            // 
            // labelHotkeyTitle
            // 
            this.labelHotkeyTitle.AutoSize = true;
            this.labelHotkeyTitle.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelHotkeyTitle.Location = new System.Drawing.Point(19, 356);
            this.labelHotkeyTitle.Name = "labelHotkeyTitle";
            this.labelHotkeyTitle.Size = new System.Drawing.Size(161, 28);
            this.labelHotkeyTitle.TabIndex = 10;
            this.labelHotkeyTitle.Text = "단축키 설정";
            // 
            // textBoxHotkey
            // 
            this.textBoxHotkey.Location = new System.Drawing.Point(24, 396);
            this.textBoxHotkey.Name = "textBoxHotkey";
            this.textBoxHotkey.Size = new System.Drawing.Size(420, 25);
            this.textBoxHotkey.TabIndex = 11;
            this.textBoxHotkey.Text = "10";
            // 
            // buttonSetHotkey
            // 
            this.buttonSetHotkey.Location = new System.Drawing.Point(24, 433);
            this.buttonSetHotkey.Name = "buttonSetHotkey";
            this.buttonSetHotkey.Size = new System.Drawing.Size(168, 44);
            this.buttonSetHotkey.TabIndex = 12;
            this.buttonSetHotkey.Text = "단축키 입력";
            this.buttonSetHotkey.UseVisualStyleBackColor = true;
            this.buttonSetHotkey.Click += new System.EventHandler(this.buttonSetHotkey_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 542);
            this.Controls.Add(this.buttonSetHotkey);
            this.Controls.Add(this.textBoxHotkey);
            this.Controls.Add(this.labelHotkeyTitle);
            this.Controls.Add(this.button1StartAndPause);
            this.Controls.Add(this.labelBrowseFolder);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelSelectedFilePath);
            this.Controls.Add(this.buttonBrowseFolder);
            this.Controls.Add(this.buttonBrowseFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSaveInterval);
            this.Controls.Add(this.labelTickTime);
            this.Controls.Add(this.buttonTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Timer timerAutoSave;
        private System.Windows.Forms.Label labelTickTime;
        private System.Windows.Forms.TextBox textBoxSaveInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBrowseFile;
        private System.Windows.Forms.Button buttonBrowseFolder;
        private System.Windows.Forms.Label labelSelectedFilePath;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelBrowseFolder;
        private System.Windows.Forms.Button button1StartAndPause;
        private System.Windows.Forms.Label labelHotkeyTitle;
        private System.Windows.Forms.TextBox textBoxHotkey;
        private System.Windows.Forms.Button buttonSetHotkey;
    }
}

