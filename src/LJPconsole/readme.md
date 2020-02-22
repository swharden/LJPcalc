# LJP Console

**LJP console** is a cross-platform console version of the [LJPcalc](https://github.com/swharden/LJPcalc) application for calculating liquid junction potential from a set of ions of given concentration on each side of the junction. 

![](screenshot.png)

## Build and Run
_These instructions work identically on Windows, Linux, and MacOS_
* Install [.NET Core SDK](https://dotnet.microsoft.com/download)
* `cd src/LJPconsole`
* `dotnet run`

## Quickstart

**Add ions with the `add` command** followed by the ion name and concentration on each side of the junction. Ion valence, conductivity, and mu are automatically sourced from [IonTable.csv](/src/IonTable.csv) but can be defined manually if desired (use `help` command for details).

```
>> add zn 9 .0284
Adding Ion Zn (+2): mu=1.708E+11, c0=9.000, cL=0.028

>> add k 0 3
Adding Ion K (+1): mu=4.755E+11, c0=0.000, cL=3.000

>> add cl 18 3.0568
Adding Ion Cl (-1): mu=4.936E+11, c0=18.000, cL=3.057
```

**Use the `ljp` command to calculate liquid junction potential.** This command shows the ion set (and phi values) used for calculations. LJP is displayed in millivolts.

```
>> ljp
calculating...
Values for cL were adjusted to achieve electro-neutrality:

 Name               | Charge | Conductivity (E-4) | C0 (mM)      | CL (mM)
--------------------|--------|--------------------|--------------|--------------
 Zn                 | +2     | 52.8               | 9            | 0.028404
 K                  | +1     | 73.5               | 0            | 3
 Cl                 | -1     | 76.31              | 18           | 3.0568079

Equations were solved in 32.91 ms
LJP = -20.7955864304731 mV
```