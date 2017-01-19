@ECHO OFF
call undeploywsp.bat
ECHO Restarting IIS ...
iisreset
call createwsp.bat
call deploywsp.bat