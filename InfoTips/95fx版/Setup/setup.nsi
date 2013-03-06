!include "LogicLib.nsh"
!include "WordFunc.nsh"

; 安装程序初始定义常量
!define PRODUCT_PATH ""
!define PRODUCT_NAME "黄金外汇宝"
!define PRODUCT_VERSION "v2.0"
!define PRODUCT_PUBLISHER ""
!define PRODUCT_Install_KEY "Software\黄金外汇宝\"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_HKLM_ROOT_KEY "HKLM"
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_FUNCTION fucRunEXE
SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI2.nsh"

; MUI 预定义常量
;--------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON "logo.ico"
!define MUI_UNICON "logo.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP "左侧文件.bmp"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "顶部文件.bmp"

;-------------修复页面变量定义-----------------------
!include nsDialogs.nsh
Var UninstallFileName
Var RADIO_REPAIR
Var RADIO_REMOVE
Var Checkbox_State_REPAIR
Var Checkbox_State_REMOVE
Var Checkbox_State

;-------------页面定义---------------------------
; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 修复页面
Page custom nsDialogsRepair nsDialogsRepairLeave
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH
; 安装卸载确认页面
!insertmacro MUI_UNPAGE_CONFIRM
; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES
; 安装卸载完成页面
!insertmacro MUI_UNPAGE_FINISH
; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; ------ MUI 现代界面定义结束 ------
;名称
Name "${PRODUCT_NAME}"
;生成的文件名称
OutFile "..\${PRODUCT_NAME}${PRODUCT_VERSION}.exe"
;生成的安装路径
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
RequestExecutionLevel user

BrandingText ${PRODUCT_PUBLISHER}——做软件，我们更专业"

Section "NET Framework 2.0.50727"
ReadRegDWORD $0 HKLM 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727' Install
${If} $0 == ''
NSISdl::download /TRANSLATE2 '正在下载 %s' '正在连接...' '(剩余 1 秒)' '(剩余 1 分钟)' '(剩余 1 小时)' '(剩余 %u 秒)' '(剩余 %u 分钟)' '(剩余 %u 小时)' '已完成：%skB(%d%%) 大小：%skB 速度：%u.%01ukB/s' /TIMEOUT=7500 /NOIEPROXY 'http://download.microsoft.com/download/c/6/e/c6e88215-0178-4c6c-b5f3-158ff77b1f38/NetFx20SP2_x86.exe' '$EXEDIR\NetFx20SP2_x86.exe'
Pop $R0
StrCmp $R0 "success" 0 +3
MessageBox MB_YESNO|MB_ICONQUESTION ".net framework 2.0 已成功下载至：$\r$\n$\t$EXEDIR\NetFx20SP2_x86.exe$\r$\n是否立即执行安装程序？" IDNO +2
ExecWait '$EXEDIR\NetFx20SP2_x86.exe'
${Else}
DetailPrint "..NET Framework 2.0 already installed !!"
${EndIf}
SectionEnd

Section "外汇黄金宝" SecA
	; 主程序运行目录
	SetOutPath "$INSTDIR\"
	; 如果存在文件，覆盖掉
	SetOverwrite on
		File /r "..\Output\"
SectionEnd

Section -Post
	CreateShortCut "$DESKTOP\黄金外汇宝.lnk" "$INSTDIR\InfoTips.exe"	
	CreateShortCut "$SMPROGRAMS\黄金外汇宝\黄金外汇宝.lnk" "$INSTDIR\InfoTips.exe"
	CreateShortCut "$SMPROGRAMS\黄金外汇宝\卸载黄金外汇宝.lnk" "$INSTDIR\uninst.exe"
	
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}" "InstallPath" "$INSTDIR"

	WriteUninstaller "$INSTDIR\uninst.exe"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
SectionEnd

; 页面加载之前进行初始化
Function .onInit
	ReadRegStr $UninstallFileName ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"
	ReadRegStr $INSTDIR ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}" "InstallPath"

	${if} $INSTDIR == ""
		StrCpy $INSTDIR "$PROGRAMFILES\黄金外汇宝\"
	${EndIf}
FunctionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
	; 删除文件
	RMDir /r $INSTDIR
	;删除快捷方式
	Delete "$DESKTOP\黄金外汇宝.lnk"
	Delete "$SMPROGRAMS\黄金外汇宝\黄金外汇宝.lnk"
	Delete "$SMPROGRAMS\黄金外汇宝\卸载黄金外汇宝.lnk"
	Delete "$SMPROGRAMS\黄金外汇宝\"
	
	;卸载安装目录的注册表
	DeleteRegKey ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
	DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"

	;刷新页面
	System::Call 'shell32.dll::SHChangeNotify(i, i, i, i) v (0x08000000, 0, 0, 0)'
	SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#
/******************************
 *  以下是安装程序的修复部分  *
 ******************************/

Function nsDialogsRepairLeave
	${NSD_GetState} $RADIO_REPAIR $Checkbox_State_REPAIR
	${NSD_GetState} $RADIO_REMOVE $Checkbox_State_REMOVE
	${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
		Exec $UninstallFileName
		Quit
	${EndIf}
FunctionEnd

Function fucRunEXE
	Exec "$INSTDIR\InfoTips.exe"
FunctionEnd

Function nsDialogsRepair
	${if} $UninstallFileName == ""
		Abort
	${EndIf}
	
	!insertmacro MUI_HEADER_TEXT "已经安装" "选择您要执行的操作"

	nsDialogs::Create /NOUNLOAD 1018

	${NSD_CreateLabel} 10u 0u 300u 30u "软件已经安装，请选择您要执行的操作，并点击『下一步(N)』继续"
	
	${NSD_CreateRadioButton}  40u 30u 100u 30u "修复或重新安装"
	Pop $RADIO_REPAIR
	${If} $Checkbox_State_REPAIR == ${BST_CHECKED}
		${NSD_Check} $RADIO_REPAIR
		${NSD_GetState} $RADIO_REPAIR $Checkbox_State
	${EndIf}

	${NSD_CreateRadioButton}  40u 60u 100u 30u "卸载"
	Pop $RADIO_REMOVE
	;${NSD_SetState} $RADIO_REMOVE 1
	${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
		${NSD_Check} $RADIO_REMOVE
		${NSD_GetState} $RADIO_REMOVE $Checkbox_State
	${EndIf}

	${If} $Checkbox_State <> ${BST_CHECKED}
		${NSD_Check} $RADIO_REPAIR
	${EndIf}
	nsDialogs::Show
FunctionEnd