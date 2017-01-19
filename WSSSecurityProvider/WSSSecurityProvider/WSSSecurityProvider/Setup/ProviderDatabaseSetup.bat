@SET SPDIR=microsoft shared\web server extensions\12
@SET FORCE=-force
@echo off

if (%1)==() goto NoParam

rem 
echo create aspnetdb database
rem
%windir%\Microsoft.NET\Framework\v2.0.50727\aspnet_regsql -A all -E -d SecProviderDB -S %1%


rem
echo create Sitemaps table and Stored Procedures for SQL Auth
rem
osql -E -S %1 -d SecProviderDB -i sitemaps.sql -n
rem 


goto exit

:NoParam
echo usage: ProviderDatabaseSetup "servername"
echo.

:exit