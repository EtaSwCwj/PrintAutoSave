namespace PrintAutoSave
{
    partial class FormProcessSelect
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
            this.listBoxProcesses = new System.Windows.Forms.ListBox();
            this.buttonProcessSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxProcesses
            // 
            this.listBoxProcesses.FormattingEnabled = true;
            this.listBoxProcesses.ItemHeight = 15;
            this.listBoxProcesses.Location = new System.Drawing.Point(12, 12);
            this.listBoxProcesses.Name = "listBoxProcesses";
            this.listBoxProcesses.Size = new System.Drawing.Size(457, 664);
            this.listBoxProcesses.TabIndex = 0;
            // 
            // buttonProcessSelect
            // 
            this.buttonProcessSelect.Location = new System.Drawing.Point(21, 682);
            this.buttonProcessSelect.Name = "buttonProcessSelect";
            this.buttonProcessSelect.Size = new System.Drawing.Size(180, 51);
            this.buttonProcessSelect.TabIndex = 1;
            this.buttonProcessSelect.Text = "Select";
            this.buttonProcessSelect.UseVisualStyleBackColor = true;
            this.buttonProcessSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // FormProcessSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 745);
            this.Controls.Add(this.buttonProcessSelect);
            this.Controls.Add(this.listBoxProcesses);
            this.Name = "FormProcessSelect";
            this.Text = "프로세스 목록";
            this.Load += new System.EventHandler(this.FormProcessSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxProcesses;
        private System.Windows.Forms.Button buttonProcessSelect;
    }
}