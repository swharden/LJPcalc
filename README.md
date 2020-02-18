# LJPcalc

[![Build Status](https://dev.azure.com/swharden/swharden/_apis/build/status/swharden.LJPcalc?branchName=master)](https://dev.azure.com/swharden/swharden/_build/latest?definitionId=7&branchName=master)

LJPcalc is a free and open source liquid junction potential (LJP) calculator. LJPcalc is available as a click-to-run EXE for Windows and a console application for Linux and MacOS.

![](src/LJPcalc/screenshot.jpg)

### Download (Windows 64-bit): ~~[LJPcalc.zip]()~~

> ⚠️ **This project is still early in development.** A download link for Windows will be added when development completes. Until then users must download and build LJPcalc from source code. _-Scott Harden, February 2020_

## LJPcalc Console Application

A cross-platform console version of LJPcalc is available which supports Windows, Linux, and MacOS.

* [building from source](src/LJPconsole#build-and-run)
* [LJP console quickstart guide](src/LJPconsole#quickstart)

![](src/LJPconsole/screenshot.jpg)

## LJPcalc Mobile

> ⚠️ The LJPcalc mobile app is still under development.

[**NernstApp**](https://github.com/swharden/NernstApp) is a Nernst potential calculator for Android

## Theory

### Calculation Method

LJPcalc calculates the liquid junction potential according to the stationary Nernst-Planck equation which is typically regarded as superior to the simpler Henderson equation used by most commercial LJP calculators. Both equations produce nearly identical LJPs, but the Henderson equation becomes inaccurate as ion concentrations increase, and also when calculating LJP for solutions containing polyvalent ions.

### Ion Sequence

When calculating LJP for a set of ions it is important to note the following facts. Additional information can be found in [Marino et al., 2014](https://arxiv.org/abs/1403.3640) which describes the exact computational methods employed by LJPcalc.

* **The last ion's concentrations will be recalculated.** LJPcalc will override the concentrations the user provides for the last ion in the set, replacing them with those calculated to achieve electro-neutrality of the ion set.

* **The second-to-last ion's concentration cannot be equal on both sides.** This requirement ensures LJPcalc can properly solve for the final ion.


### References
* **[Marino et al. (2014)](https://arxiv.org/abs/1403.3640)** - describes a computational method to calculate LJP according to the stationary Nernst-Planck equation. The JAVA software described in this manuscript is open-source and now on GitHub ([JLJP](https://github.com/swharden/jljp)). Figure 1 directly compares LJP calculated by the Nernst-Planck vs. Henderson equation.
* **[Perram and Stiles (2006)](https://pubs.rsc.org/en/content/articlelanding/2006/cp/b601668e)** - A review of several methods used to calculate liquid junction potential. This manuscript provides excellent context for the history of LJP calculations and describes the advantages and limitations of each.
* **[Shinagawa (1980)](https://www.ncbi.nlm.nih.gov/pubmed/7401663)** _"Invalidity of the Henderson diffusion equation shown by the exact solution of the Nernst-Planck equations"_ - a manuscript which argues that the Henderson equation is inferior to solved Nernst-Planck-Poisson equations due to how it accounts for ion flux in the charged diffusion zone.
* **[Lin (2011)](http://www.sci.osaka-cu.ac.jp/~ohnita/2010/TCLin.pdf)** _"The Poisson The Poisson-Nernst-Planck (PNP) system for ion transport (PNP) system for ion transport"_ - a PowerPoint presentation which reviews mathematical methods to calculate LJP with notes related to its application in measuring voltage across cell membranes.
* **[Nernst-Planck equation](https://en.wikipedia.org/wiki/Nernst%E2%80%93Planck_equation)** (Wikipedia)
* **[Goldman Equation](https://en.wikipedia.org/wiki/Goldman_equation)** (Wikipedia)

## Citing LJPcalc

If LJPcalc facilitated your research, consider citing this project by name so it can benefit others too:

> "Liquid junction potential was calculated according to the stationary Nernst-Planck equation using LJPcalc¹"
>
> [1] Harden, SW and Brogioli, D (2020). LJPcalc v1.0. [Online]. Available: https://github.com/swharden/LJPcalc, Accessed on: Feb. 16, 2020.

## Authors
LJPcalc was created by [Scott W Harden](http://swharden.com/) in 2020. LJPcalc began as C# port of [JLJP](https://github.com/swharden/JLJP) by [Doriano Brogioli](https://sites.google.com/site/dbrogioli/) originally published on SourceForge in 2013. LJPcalc is heavily influenced by [Marino et al., 2014](https://arxiv.org/abs/1403.3640).
