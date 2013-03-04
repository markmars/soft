!include "LogicLib.nsh"
!include "WordFunc.nsh"

; 安装程序初始定义常量
!define PRODUCT_NAME "软件名称"
!define PRODUCT_Version "1.0.0.0"
!define PRODUCT_PUBLISHER "公司名称"
!define PRODUCT_Install_KEY "Software\公司名称\软件名称\"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_HKLM_ROOT_KEY "HKLM"
SetCompressor lzma
SetCompressorDictSize 32

;-------------变量定义-----------------------
!include nsDialogs.nsh
Var UninstallFileName
Var RADIO_REPAIR
Var RADIO_REMOVE
Var Checkbox_State_REPAIR
Var Checkbox_State_REMOVE
Var Checkbox_State

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI2.nsh"

;页面预定义常量
;Interface Configuration
!define MUI_ABORTWARNING
!define MUI_ICON "..\..\Logo\安装程序.ico";安装图标
!define MUI_UNICON "..\..\Logo\安装程序.ico" ;卸载图标
!define MUI_PAGE_HERDER_TEXT "欢迎使用xxxxxx软件" ;显示向导头部信息（所有页面适用）
!define MUI_PAGE_HERDER_STEXT "这里是子标题" ;子标题
!define MUI_WELCOMEFINISHPAGE_BITMAP "..\..\Logo\左侧文件.bmp"
!define MUI_HEADERIMAGE;定义头部图片
!define MUI_HEADERIMAGE_RIGHT;定义上面图片在右边显示
!define MUI_HEADERIMAGE_BITMAP "..\..\Logo\顶部文件.bmp";图片路径

;-------------页面定义---------------------------
; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 修复页面
Page custom nsDialogsRepair nsDialogsRepairLeave
; 许可协议页面
; !define MUI_LICENSEPAGE_RADIOBUTTONS
; !define MUI_LICENSEPAGE_TEXT_TOP "这是授权页顶部"
; !define MUI_LICENSEPAGE_TEXT_BOTTOM "这是授权页底部"
; !insertmacro MUI_PAGE_LICENSE "SoftwareLicence.txt"
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_SHOWREADME
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION createZhuoMian
!define MUI_FINISHPAGE_SHOWREADME_TEXT "创建桌面快捷方式"
; !define MUI_FINISHPAGE_SHOWREADME
; !define MUI_FINISHPAGE_SHOWREADME_FUNCTION createKaiShiCaiDan
; !define MUI_FINISHPAGE_SHOWREADME_TEXT "创建开始菜单"
; !define MUI_FINISHPAGE_SHOWREADME
; !define MUI_FINISHPAGE_SHOWREADME_FUNCTION readme
; !define MUI_FINISHPAGE_SHOWREADME_TEXT "查看使用说明"
!define MUI_FINISHPAGE_RUN "notepad.exe"
!define MUI_FINISHPAGE_RUN_TEXT "运行程序"
!insertmacro MUI_PAGE_FINISH
; 卸载向导欢迎界面
;!insertmacro MUI_UNPAGE_WELCOME
; 安装卸载路径页面
!insertmacro MUI_UNPAGE_CONFIRM
; 卸载向导组件信息
;!insertmacro MUI_UNPAGE_COMPONENTS
; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES
; 安装卸载完成页面
!insertmacro MUI_UNPAGE_FINISH
; 安装预释放文件
;!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"
; ------ MUI 现代界面定义结束 ------

;名称
Name "${PRODUCT_NAME}${PRODUCT_Version}"
;生成的文件名称
OutFile "${PRODUCT_NAME}${PRODUCT_Version}.exe"
;生成默认安装路径
InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
;检测以前的安装路径
InstallDirRegKey ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}" "InstallPath"
;显示安装详细信息
ShowInstDetails show
;显示卸载详细信息
ShowUninstDetails show
;设置协议页左下角灰色信息处
BrandingText ${PRODUCT_PUBLISHER}

Section "判断系统版本"
	System::Call "Kernel32::GetVersion(v) i .s"
	Pop $0
	IntOp $1 $0 & 0xFF
	DetailPrint "Windows 主版本: $1"
	IntOp $1 $0 & 0xFFFF
	IntOp $1 $1 >> 8
	DetailPrint "Windows 次版本: $1"
	IntCmpU $0 0x80000000 0 nt
	DetailPrint "Windows 95/98/Me"
	nt:
	DetailPrint "Windows NT/2000/XP"
SectionEnd

Section "判断NET Framework 3.5"
ReadRegDWORD $0 HKLM 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5' Install
${If} $0 == ''
NSISdl::download /TRANSLATE2 '正在下载 %s' '正在连接...' '(剩余 1 秒)' '(剩余 1 分钟)' '(剩余 1 小时)' '(剩余 %u 秒)' '(剩余 %u 分钟)' '(剩余 %u 小时)' '已完成：%skB(%d%%) 大小：%skB 速度：%u.%01ukB/s' /TIMEOUT=7500 /NOIEPROXY 'http://download.microsoft.com/download/2/0/E/20E90413-712F-438C-988E-FDAA79A8AC3D/dotnetfx35.exe' '$EXEDIR\dotnetfx35.exe'
Pop $R0
StrCmp $R0 "success" 0 +3
MessageBox MB_YESNO|MB_ICONQUESTION ".net framework 3.5 已成功下载至：$\r$\n$\t$EXEDIR\dotnetfx35.exe$\r$\n是否立即执行安装程序？" IDNO +2
ExecWait '$EXEDIR\dotnetfx35.exe'
${Else}
DetailPrint "..NET Framework 3.5 already installed !!"
${EndIf}
SectionEnd

Section "文件拷贝" SecA
	SetOutPath "$INSTDIR\";主程序运行目录
	SetOverwrite on ;如果存在文件，覆盖掉
	File /r "..\文件夹\"; 遍历子目录复制并压缩文件
	File "文件.exe"
SectionEnd

Section "-Post";"-"开头，隐藏区段
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}" "InstallPath" "$INSTDIR"
	WriteUninstaller "$INSTDIR\uninst.exe"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Version" "${PRODUCT_Version}"
	WriteRegStr ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_DIR_REGKEY}" "Path" "$INSTDIR\${PRODUCT_NAME}.exe"
SectionEnd

Section "Uninstall"
	;删除安装目录中的所有文件
	RMDir /r $INSTDIR
	
	;删除快捷方式
	Delete "$DESKTOP\桌面快捷方式.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_PUBLISHER}\开始菜单快捷方式.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_PUBLISHER}\"

	
	;卸载安装目录的注册表
	DeleteRegKey ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}"
	DeleteRegKey ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
	DeleteRegKey ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_DIR_REGKEY}"

	;刷新页面
	System::Call 'shell32.dll::SHChangeNotify(i, i, i, i) v (0x08000000, 0, 0, 0)'
	SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#
 
;安装程序加载前进行初始化
Function .onInit
	;安装程序开始时候的背景图片设置,插件目录在安装程序推出后自动删除
	InitPluginsDir
	File /oname=$PLUGINSDIR\splash.bmp "..\..\Logo\背景图片.bmp"
	splash::show 2000 $PLUGINSDIR\splash
	
	;读取卸载程序路径
	ReadRegStr $UninstallFileName ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"
	
	;读取以前的安装路径，如果以前没安装，则设置默认值
	;ReadRegStr $INSTDIR ${PRODUCT_HKLM_ROOT_KEY} "${PRODUCT_Install_KEY}" "InstallPath"
	;${if} $INSTDIR == ""
	;StrCpy $INSTDIR "$PROGRAMFILES\${PRODUCT_NAME}\"
	;${EndIf}
	
	;判断系统版本
	; ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion" CurrentVersion
	; strcmp $0 "" 98 nt
	; 98:
	; messagebox  MB_ICONINFORMATION|MB_OK "您使用的操作系统版本过低，无法安装，程序将退出！"
	; quit
	; nt:
FunctionEnd

Function .onInstSuccess
	; HideWindow
	; MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功安装到您的计算机。"
	; ;自动删除安装程序
	; System::Call 'kernel32::GetModuleFileNameA(i 0, t .s, i ${NSIS_MAX_STRLEN}) i'
	; Pop $0
	; Delete /REBOOTOK $0
FunctionEnd

Function un.onInit
	; MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
	; Abort
FunctionEnd

Function un.onUninstScess
HideWindow
MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd

Function readme
ExecShell "open" "http://www.baidu.com"
Functionend

Function createKaiShiCaiDan
	CreateDirectory "$SMPROGRAMS\${PRODUCT_PUBLISHER}";开始菜单
	CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\开始菜单快捷方式.lnk" "notepad.exe";开始菜单快捷方式
FunctionEnd

Function createZhuoMian
	CreateShortCut "$DESKTOP\桌面快捷方式.lnk" "notepad.exe";桌面快捷方式
FunctionEnd

Function nsDialogsRepairLeave
  ${NSD_GetState} $RADIO_REPAIR $Checkbox_State_REPAIR
  ${NSD_GetState} $RADIO_REMOVE $Checkbox_State_REMOVE
  ${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
    Exec $UninstallFileName
    Quit
  ${EndIf}
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
	;${NSD_SetState} $RADIO_REMOVE 1;默认选定
	${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
	${NSD_Check} $RADIO_REMOVE
	${NSD_GetState} $RADIO_REMOVE $Checkbox_State
	${EndIf}

	${If} $Checkbox_State <> ${BST_CHECKED}
	${NSD_Check} $RADIO_REPAIR
	${EndIf}
	nsDialogs::Show
FunctionEnd