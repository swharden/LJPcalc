# LJPcalc

[![Build Status](https://dev.azure.com/swharden/swharden/_apis/build/status/swharden.LJPcalc?branchName=master)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=7&branchName=master)

LJPcalc is a free and open source liquid junction potential (LJP) calculator.

> ⚠️ **This application is a work in progress.** Users are encouraged to use the LJP console application while the LJPcalc GUI is being developed.

![](src/LJPcalc/screenshot.jpg)

## LJP Console

**LJP console** is a cross-platform console application for calculating liquid junction potential from a set of ions of given concentration on each side of the junction.

![](src/LJPconsole/screenshot.jpg)

### Build and Run (Windows, Linux, and MacOS)
* Install [.NET Core SDK](https://dotnet.microsoft.com/download)
* `cd src/LJPconsole`
* `dotnet run`

### Quickstart

**Add ions with the `add` command** followed by the ion name and concentration on each side of the junction. Ion valence, conductivity, and mu are automatically sourced from [IonTable.csv](IonTable.csv) but can be defined manually if desired (use `help` command for details).

```
>> add zn 9 .0284
Adding Ion Zn (+2): mu=1.713E+11, phi=0, c0=9, cL=0.0284

>> add k 0 3
Adding Ion K (+1): mu=4.769E+11, phi=0, c0=0, cL=3

>> add cl 18 3.0568
Adding Ion Cl (-1): mu=4.952E+11, phi=0, c0=18, cL=3.0568
```

**Use the `ljp` command to calculate liquid junction potential.** This command shows the ion set (and phi values) used for calculations. LJP is displayed in millivolts.

```
>> ljp
calculating...

ION TABLE:
Ion       Charge Cond (E4) Mu (E11)  Phi       c0 (mM)   cL (mM)
Zn        2      52.8      1.71304   -8.99789  0.0284    9
K         1      73.5      4.76926   3         3         0
Cl        -1     76.31     4.95159   -14.99577 3.05681   18

LJP = -20.8238808914194 mV
```

NOTE: The concentration of the last ion is overridden by that required to achieve electro-neutrality of the ion set. The second-to-last ion is (???). See [Marino et al., 2014](https://arxiv.org/abs/1403.3640) for details.

## LJP Theory 

LJPcalc calculates LJP according to the stationary Nernst-Planck equation which is regarded as superior to the Henderson equation (used by most commercial LJP calculators) as described in [Marino et al., 2014](https://arxiv.org/abs/1403.3640).

## Source Code Project Structure

Project | Platform | Purpose
---|---|---
**`LJPmath`** | [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) | Platform-independent library to calculate LJP
**`LJPconsole`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Cross-platform console-based LJP calculator
**`LJPcalc`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Graphical LJP calculator for Windows (WPF)
**`LJPapp`** | [Xamarin.Forms](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms) | Mobile app for iPhone and Android
**`LJPtest`** | [.NET Core](https://en.wikipedia.org/wiki/.NET_Core) | Tests for the LJPmath module

## Citing LJPcalc

If LJPcalc facilitated your research, consider citing this project by name so it can benefit others too:

> "Liquid junction potential was calculated according to the stationary Nernst-Planck equation using LJPcalc version 1.2 by Scott Harden and Doriano Brogioli."

## Authors
LJPcalc was created by [Scott W Harden](http://swharden.com/) in 2020. LJPcalc began as C# port of [JLJP](https://github.com/swharden/JLJP) by [Doriano Brogioli](https://sites.google.com/site/dbrogioli/) originally published on SourceForge in 2013. LJPcalc is heavily influenced by [Marino et al., 2014](https://arxiv.org/abs/1403.3640).
