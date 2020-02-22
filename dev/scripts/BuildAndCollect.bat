del /s /q .\Release\*.*

dotnet clean ..\..\src\LJP.sln
dotnet clean ..\..\src\LJP.sln --configuration Release
dotnet build ..\..\src\LJP.sln --configuration Release

pause

:: copy LJPconsole
copy ..\..\src\LJPconsole\bin\Release\netcoreapp2.0\LJPconsole*.* .\Release

:: copy LJPcalc
copy ..\..\src\LJPcalc\bin\Release\netcoreapp3.0\*.* .\Release

:: copy ion table
copy ..\..\src\IonTable.md .\Release

:: copy Bundle contents
copy .\Bundle\*.* .\Release

pause