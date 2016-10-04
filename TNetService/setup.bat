
::安装服务文件
@SET TNetServiceDir=%~dp0bin\Debug\TNetService.exe



@SET FrameworkDir=%WINDIR%\Microsoft.NET\Framework

@SET FrameworkVersion=v4.0.30319

@SET PATH=%FrameworkDir%\%FrameworkVersion%;%WINDIR%\System32;%PATH%;

@SET/p setup_type=输入1：安装、 2:卸载.输完并按车继续：

@echo off
if "%setup_type%" == "1" (
	@echo -----安装服务-----

	InstallUtil.exe %TNetServiceDir%
	
	::sc start TNetService

	pause

)   

if "%setup_type%" == "2" (
	@echo -----卸载服务-----

	InstallUtil.exe /u  %TNetServiceDir% 

	pause

)   
 

 

