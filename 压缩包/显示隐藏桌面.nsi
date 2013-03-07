!include nsDialogs.nsh
XPStyle on
ChangeUI all '${NSISDIR}\Contrib\UIs\sdbarker_tiny.exe'
Name 隐藏桌面

OutFile 隐藏桌面.exe

Page custom HideDesktop

Function HideDesktop
   nsDialogs::Create /NOUNLOAD 1018
Pop $0
${NSD_CreateButton} 40 60 70 40 "隐藏桌面"
pop $1
${NSD_OnClick} $1 OnButton
${NSD_CreateButton} 280 60 70 40 "显示桌面"
pop $1
   ${NSD_OnClick} $1 OnButton
   ${NSD_CreateLabel} 140 70 100% 100u "无聊制作，纯属恶搞。"
Pop $7
   nsDialogs::Show
FunctionEnd

Section ""
SectionEnd

Function OnButton
Pop $1 # HWND
FindWindow $4 "Progman" "Program Manager"
FindWindow $5 "SHELLDLL_DefView" "" $4 0
FindWindow $6 "SysListView32" "" $5 0
${NSD_GetText} $1 $3
${If} $3 == "隐藏桌面"
ShowWindow $6 0
${ElseIf} $3 == "显示桌面"
ShowWindow $6 1
${EndIf}
System::Call "user32.dll::PostMessage(i$6, i256, i0x74, i0)"
System::Call "user32.dll::PostMessage(i$6, i257, i0x74, i1)" #刷新下桌面
FunctionEnd