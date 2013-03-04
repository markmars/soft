!include "LogicLib.nsh"
!include "WordFunc.nsh"

Name "Example1"
OutFile "example1.exe"
InstallDir $DESKTOP\Example1
RequestExecutionLevel user

Page directory
Page instfiles

Section ""
  SetOutPath $INSTDIR
  File example1.nsi
SectionEnd

Function .onInit
	chongshi:
		FindProcDLL::FindProc "notepad.exe"
		Pop $R0
		${if} $R0 == "1"
		MessageBox MB_ABORTRETRYIGNORE "notepad.exe正在运行,请先关闭程序!" /SD IDABORT IDIGNORE next IDRETRY chongshi
		Quit
		${else}
		Goto next
		${Endif}
		
	next:
	MessageBox MB_OK "OK"
FunctionEnd












