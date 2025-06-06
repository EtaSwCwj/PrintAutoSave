@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

:: === [기본 설정] ===
set "BASE_DIR=%~dp0"
cd /d "%BASE_DIR%"
for %%* in (.) do set "OUTPUT_NAME=%%~nx*"

set "VERSION=ver0.1"
for /f "tokens=2 delims==." %%I in ('"wmic os get localdatetime /value"') do set datetime=%%I
set "DATE=%datetime:~0,8%"

:: === [중복 방지용 zip 파일명 결정] ===
set "ZIP_NAME=%OUTPUT_NAME%_%VERSION%_%DATE%.zip"
set /a COUNT=1
:CHECK_DUPLICATE
if exist "%ZIP_NAME%" (
    set "ZIP_NAME=%OUTPUT_NAME%_%VERSION%_%DATE%_%COUNT%.zip"
    set /a COUNT+=1
    goto CHECK_DUPLICATE
)

:: === [임시 압축 폴더 생성] ===
set "TMP_DIR=_zip_temp"
rmdir /s /q "%TMP_DIR%" >nul 2>&1
mkdir "%TMP_DIR%"

:: === [명시적 핵심 파일 지정] ===
set FILES=^
Program.cs ^
Service1.cs ^
Service1.Designer.cs ^
ProjectInstaller.cs ^
App.config ^
PrintAutoSave.csproj ^
Settings1.settings

:: === [명시적 파일 복사] ===
for %%F in (%FILES%) do (
    if exist "%%F" copy /Y "%%F" "%TMP_DIR%\" >nul
)

:: === [자동 감지: .cs, .resx, .config 파일 추가 복사] ===
for %%F in (*.cs *.resx *.config) do (
    if not exist "%TMP_DIR%\%%~nxF" (
        copy /Y "%%F" "%TMP_DIR%\" >nul
    )
)

:: === [force_paths.rmd 생성] ===
(
    echo # 자동 포함된 파일 목록
    echo [force_path]
) > "%TMP_DIR%\__force_paths.rmd"

pushd "%TMP_DIR%" >nul
for %%F in (*) do (
    echo %%F = %%F>> "__force_paths.rmd"
)
popd >nul

:: === [project_info.rmd 생성] ===
(
    echo [project_info]
    echo OUTPUT_NAME = %OUTPUT_NAME%
    echo project_version = %VERSION%
    echo zip_name = %ZIP_NAME%
    echo created_at = %DATE%
    echo.
    echo description = PrintAutoSave 프로젝트 백업 압축 파일입니다.
    echo framework = .NET Framework 4.8 (C# WinForms)
    echo compiler = Visual Studio + MSBuild
    echo structure = GUI ServiceBase 기반 프로젝트
    echo main_entry = Service1.cs
    echo gui_type = Windows Service (Non-interactive)
    echo notes = 이 GUI는 그림판 자동 저장 등을 위해 Windows Service 형태로 구현됨.
) > "%TMP_DIR%\__project_info.rmd"

:: === [압축 수행] ===
powershell -Command "Compress-Archive -Path '%TMP_DIR%\*' -DestinationPath '%ZIP_NAME%'"

:: === [정리 및 출력] ===
type "%TMP_DIR%\__project_info.rmd"
rmdir /s /q "%TMP_DIR%"

echo.
echo ✅ 압축 완료: %ZIP_NAME%
pause
