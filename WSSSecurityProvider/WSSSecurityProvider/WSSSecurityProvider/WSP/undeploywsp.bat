@ECHO OFF
SET STSADM="C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\stsadm"
ECHO Retracting solution ...
%STSADM% -o retractsolution -n WSSSecurityProvider.wsp -url http://localhost -local
ECHO Deleting solution ...
%STSADM% -o deletesolution -n WSSSecurityProvider.wsp