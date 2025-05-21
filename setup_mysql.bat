@echo off
REM -- MySQL Configuration Script After Unattended Install --
REM Assumes MySQL is installed in the default location and added to PATH

set MYSQL_PATH="C:\Program Files\MySQL\MySQL Server 9.2\bin\mysql.exe"

echo Setting root password and user privileges...
%MYSQL_PATH% -u root --connect-expired-password < "%~dpcompanydump.sql"

echo Restoring companydump.sql into companymanagement...
%MYSQL_PATH% -u root -p -e "CREATE DATABASE IF NOT EXISTS companymanagement;"
%MYSQL_PATH% -u root -p companymanagement < "%~dp0companydump.sql"

echo MySQL setup and database restoration complete.
pause