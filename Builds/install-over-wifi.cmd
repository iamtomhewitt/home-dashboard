@echo off
REM Get IP address
set /p device_ip=Device IP Address: 

REM Connect
adb connect %device_ip%:5555

REM REM Uninstall
REM adb uninstall com.TomHewitt.HomeDashboard

REM REM Install
REM adb install "Home Dashboard".apk

REM Disconnect
adb disconnect %device_ip%:5555