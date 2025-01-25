::@echo off
setlocal

for /f %%i in ('powershell -NoProfile -Command "Get-Date -Format yyyy-MM-dd"') do set currentDate=%%i

for /f "tokens=*" %%i in ('cscript //nologo input.vbs') do set input=%%i

set "baseDir=F:\VK_Projects\LuckyGuy"

if not exist "%baseDir%" (
mkdir "%baseDir%"
)

cd /d "%baseDir%"

set "suffix=1"

:check_folder
if %suffix% LSS 10 (
	set "formattedSuffix=0%suffix%"
) else (
	set "formattedSuffix=%suffix%"
)

set "dateFolder=%currentDate%_%formattedSuffix%"

if exist "%dateFolder%" (
	set /a suffix+=1
	goto check_folder
)

set folder="%dateFolder% - %input%"
mkdir %folder%
cd %folder%
pause
mkdir Files
mkdir "Explanatory notes"
mkdir Examples
mkdir Project
cd "Explanatory notes"
type nul > "V1.0.0.1 - Explanatory notes.docx"
start "" "C:\Total Commander Extended\Totalcmd64.exe" /O/R="f:\VK_Projects\#CMD\"/L="%baseDir%\%folder%"
endlocal