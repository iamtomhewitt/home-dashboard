@echo off
REM Get IP address
set /p device_ip=Device IP Address: 

REM Connect
adb connect %device_ip%:5555

REM Uninstall
adb uninstall com.TomHewitt.HomeDashboard

REM Install
adb install "Home Dashboard".apk

REM Start the app on the tablet
adb shell monkey -p com.TomHewitt.HomeDashboard -c android.intent.category.LAUNCHER 1

REM Disconnect
adb disconnect %device_ip%:5555