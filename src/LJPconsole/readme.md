# LJP Console

**LJP console** is a cross-platform console version of the [LJPcalc](https://github.com/swharden/LJPcalc) application for calculating liquid junction potential from a set of ions of given concentration on each side of the junction. 

![](screenshot.jpg)

## Build and Run
_These instructions work identically on Windows, Linux, and MacOS_
* Install [.NET Core SDK](https://dotnet.microsoft.com/download)
* `cd src/LJPconsole`
* `dotnet run`

## Quickstart

**Add ions with the `add` command** followed by the ion name and concentration on each side of the junction. Ion valence, conductivity, and mu are automatically sourced from [IonTable.csv](/src/IonTable.csv) but can be defined manually if desired (use `help` command for details).

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