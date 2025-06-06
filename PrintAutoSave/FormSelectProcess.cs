using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace PrintAutoSave
{
    public partial class FormProcessSelect: System.Windows.Forms.Form
    {
        public Process SelectedProcess { get; private set; }
        public FormProcessSelect()
        {
            InitializeComponent();
        }

        private void FormProcessSelect_Load(object sender, EventArgs e)
        {
            LoadProcessList();
        }

        private void LoadProcessList()
        {
            listBoxProcesses.Items.Clear();
            Process[] processes = Process.GetProcessesByName("mspaint");
            foreach (var proc in processes)
            {
                string title = string.IsNullOrEmpty(proc.MainWindowTitle) ? "(제목 없음)" : proc.MainWindowTitle;
                listBoxProcesses.Items.Add($"{proc.Id} - {title}");
            }
        }
        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (listBoxProcesses.SelectedIndex < 0) return;

            string selectedText = listBoxProcesses.SelectedItem.ToString();
            int pid = int.Parse(selectedText.Split('-')[0].Trim());

            SelectedProcess = Process.GetProcessById(pid);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
