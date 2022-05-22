using LJPcalc.Core.Solver;

namespace LJPcalc.Core;

public class Calculate
{
    public static LjpResult Ljp(Ion[] ions2, double temperatureC = 25)
    {
        LjpCalculationOptions defaultOptions = new() { TemperatureC = temperatureC };
        return Ljp(ions2, defaultOptions);
    }

    public static LjpResult Ljp(Ion[] ions2, LjpCalculationOptions options)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        Ion[] ionsInput = ions2.Select(x => x.Clone()).ToArray();
        Ion[] ions = ions2.Select(x => x.Clone()).ToArray();

        if (ions.Any(x => x.Charge == 0))
            throw new ArgumentException("ion charge cannot be zero");

        if (ions.Any(x => x.Mu == 0))
            throw new ArgumentException("ion mu cannot be zero");

        if (options.AutoSort)
            ions = PreCalculationIonListSort(ions);

        Ion secondFromLastIon = ions[ions.Length - 2];
        Ion LastIon = ions[ions.Length - 1];

        if (secondFromLastIon.C0 == secondFromLastIon.CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        // determine flux for all ions
        double[] phis = ions.Take(ions.Length - 2).Select(x => x.CL - x.C0).ToArray();
        int iterations = 0;

        // use the fancy solver to optimize phis when there are more than 2 ions
        if (ions.Length > 2)
        {
            IEquationSystem equations = new PhiEquationSystem(ions, options.TemperatureC);

            EquationSolver solver = new(equations, phis)
            {
                MaximumIterations = options.MaximumIterations,
                ThrowIfIterationLimitExceeded = options.ThrowIfIterationLimitExceeded
            };
            solver.IterationFinished += (object? sender, EventArgs args) => options.OnIterationFinished(sender, args);

            phis = solver.Solve();

            iterations = solver.Iterations;
        }

        // calculate LJP (modifies one of the phis and all the CLs)
        (double ljp_V, double[] solveCLs) = SolveForLJP(ions, phis, options.TemperatureC);

        // update ions based on new phis and CLs (all ions except the last two)
        for (int j = 0; j < phis.Length; j++)
        {
            ions[j].Phi = phis[j];
            ions[j].CL = solveCLs[j];
        }

        // second from last ion phi is concentration difference
        secondFromLastIon.Phi = secondFromLastIon.CL - secondFromLastIon.C0;

        // last ion's phi is calculated from all the phis before it
        LastIon.Phi = -ions.Take(ions.Length - 1).Sum(x => x.Phi * x.Charge) / LastIon.Charge;

        // last ion's cL is calculated from all the cLs before it
        LastIon.CL = -ions.Take(ions.Length - 1).Sum(x => x.CL * x.Charge) / LastIon.Charge;

        // load new details into the result
        LjpResult result = new(ionsInput, options.TemperatureC, ljp_V * 1000, sw.Elapsed, iterations);

        return result;
    }

    /// <summary>
    /// WARNING: this method modifies input arrays (the last ion's C0 and all the CLs).
    /// It is only to be called by the solver.
    /// </summary>
    public static (double volts, double[] cls) SolveForLJP(Ion[] ions, double[] startingPhis, double temperatureC)
    {
        int ionCount = ions.Length;
        int ionCountMinusOne = ionCount - 1;
        int ionCountMinusTwo = ionCount - 2;
        int indexLastIon = ionCount - 1;
        int indexSecondFromLastIon = ionCount - 2;

        Ion lastIon = ions[indexLastIon];
        Ion secondFromLastIon = ions[indexSecondFromLastIon];

        if (startingPhis.Length != ionCount - 2)
            throw new ArgumentException($"{nameof(startingPhis)} length must be two less than the length of {nameof(ions)}");

        // populate charges, mus, and rhos from all ions except the last one
        double[] charges = new double[ionCountMinusOne];
        double[] mus = new double[ionCountMinusOne];
        double[] rhos = new double[ionCountMinusOne];
        for (int j = 0; j < ionCountMinusOne; j++)
        {
            charges[j] = ions[j].Charge;
            mus[j] = ions[j].Mu;
            rhos[j] = ions[j].C0;
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
        double zCl = ions[indexLastIon].Charge;
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
                throw new InvalidOperationException("singularity detected");

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

        // CLs were adjusted during the calculation
        double[] newCLs = ions.Select(x => x.CL).ToArray();
        for (int j = 0; j < ionCountMinusTwo; j++)
            newCLs[j] = rhos[j];

        return (V, newCLs);
    }

    /// <summary>
    /// This strategy places the ion with the largest difference between C0 and CL last
    /// and the remaining ion with the largest CL second to last.
    /// </summary>
    private static Ion[] PreCalculationIonListSort(Ion[] inputIons)
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
