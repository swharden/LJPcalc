# LJPcalc Developer Notes

### Building in Windows
* Install [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) (free)
* Open [/src/LJP.sln](/src/LJP.sln) in Visual Studio
* Right-click the LJPcalc project and select "Set as StartUp Project"
* Press F5 to build and run

### Running in Windows
* LJPcalc.exe requires the .NET Core 3.0 runtime (or SDK): https://dotnet.microsoft.com/download
* Use `dotnet --info` to see what is installed

### Source Code Project Structure

Project | Platform | Purpose
---|---|---
LJPmath | [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) | Platform-independent library to calculate LJP
LJPconsole | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Cross-platform console-based LJP calculator
LJPcalc | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Graphical LJP calculator for Windows (WPF)
LJPapp | [Xamarin.Forms](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms) | Mobile app for iPhone and Android
LJPresearch | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Tool for writing experimental LJP calculations
LJPtest | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Tests for the LJPmath module