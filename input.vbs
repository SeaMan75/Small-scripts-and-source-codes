' Получаем список раскладок из WMI
Set objShell = CreateObject("WScript.Shell")
languageID = objShell.AppActivate("user32.dll")

' Параметры для вывода названия текущей раскладки
currentLayout = "[EN]"

' Проверим.
Set objShell = CreateObject("WScript.Shell")
currentLang = objShell.RegRead("HKEY_CURRENT_USER\Keyboard Layout\Preload\1")

If currentLang = "00000419" Then
    currentLayout = "[RU]"
ElseIf currentLang = "00000415" Then
    currentLayout = "[PL]"
End If

' Показать InputBox с текущей раскладкой
input = InputBox("Project global name ", currentLayout)
WScript.Echo input