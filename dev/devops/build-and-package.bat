@echo off

:: edit the LJPmath csproj file to set version

echo.
echo ### DELETING RELEASE FOLDERS ###
RMDIR ..\..\src\LJPcalc\bin\Release /S /Q
RMDIR ..\..\src\LJPconsole\bin\Release /S /Q

echo.
echo ### CLEANING SOLUTION ###
dotnet clean ..\..\src\LJP.sln --verbosity quiet --configuration Release

echo.
echo ### REBUILDING SOLUTION ###
dotnet build ..\..\src\LJP.sln --verbosity quiet --configuration Release

echo.
echo ### COPYING FILES ###
copy Download* ..\..\src\LJPcalc\bin\Release
copy LJPcalc* ..\..\src\LJPcalc\bin\Release
copy ..\..\src\IonTable.md ..\..\src\LJPcalc\bin\Release\net472
copy ..\..\src\LJPconsole\LJPconsole.bat ..\..\src\LJPcalc\bin\Release\net472
copy ..\..\src\LJPconsole\bin\Release\net472\LJPconsole* ..\..\src\LJPcalc\bin\Release\net472

rename ..\..\src\LJPcalc\bin\Release\net472 LJPcalc
del ..\..\src\LJPcalc\bin\Release\LJPcalc\*.pdb
del ..\..\src\LJPcalc\bin\Release\LJPcalc\*.config

echo.
echo ### LAUNCHING OUTPUT FOLDER ###
explorer.exe ..\..\src\LJPcalc\bin\Release
pause