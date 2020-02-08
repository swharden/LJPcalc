# LJPcalc

[![Build Status](https://dev.azure.com/swharden/swharden/_apis/build/status/swharden.LJPcalc?branchName=master)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=7&branchName=master)

LJPcalc is a free and open source liquid junction potential (LJP) calculator.

**WARNING: LJPcalc is not ready for use:** This software is early in development and users interested in calculating LJPs are encouraged to use [JLJP](https://github.com/swharden/JLJP) while this project is being developed.

### Features
* Available for a variety of platforms
  * **LJPcalc** is a graphical click-to-run Windows application (.exe)
  * **LJPconsole** is a cross-platform LJP calculator
  * **LJPapp** is an Android application to compliment [NernstApp](https://github.com/swharden/NernstApp)
* LJPcalc calculates LJP according to the stationary Nernst-Planck equation which is regarded as superior to the Henderson equation (used by most commercial LJP calculators) as described in [Marino et al., 2014](https://arxiv.org/abs/1403.3640).
* Diffusion coefficients can be calculated dynamically from a concentration-dependent formula provided by the user.

### Developer Notes

Project | Platform | Purpose
---|---|---
**`LJPmath`** | [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) | Platform-independent library to calculate LJP
**`LJPconsole`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Cross-platform console-based LJP calculator
**`LJPcalc`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Graphical LJP calculator for Windows (WPF)
**`LJPapp`** | [Xamarin.Forms](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms) | Mobile app for iPhone and Android
**`LJPtest`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Tests for the LJPmath module

### Authors
LJPcalc was created by [Scott W Harden](http://swharden.com/) in 2020. LJPcalc began as C# port of [JLJP](https://github.com/swharden/JLJP) by [Doriano Brogioli](https://sites.google.com/site/dbrogioli/) originally published on SourceForge in 2013. LJPcalc is heavily influenced by [Marino et al., 2014](https://arxiv.org/abs/1403.3640).
