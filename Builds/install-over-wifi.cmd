@echo off
REM Get IP address
set /p device_ip=Device IP Address: 

REM Connect
echo.
echo Connecting to device
adb connect %device_ip%:5555

REM Install
echo.
echo Installing dashboard
adb install -r "Home Dashboard".apk

REM Start the app on the tablet
echo.
echo Restarting dashboard
adb shell monkey -p com.TomHewitt.HomeDashboard -c android.intent.category.LAUNCHER 1

REM Disconnect
echo.
echo Disconnecting from device
adb disconnect %device_ip%:5555