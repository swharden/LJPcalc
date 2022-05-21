using LJPcalc.Core.Solver;

namespace LJPcalc.Core;

public static class Calculate
{
    public static LjpResult Ljp(Ion[] ions, double temperatureC = 25, bool autoSort = true, double timeoutMilliseconds = 5000, bool throwIfTimeout = false)
    {
        if (ions.Any(x => x.Charge == 0))
            throw new ArgumentException("ion charge cannot be zero");

        if (ions.Any(x => x.Mu == 0))
            throw new ArgumentException("ion mu cannot be zero");

        if (autoSort)
            ions = PreCalculationIonListSort(ions);

        Ion secondFromLastIon = ions[ions.Length - 2];
        Ion LastIon = ions[ions.Length - 1];

        if (secondFromLastIon.C0 == secondFromLastIon.CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        // create this now to preserve original stats about each ion
        Ion[] ionsInput = ions.ToArray();

        // solve for phis (if the number of ions is greater than 2)
        System.Diagnostics.Stopwatch sw = new();
        sw.Restart();
        var phiSolution = new PhiSolver(ions, temperatureC, timeoutMilliseconds, throwIfTimeout);
        double[] phis = phiSolution.SolvedPhis;
        TimeSpan timePhi = sw.Elapsed;

        // calculate LJP (modifies one of the phis and all the CLs)
        double[] cLs = new double[phis.Length];
        sw.Restart();
        double ljp_V = SolveForLJP(ions, phis, cLs, temperatureC);
        TimeSpan timeLjp = sw.Elapsed;

        // update ions based on new phis and CLs (all ions except the last two)
        for (int j = 0; j < phis.Length; j++)
        {
            ions[j].Phi = phis[j];
            ions[j].CL = cLs[j];
        }

        // second from last ion phi is concentration difference
        secondFromLastIon.Phi = secondFromLastIon.CL - secondFromLastIon.C0;

        // last ion's phi is calculated from all the phis before it
        LastIon.Phi = -ions.Take(ions.Length - 1).Sum(x => x.Phi * x.Charge) / LastIon.Charge;

        // last ion's cL is calculated from all the cLs before it
        LastIon.CL = -ions.Take(ions.Length - 1).Sum(x => x.CL * x.Charge) / LastIon.Charge;

        // load new details into the result
        LjpResult result = new(ionsInput, temperatureC, ljp_V * 1000, phiSolution.SolutionM, timePhi, timeLjp);

        return result;
    }

    /// <summary>
    /// WARNING: this method modifies input arrays (the last ion's C0 and all the CLs).
    /// It is only to be called by the solver.
    /// </summary>
    public static double SolveForLJP(Ion[] ionList, double[] startingPhis, double[] CLs, double temperatureC)
    {
        int ionCount = ionList.Length;
        int ionCountMinusOne = ionCount - 1;
        int ionCountMinusTwo = ionCount - 2;
        int indexLastIon = ionCount - 1;
        int indexSecondFromLastIon = ionCount - 2;

        Ion lastIon = ionList[indexLastIon];
        Ion secondFromLastIon = ionList[indexSecondFromLastIon];

        if (startingPhis.Length != ionCount - 2)
            throw new ArgumentException();

        if (CLs.Length != ionCount - 2)
            throw new ArgumentException();

        // populate charges, mus, and rhos from all ions except the last one
        double[] charges = new double[ionCountMinusOne];
        double[] mus = new double[ionCountMinusOne];
        double[] rhos = new double[ionCountMinusOne];
        for (int j = 0; j < ionCountMinusOne; j++)
        {
            Ion ion = ionList[j];
            charges[j] = ion.Charge;
            mus[j] = ion.Mu;
            rhos[j] = ion.C0;
        }

        // populate phis from all ions except the last two
        double[] phis = new double[ionCountMinusOne];
        for (int j = 0; j < ionCountMinusTwo; j++)
            phis[j] = startingPhis[j];

        // second from last phi is the concentration difference
        phis[indexSecondFromLastIon] = secondFromLastIon.CL - secondFromLastIon.C0;

        // prepare info about second from last ion concentration difference for loop
        if (secondFromLastIon.C0 == secondFromLastIon.CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        double KC0 = secondFromLastIon.C0;
        double KCL = secondFromLastIon.CL;
        double dK = (KCL - KC0) / 1000.0;

        // set last ion C0 based on charges, rhos, and linear algebra
        double zCl = ionList[indexLastIon].Charge;
        double rhoCl = -LinearAlgebra.ScalarProduct(charges, rhos) / zCl;
        lastIon.C0 = rhoCl;

        // cycle to determine junction voltage
        double V = 0.0;
        double KT = Constants.Boltzmann * (temperatureC + Constants.ZeroCelsiusInKelvin);
        double cdadc = 1.0; // fine for low concentrations
        for (double rhoK = KC0; ((dK > 0) ? rhoK <= KCL : rhoK >= KCL); rhoK += dK)
        {
            rhoCl = -LinearAlgebra.ScalarProduct(rhos, charges) / zCl;

            double DCl = lastIon.Mu * KT * cdadc;
            double vCl = zCl * Constants.e * lastIon.Mu * rhoCl;

            double[] v = new double[ionCountMinusOne];
            double[,] mD = new double[ionCountMinusOne, ionCountMinusOne];

            for (int j = 0; j < ionCountMinusOne; j++)
                for (int k = 0; k < ionCountMinusOne; k++)
                    mD[j, k] = 0.0;

            for (int j = 0; j < ionCountMinusOne; j++)
            {
                for (int k = 0; k < ionCountMinusOne; k++)
                    mD[j, k] = 0.0;
                mD[j, j] = mus[j] * KT * cdadc;
                v[j] = charges[j] * Constants.e * mus[j] * rhos[j];
            }

            if (LinearAlgebra.ScalarProduct(charges, v) + zCl * vCl == 0.0)
            {
                return double.NaN; // Singularity; abort calculation
            }

            double[,] identity = LinearAlgebra.Identity(ionCountMinusOne);
            double[,] linAlgSum = LinearAlgebra.Sum(1, mD, -DCl, identity);
            double[] sumCharge = LinearAlgebra.Product(linAlgSum, charges);
            double chargesProd = LinearAlgebra.ScalarProduct(charges, v);
            double chargesProdPlusCl = chargesProd + zCl * vCl;
            double[] delta = LinearAlgebra.ScalarMultiply(sumCharge, 1.0 / chargesProdPlusCl);

            double[,] mDyadic = new double[ionCountMinusOne, ionCountMinusOne];
            for (int j = 0; j < ionCountMinusOne; j++)
                for (int k = 0; k < ionCountMinusOne; k++)
                    mDyadic[j, k] = v[j] * delta[k];
            double[,] mM = LinearAlgebra.Sum(1, mDyadic, -1, mD);

            double[] rhop = LinearAlgebra.Solve(mM, phis);
            double rhopK = rhop[indexSecondFromLastIon];
            rhos = LinearAlgebra.Sum(1, rhos, dK / rhopK, rhop);

            double E = LinearAlgebra.ScalarProduct(delta, rhop);
            V -= E * dK / rhopK;

        }

        // modify the input CLs based on the rhos we calculated
        for (int j = 0; j < ionCountMinusTwo; j++)
            CLs[j] = rhos[j];

        return V;
    }

    /// <summary>
    /// This strategy places the ion with the largest difference between C0 and CL last
    /// and the remaining ion with the largest CL second to last.
    /// </summary>
    public static Ion[] PreCalculationIonListSort(Ion[] inputIons)
    {
        List<Ion> ionList = inputIons.ToList();

        // absolute largest cL should be last
        Ion ionWithLargestCL = ionList.OrderBy(x => x.CL).Last();
        ionList.Remove(ionWithLargestCL);

        // largest diff should be second from last
        Ion ionWithLargestCDiff = ionList.OrderBy(x => Math.Abs(x.CL - x.C0)).Last();
        ionList.Remove(ionWithLargestCDiff);

        // place the removed ions back in the proper order
        ionList.Add(ionWithLargestCDiff);
        ionList.Add(ionWithLargestCL);
        return ionList.ToArray();
    }
}
