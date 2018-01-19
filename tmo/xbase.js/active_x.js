/*
通过网页修改activex安全设置，添加信任站点，禁用弹出窗口阻止程序 
信任站点的注册表项

HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Ranges\Range[*]

ActiveX的注册表项

HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\[0-4]\[*]

[0-4]

值         设置
------------------------------
0        我的电脑
1        本地 Intranet 区域
2        受信任的站点区域
3        Internet 区域
4        受限制的站点区域

[*]

1001     下载已签名的 ActiveX 控件
1004     下载未签名的 ActiveX 控件
1200     运行 ActiveX 控件和插件
1201     对没有标记为安全的 ActiveX 控件进行初始化和脚本运行
1405     对标记为可安全执行脚本的 ActiveX 控件执行脚本
2201     ActiveX 控件自动提示 **

弹出窗口阻止程序的注册表项

HKEY_CURRENT_USERHKCU\Software\Microsoft\Internet Explorer\New Windows\PopupMgr

具体脚本如下:

*/ 

var WshShell=new ActiveXObject("WScript.Shell");
//添加信任站点ip
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\ZoneMap\\Ranges\\Range100\\","");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\ZoneMap\\Ranges\\Range100\\http","2","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\ZoneMap\\Ranges\\Range100\\:Range","192.168.1.5");
//修改IE ActiveX安全设置 1本地 Intranet 区域2受信任的站点区域3Internet 区域
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\1001","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\1004","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\1200","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\1201","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\1405","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\1\\2201","0","REG_DWORD");

WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\1001","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\1004","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\1200","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\1201","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\1405","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\2\\2201","0","REG_DWORD");

WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\1001","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\1004","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\1200","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\1201","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\1405","0","REG_DWORD");
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\3\\2201","0","REG_DWORD");
//禁用Winxp弹出窗口阻止程序
WshShell.RegWrite("HKCU\\Software\\Microsoft\\Internet Explorer\\New Windows\\PopupMgr","no");
//-->


