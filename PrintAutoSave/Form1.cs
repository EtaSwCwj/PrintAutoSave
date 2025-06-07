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
using System.IO;

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

                    // 💡 저장 간격 텍스트박스에서 읽기
                    if (!int.TryParse(textBoxSaveInterval.Text, out int intervalSec) || intervalSec <= 0)
                    {
                        MessageBox.Show("유효한 숫자를 입력하세요 (초 단위)");
                        return;
                    }

                    saveIntervalSeconds = intervalSec;
                    tickCounter = 0;
                    pendingSave = false;

                    //timerAutoSave.Start();
                    CheckFilenameConsistency();

                    MessageBox.Show($"선택된 그림판 PID: {selectedProcess.Id}\n{saveIntervalSeconds}초마다 저장이 시작됩니다.");
                }
                else
                {
                    MessageBox.Show("선택된 프로세스가 없습니다.");
                }
            }
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
        int saveIntervalSeconds = 10; // 기본값: 10초
        private bool isHotkeyPressed = false; // 단축키 눌림 여부
        private bool isHotkeyBlockActive = false; // 저장 차단 상태 플래그

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTickTime.Text = $"Elapsed: {tickCounter} sec";

            // 🔸 단축키 눌림 상태 확인 및 플래그 제어
            if (userDefinedHotkeys.Count > 0)
            {
                bool allPressed = true;
                foreach (var key in userDefinedHotkeys)
                {
                    Keys checkKey = key;

                    // Modifier 키는 감지 가능한 키로 변환
                    if (key == Keys.Control) checkKey = Keys.ControlKey;
                    else if (key == Keys.Shift) checkKey = Keys.ShiftKey;
                    else if (key == Keys.Alt) checkKey = Keys.Menu;

                    if ((GetAsyncKeyState(checkKey) & 0x8000) == 0)
                    {
                        allPressed = false;
                        break;
                    }
                }

                if (allPressed)
                {
                    labelHotkeyTitle.Text = "단축키 눌림";
                    isHotkeyBlockActive = true;
                    isHotkeyPressed = true;
                }
                else
                {
                    labelHotkeyTitle.Text = "단축키 해제됨";
                    if (isHotkeyPressed)
                    {
                        isHotkeyPressed = false;
                        isHotkeyBlockActive = false;

                        SaveCommand();
                        CheckFilenameConsistency();
                        TryMoveSavedFile();

                        labelTickTime.Text = "[단축키 해제 → 저장됨]";
                        tickCounter = 0;
                        pendingSave = false;
                    }
                }
            }

            // 🔸 저장 차단 상태면 아무 것도 하지 않음
            if (isHotkeyBlockActive)
            {
                labelTickTime.Text = "[단축키 눌림 중 → 저장 차단]";
                return;
            }

            // 🔸 대상 프로세스가 유효한지 확인
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                tickCounter = 0;
                pendingSave = false;
                return;
            }

            // 🔸 포커스 확인
            IntPtr foreground = GetForegroundWindow();
            IntPtr selectedHwnd = selectedProcess.MainWindowHandle;

            if (foreground != selectedHwnd)
            {
                tickCounter = 0;
                pendingSave = false;
                labelTickTime.Text = "[포커스 없음]";
                return;
            }

            // 🔸 타이머 증가
            tickCounter++;

            if (tickCounter < saveIntervalSeconds)
                return;

            // 🔸 저장 조건 판단
            if (!pendingSave)
            {
                if (IsUserActivelyInputting())
                {
                    pendingSave = true;
                    labelTickTime.Text = "[입력 중 → 저장 대기]";
                    return;
                }
                else
                {
                    SaveCommand();
                    CheckFilenameConsistency();
                    TryMoveSavedFile();
                    labelTickTime.Text = "[저장 완료]";
                    tickCounter = 0;
                    pendingSave = false;
                }
            }
            else
            {
                if (!IsUserActivelyInputting())
                {
                    SaveCommand();
                    labelTickTime.Text = "[보류 후 저장 완료]";
                    TryMoveSavedFile();
                    tickCounter = 0;
                    pendingSave = false;
                }
                else
                {
                    labelTickTime.Text = "[입력 중 → 대기 연장]";
                }
            }
        }




        private void CheckFilenameConsistency()
        {
            if (selectedProcess == null || selectedProcess.HasExited || string.IsNullOrEmpty(selectedImageFilePath))
            {
                labelStatus.Text = "[선택 없음]";
                return;
            }

            string titleFromPaint = selectedProcess.MainWindowTitle; // 예: "제목 없음1.png - 그림판"
            string expectedFileName = Path.GetFileName(selectedImageFilePath); // 예: "제목 없음1.png"

            // 창 제목에서 ' - 그림판' 앞까지 잘라냄
            string fileNameInWindow = titleFromPaint.Split('-')[0].Trim();

            if (fileNameInWindow.Equals(expectedFileName, StringComparison.OrdinalIgnoreCase))
            {
                labelStatus.Text = "[파일명 일치]";
            }
            else
            {
                labelStatus.Text = "[⚠ 파일명 불일치]";
            }
        }

        private string selectedImageFilePath = ""; // 내부 저장용

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "저장된 그림 파일 선택";
                ofd.Filter = "이미지 파일 (*.png;*.bmp;*.jpg)|*.png;*.bmp;*.jpg|모든 파일 (*.*)|*.*";

                // Settings에서 마지막 경로 가져오기
                string lastFolder = Settings.Default.LastImageFolder;
                if (!string.IsNullOrEmpty(lastFolder) && Directory.Exists(lastFolder))
                {
                    ofd.InitialDirectory = lastFolder;
                }
                else
                {
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImageFilePath = ofd.FileName;
                    labelSelectedFilePath.Text = selectedImageFilePath;

                    // 📁 선택한 경로 저장 (폴더 경로만)
                    Settings.Default.LastImageFolder = Path.GetDirectoryName(ofd.FileName);
                    Settings.Default.Save();

                    CheckFilenameConsistency();
                }
            }
        }


        private string targetFolderPath = ""; // 파일 이동 대상 폴더

        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "이동할 폴더 선택";
                ofd.Filter = "폴더 선택|*.this.is.not.a.real.extension"; // 선택 막기용
                ofd.FileName = "이 폴더를 선택하려면 아무 파일도 선택하지 말고 '열기'를 누르세요";

                // 폴더 선택용으로 초기 경로 지정
                ofd.CheckFileExists = false;
                ofd.ValidateNames = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 선택된 파일의 경로에서 폴더만 추출
                    targetFolderPath = Path.GetDirectoryName(ofd.FileName);
                    labelBrowseFolder.Text = targetFolderPath;
                }
            }
        }


        private void TryMoveSavedFile()
        {
            if (string.IsNullOrEmpty(selectedImageFilePath) || string.IsNullOrEmpty(targetFolderPath))
                return;

            if (!File.Exists(selectedImageFilePath))
                return;

            string originalFileName = Path.GetFileNameWithoutExtension(selectedImageFilePath);
            string extension = Path.GetExtension(selectedImageFilePath);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string newFileName = $"{originalFileName}_{timestamp}{extension}";
            string destinationPath = Path.Combine(targetFolderPath, newFileName);

            try
            {
                File.Copy(selectedImageFilePath, destinationPath, true);
                Console.WriteLine($"[INFO] 파일 이동됨: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] 파일 이동 실패: {ex.Message}");
            }
        }

        bool SendStartPause = false;
        private void button1StartAndPause_Click(object sender, EventArgs e)
        {
            if(SendStartPause == false)
            {
                button1StartAndPause.Text = "동작 중";
                timerAutoSave.Start();
                SendStartPause = true;
            }
            else if(SendStartPause == true)
            {
                button1StartAndPause.Text = "정지";
                timerAutoSave.Stop();
                SendStartPause = false;
            }
        }


        private bool isSettingHotkey = false;
        private HashSet<Keys> hotkeyBuffer = new HashSet<Keys>();
        private List<Keys> userDefinedHotkeys = new List<Keys>();

        private void buttonSetHotkey_Click(object sender, EventArgs e)
        {
            if (!isSettingHotkey)
            {
                // 입력 대기 시작
                isSettingHotkey = true;
                hotkeyBuffer.Clear();
                labelHotkeyTitle.Text = "단축키 입력 중...";
                buttonSetHotkey.Text = "입력 완료";
            }
            else
            {
                // 단축키 설정 완료
                isSettingHotkey = false;
                userDefinedHotkeys = hotkeyBuffer.ToList();
                string hotkeyText = string.Join(" + ", userDefinedHotkeys);
                textBoxHotkey.Text = hotkeyText;
                // ❌ labelHotkeyTitle.Text = "현재 단축키:"; <-- 삭제하거나 주석 처리
                buttonSetHotkey.Text = "단축키 입력";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (isSettingHotkey)
            {
                // 누를 때마다 초기화하고 마지막 조합만 저장
                hotkeyBuffer.Clear();

                Keys normalizedKey = keyData;

                if (keyData == Keys.ControlKey || keyData == Keys.LControlKey || keyData == Keys.RControlKey)
                    normalizedKey = Keys.Control;
                else if (keyData == Keys.ShiftKey || keyData == Keys.LShiftKey || keyData == Keys.RShiftKey)
                    normalizedKey = Keys.Shift;
                else if (keyData == Keys.Menu || keyData == Keys.LMenu || keyData == Keys.RMenu)
                    normalizedKey = Keys.Alt;

                // 조합키 감지
                if ((keyData & Keys.Control) == Keys.Control) hotkeyBuffer.Add(Keys.Control);
                if ((keyData & Keys.Shift) == Keys.Shift) hotkeyBuffer.Add(Keys.Shift);
                if ((keyData & Keys.Alt) == Keys.Alt) hotkeyBuffer.Add(Keys.Alt);

                // 일반 키 추가
                Keys mainKey = keyData & Keys.KeyCode;
                if (!IsModifierKey(mainKey))
                    hotkeyBuffer.Add(mainKey);

                // 보기 좋게 정리해서 출력
                List<string> displayList = new List<string>();
                if (hotkeyBuffer.Contains(Keys.Control)) displayList.Add("Control");
                if (hotkeyBuffer.Contains(Keys.Shift)) displayList.Add("Shift");
                if (hotkeyBuffer.Contains(Keys.Alt)) displayList.Add("Alt");

                var others = hotkeyBuffer.Except(new[] { Keys.Control, Keys.Shift, Keys.Alt });
                foreach (var key in others)
                    displayList.Add(key.ToString());

                textBoxHotkey.Text = string.Join(" + ", displayList);

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // 보조 함수
        private bool IsModifierKey(Keys key)
        {
            return key == Keys.Control || key == Keys.ControlKey ||
                   key == Keys.Shift || key == Keys.ShiftKey ||
                   key == Keys.Alt || key == Keys.Menu;
        }

    }
}

