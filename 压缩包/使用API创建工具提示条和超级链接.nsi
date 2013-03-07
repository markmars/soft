!include nsDialogs.nsh
#编写：水晶石
Name "link_tooltips"
OutFile "link_tooltips.exe"
XPStyle on
Var Link
Var tipS
Page custom nsDialogsPage

Function OnTimer
${Unless} $tips <> 0
System::Call USER32::CreateWindowEx(i0x00000008,t"tooltips_class32",i,i0x80000000,i,i,i,i,i,i,i0,i)i.s
  Pop $tipS
${EndUnless}
  System::Alloc 16
Pop $0
  System::Call USER32::GetCursorPos(ir0)
  System::Call *$0(i.r1,i.r2)
  System::Free $0
  System::Call USER32::WindowFromPoint(ir1,ir2)i.r1
${If} $1 = $Link
   StrCpy $R1 "超级链接"
   FindWindow $3 "#32770" "" $HWNDPARENT
   System::Call *(i0x28,i0x010,i$3,i0x409,i,i,i,i,i0,tR1)i.R1
   SendMessage $tipS 1028 0 $R1
   SendMessage $tipS 1033 0 $R1
   SendMessage $tipS 1025 1 0
   SendMessage $tipS 1041 1 $R1
   System::Free $R1
   SetCtlColors $Link  0xFF0000 transparent
   System::Call USER32::LoadCursor(i,i32649)i.s
   System::Call USER32::SetCursor(is)
${Else}
   SendMessage $tipS 1025 0 0
   SetCtlColors $Link  0x0000FF transparent
${EndIf}
  System::Call user32::RedrawWindow(i$Link,i0,i0,i0x0105)
FunctionEnd

Function nsDialogsPage
nsDialogs::Create 1018
  Pop $0
  ${NSD_CreateLabel} 100u 50u 100% 15u "访问我的博客"
  Pop $Link
  SetCtlColors $Link  0x0000FF transparent
  System::Call user32::SetClassLong(i$Link,i-12,i0)
  ${NSD_OnClick} $Link OnClick
  ${NSD_CreateTimer} OnTimer 50
nsDialogs::Show
FunctionEnd

function OnClick
ExecShell "open" "http://blog.csdn.net/shuijing_0"
FunctionEnd

Section
SectionEnd