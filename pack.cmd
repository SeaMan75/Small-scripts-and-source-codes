@echo off
setlocal enabledelayedexpansion

:: Путь к WinRAR
set WINRAR_PATH="C:\Program Files\WinRAR\WinRAR.exe"

:: Путь к папке, которую нужно упаковать
set FOLDER_PATH=PartsRequestManager

:: Получаем имя папки
for %%I in ("%FOLDER_PATH%") do set FOLDER_NAME=%%~nI

:: Инициализируем порядковый номер
set COUNTER=1

:: Проверяем наличие файлов с порядковыми номерами и увеличиваем порядковый номер, пока файл не станет уникальным
:CHECK_FILE
if exist "%FOLDER_PATH%\%FOLDER_NAME%.!COUNTER!.rar" (
    set /a COUNTER+=1
    goto CHECK_FILE
)

:: Создаем архив с уникальным именем без использования профилей
%WINRAR_PATH% a -r -cfg- -ep1 -inul %FOLDER_NAME%.!COUNTER!.rar %FOLDER_PATH%

endlocal
