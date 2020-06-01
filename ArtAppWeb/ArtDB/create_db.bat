echo off

rem batc file script to create DB
rem 9/5/2019

sqlcmd -S localhost -E -i ArtAppDB.sql
rem sqlcmd -S localhost/mssqlserver -E -i ArtAppDB.sql
rem sqlcmd -S localhost/sqlexpress -E -i ArtAppDB.sql

echo .
echo if no error messages DB was created
pause
