using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PrintAutoSave
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }


        private Process selectedProcess = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void SaveCommand()
        {
            if (selectedProcess == null || selectedProcess.HasExited)
                return;

            IntPtr hWnd = selectedProcess.MainWindowHandle;

            if (hWnd == IntPtr.Zero)
                return;

            SetForegroundWindow(hWnd);
            System.Threading.Thread.Sleep(200);
            SendKeys.SendWait("^s"); // Ctrl + S
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FormProcessSelect selectForm = new FormProcessSelect())
            {
                var result = selectForm.ShowDialog(this);

                if (result == DialogResult.OK && selectForm.SelectedProcess != null)
                {
                    selectedProcess = selectForm.SelectedProcess;
                    timerAutoSave.Start();
                    MessageBox.Show($"선택된 그림판 PID: {selectedProcess.Id}\n10초마다 저장이 시작됩니다.");
                }
                else
                {
                    MessageBox.Show("선택이 취소되었습니다.");
                }
            }
        }

        private bool IsUserRecentlyActive()
        {
            LASTINPUTINFO lii = new LASTINPUTINFO();
            lii.cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO));
            GetLastInputInfo(ref lii);

            uint idleTimeMs = (uint)Environment.TickCount - lii.dwTime;

            // 2초 이내 입력이 있으면 true
            return idleTimeMs < 2000;
        }

        private bool IsUserActivelyInputting()
        {
            // 체크할 키나 마우스 입력만 간단히 나열
            return
                GetAsyncKeyState(Keys.LButton) < 0 || // 마우스 왼쪽
                GetAsyncKeyState(Keys.RButton) < 0 || // 마우스 오른쪽
                GetAsyncKeyState(Keys.ControlKey) < 0 || // Ctrl
                GetAsyncKeyState(Keys.A) < 0 || // 예시: 일반 키
                GetAsyncKeyState(Keys.Enter) < 0; // 엔터 등
        }


        int tickCounter = 0;
        bool pendingSave = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTickTime.Text = $"Elapsed: {tickCounter} sec";

            if (selectedProcess == null || selectedProcess.HasExited)
            {
                tickCounter = 0;
                pendingSave = false;
                return;
            }

            IntPtr foreground = GetForegroundWindow();
            IntPtr selectedHwnd = selectedProcess.MainWindowHandle;

            if (foreground != selectedHwnd)
            {
                tickCounter = 0;
                pendingSave = false;
                labelTickTime.Text = "[포커스 없음]";
                return;
            }

            tickCounter++;

            if (tickCounter < 10)
                return;

            // 10초 경과 이후: SaveCommand 조건 판단
            if (pendingSave == false)
            {
                // 10초 도달 → 저장 시도 시작
                if (IsUserActivelyInputting())
                {
                    pendingSave = true; // 저장은 보류
                    labelTickTime.Text = "[입력 중 → 저장 대기]";
                    return;
                }
                else
                {
                    SaveCommand();
                    labelTickTime.Text = "[저장 완료]";
                    tickCounter = 0;
                    pendingSave = false;
                }
            }
            else
            {
                // 보류 상태 → 계속 입력 체크
                if (!IsUserActivelyInputting())
                {
                    SaveCommand();
                    labelTickTime.Text = "[보류 후 저장 완료]";
                    tickCounter = 0;
                    pendingSave = false;
                }
                else
                {
                    labelTickTime.Text = "[입력 중 → 대기 연장]";
                }
            }
        }

    }
}

