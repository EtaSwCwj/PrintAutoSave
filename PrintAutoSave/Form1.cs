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
        // 외부 Win32 API 호출을 위한 선언
        [DllImport("user32.dll")]

        // ===================== 함수 정의 =====================
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // 외부 Win32 API 호출을 위한 선언
        [DllImport("user32.dll")]

        // ===================== 함수 정의 =====================
        private static extern IntPtr GetForegroundWindow();

        // 외부 Win32 API 호출을 위한 선언
        [DllImport("user32.dll")]

        // ===================== 함수 정의 =====================
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        // 외부 Win32 API 호출을 위한 선언
        [DllImport("user32.dll")]

        // ===================== 함수 정의 =====================
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
            // 폼의 모든 구성요소 초기화
            InitializeComponent();
        }

        // ===================== 함수 정의 =====================
        private void SaveCommand()
        {
            // 유효한 대상 프로세스가 있는지 확인
            if (selectedProcess == null || selectedProcess.HasExited)
                return;

            // 사용자 입력 감지되면 저장 보류
            // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
            if (IsUserActivelyInputting())
            {
                labelTickTime.Text = "[입력 감지됨 → 저장 생략]";
                return;
            }

            // 저장 실행
            IntPtr hWnd = selectedProcess.MainWindowHandle;
            if (hWnd == IntPtr.Zero) return;

            // 대상 윈도우를 포그라운드로 설정 (입력 가능하게)
            SetForegroundWindow(hWnd);
            System.Threading.Thread.Sleep(200);
            // Ctrl+S 키 입력을 그림판에 전송하여 저장 수행
            SendKeys.SendWait("^s"); // Ctrl + S
        }




        // ===================== 함수 정의 =====================
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

                    // 자동 저장 타이머 시작 또는 정지
                    //timerAutoSave.Start();
                    // 현재 그림판 파일명과 선택된 파일명 일치 여부 확인
                    CheckFilenameConsistency();

                    MessageBox.Show($"선택된 그림판 PID: {selectedProcess.Id}\n{saveIntervalSeconds}초마다 저장이 시작됩니다.");
                }
                else
                {
                    MessageBox.Show("선택된 프로세스가 없습니다.");
                }
            }
        }


        // ===================== 함수 정의 =====================
        private bool IsUserActivelyInputting()
        {
            // 키보드 입력
            for (int key = 8; key <= 255; key++)
            {
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                if ((GetAsyncKeyState((Keys)key) & 0x8000) != 0)
                    return true;
            }

            // 마우스 입력
            return
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                GetAsyncKeyState(Keys.LButton) < 0 ||
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                GetAsyncKeyState(Keys.RButton) < 0 ||
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                GetAsyncKeyState(Keys.MButton) < 0;
        }




        int tickCounter = 0;
        bool pendingSave = false;
        int saveIntervalSeconds = 10; // 기본값: 10초

        // ===================== 함수 정의 =====================
        private bool isHotkeyPressed = false; // 단축키 눌림 여부

        // ===================== 함수 정의 =====================
        private bool isHotkeyBlockActive = false; // 저장 차단 상태 플래그


        // ===================== 함수 정의 =====================
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTickTime.Text = $"Elapsed: {tickCounter} sec";

            // 🔸 단축키 눌림 상태 확인 및 플래그 제어
            // 등록된 단축키 조합이 있는지 확인
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

                    // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
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
                    // 단축키 눌림 상태로 진입
                    isHotkeyPressed = true;
                }
                else
                {
                    labelHotkeyTitle.Text = "단축키 해제됨";

                    // [1] 단축키는 떨어졌지만
                    if (isHotkeyPressed)
                    {
                        // [2] 키보드 입력이 아직 있다면 저장 보류
                        // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                        if (IsUserActivelyInputting())
                        {
                            labelTickTime.Text = "[단축키 해제됐지만 입력 중 → 저장 안함]";
                            return;
                        }

                        // [3] 진짜로 아무것도 안 눌렸을 때만 저장
                        // 단축키에서 손을 뗀 상태로 판단
                        isHotkeyPressed = false;
                        isHotkeyBlockActive = false;

                        SaveCommand();
                        // 현재 그림판 파일명과 선택된 파일명 일치 여부 확인
                        CheckFilenameConsistency();
                        // 저장된 파일을 다른 폴더로 이동 (타임스탬프 포함)
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
            // 유효한 대상 프로세스가 있는지 확인
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
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                if (IsUserActivelyInputting())
                {
                    pendingSave = true;
                    labelTickTime.Text = "[입력 중 → 저장 대기]";
                    return;
                }
                else
                {
                    SaveCommand();
                    // 현재 그림판 파일명과 선택된 파일명 일치 여부 확인
                    CheckFilenameConsistency();
                    // 저장된 파일을 다른 폴더로 이동 (타임스탬프 포함)
                    TryMoveSavedFile();
                    labelTickTime.Text = "[저장 완료]";
                    tickCounter = 0;
                    pendingSave = false;
                }
            }
            else
            {
                // 입력 감지: 키보드나 마우스가 눌려 있는지 확인
                if (!IsUserActivelyInputting())
                {
                    SaveCommand();
                    labelTickTime.Text = "[보류 후 저장 완료]";
                    // 저장된 파일을 다른 폴더로 이동 (타임스탬프 포함)
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





        // ===================== 함수 정의 =====================
        private void CheckFilenameConsistency()
        {
            // 유효한 대상 프로세스가 있는지 확인
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

        // 클래스 내부 상태 추적용 필드
        private string selectedImageFilePath = ""; // 내부 저장용


        // ===================== 함수 정의 =====================
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

                    // 현재 그림판 파일명과 선택된 파일명 일치 여부 확인
                    CheckFilenameConsistency();
                }
            }
        }


        // 클래스 내부 상태 추적용 필드
        private string targetFolderPath = ""; // 파일 이동 대상 폴더


        // ===================== 함수 정의 =====================
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



        // ===================== 함수 정의 =====================
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

        // ===================== 함수 정의 =====================
        private void button1StartAndPause_Click(object sender, EventArgs e)
        {
            if (SendStartPause == false)
            {
                button1StartAndPause.Text = "동작 중";
                // 자동 저장 타이머 시작 또는 정지
                timerAutoSave.Start();
                SendStartPause = true;
            }
            else if (SendStartPause == true)
            {
                button1StartAndPause.Text = "정지";
                // 자동 저장 타이머 시작 또는 정지
                timerAutoSave.Stop();
                SendStartPause = false;
            }
        }



        // ===================== 함수 정의 =====================
        private bool isSettingHotkey = false;
        private HashSet<Keys> hotkeyBuffer = new HashSet<Keys>();
        private List<Keys> userDefinedHotkeys = new List<Keys>();


        // ===================== 함수 정의 =====================
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


        // ===================== 함수 정의 =====================
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

        // ===================== 함수 정의 =====================
        private bool IsModifierKey(Keys key)
        {
            return key == Keys.Control || key == Keys.ControlKey ||
                   key == Keys.Shift || key == Keys.ShiftKey ||
                   key == Keys.Alt || key == Keys.Menu;
        }

    }
}
