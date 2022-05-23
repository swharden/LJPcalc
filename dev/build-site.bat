rmdir /s /q ..\src\LJPcalc.Web\bin\
dotnet publish --configuration Release ..\src\LJPcalc.Web\
explorer ..\src\LJPcalc.Web\bin\Release\net6.0\publish\wwwroot
pause