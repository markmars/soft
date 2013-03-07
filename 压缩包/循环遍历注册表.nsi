!include "LogicLib.nsh"
!include "WordFunc.nsh"
!include nsDialogs.nsh

!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\"
Var DisplayName
Var UninstallString

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
  StrCpy $0 0
loop:
  EnumRegKey $1 HKLM ${PRODUCT_UNINST_KEY} $0
  StrCmp $1 "" done
  IntOp $0 $0 + 1
  
  ReadRegStr $DisplayName HKLM "${PRODUCT_UNINST_KEY}\$1" "DisplayName"
  
  ${if} $DisplayName == "山西定额库2011"
    ReadRegStr $UninstallString HKLM "${PRODUCT_UNINST_KEY}\$1" "UninstallString"
	MessageBox MB_OK $DisplayName
	MessageBox MB_OK $UninstallString
	nsExec::ExecToStack $UninstallString
  ${Elseif} $DisplayName != "山西定额库2011"
  Goto loop
  ${EndIf}
  
done:
  
FunctionEnd

