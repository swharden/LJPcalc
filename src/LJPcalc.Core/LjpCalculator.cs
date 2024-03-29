﻿using System.Diagnostics;
using LJPcalc.Core.Solver;

namespace LJPcalc.Core;

public class LjpCalculator
{
    public readonly Ion[] Ions;

    public readonly double TemperatureC;
    public int Iterations => Solver.Iterations;

    private readonly Stopwatch Stopwatch = new();

    public TimeSpan Elapsed => Stopwatch.Elapsed;

    private readonly PhiEquationSolver Solver;

    private readonly PhiEquation PhiEquations;

    public PhiEquationSolution BestSolution => Solver.BestSolution;

    public PhiEquationSolution LatestSolution;

    public LjpCalculator(Ion[] ions, double temperature = 25, bool autoSort = true)
    {
        if (ions is null)
            throw new ArgumentNullException(nameof(ions));

        if (ions.Length == 1)
            throw new ArgumentException("LJP calculation requires at least two ions");

        foreach (Ion ion in ions)
        {
            if (ion.Charge == 0)
                throw new ArgumentException($"ion charge cannot not be zero: {ion}");

            if (ion.Conductivity == 0)
                throw new ArgumentException($"ion conductivity cannot not be zero: {ion}");

            if (ion.Mu == 0)
                throw new ArgumentException($"ion mobility cannot not be zero: {ion}");
        }

        if (ions.Select(x => x.C0).Sum() == 0)
            throw new InvalidOperationException("Left side of the junction contains no ions");

        if (ions.Select(x => x.CL).Sum() == 0)
            throw new InvalidOperationException("Right side of the junction contains no ions");


        // clone all ions so we don't accidentally mutate any values passed in
        ions = ions.Select(x => x.Clone()).ToArray();

        if (autoSort)
            ions = IonSorting.PreCalculationSort(ions);

        if (ions[ions.Length - 2].C0 == ions[ions.Length - 2].CL)
            throw new InvalidOperationException("second from last ion concentrations cannot be equal");

        Ions = ions;
        TemperatureC = temperature;
        PhiEquations = new PhiEquation(ions, temperature);

        Solver = new PhiEquationSolver(PhiEquations);
        LatestSolution = Solver.BestSolution;
    }

    public override string ToString()
    {
        return $"After {Iterations} iterations: {BestSolution}";
    }

    public static string GetVersion()
    {
        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }

    /// <summary>
    /// Perform a single iteration
    /// </summary>
    public void Iterate()
    {
        Stopwatch.Start();
        LatestSolution = Solver.Iterate();
        Stopwatch.Stop();
    }

    /// <summary>
    /// Iterate the solver until the error is below the target or the iteration count exceeds the limit
    /// </summary>
    public void IterateRepeatedly(double targetError = 1.0, int maxIterations = 5000)
    {
        while (Solver.BestSolution.MaxAbsoluteError > targetError && Iterations < maxIterations)
        {
            Iterate();
        }
    }

    /// <summary>
    /// Get details about the best LJP solution available
    /// </summary>
    public LjpResult GetBestLjpResult()
    {
        // TODO: consider moving this logic into the iterator???

        return GetLjpResult(Ions, BestSolution.Phis, BestSolution.SolvedCLs, TemperatureC, BestSolution.LjpVolts, Elapsed, Iterations, BestSolution.MaxAbsoluteError);
    }

    /// <summary>
    /// Get details about the most recently solved LJP solution
    /// </summary>
    public LjpResult GetLatestLjpResult()
    {
        // TODO: consider moving this logic into the iterator???

        return GetLjpResult(Ions, LatestSolution.Phis, LatestSolution.SolvedCLs, TemperatureC, LatestSolution.LjpVolts, Elapsed, Iterations, LatestSolution.MaxAbsoluteError);
    }

    /// <summary>
    /// Get details about the best LJP solution available
    /// </summary>
    public static LjpResult GetLjpResult(Ion[] Ions, double[] phis, double[] solvedCLs, double temperatureC, double ljpVolts, TimeSpan elapsed, int iterations, double maxError)
    {
        // TODO: consider moving this logic into the iterator???

        Ion[] ions = Ions.Select(x => x.Clone()).ToArray();
        Ion lastIon = ions[Ions.Length - 1];
        Ion secondFromLastIon = ions[Ions.Length - 2];

        // update ions based on new phis and CLs (all ions except the last two)
        for (int j = 0; j < phis.Length; j++)
        {
            ions[j].Phi = phis[j];
            ions[j].CL = solvedCLs[j];
        }

        // second from last ion phi is its concentration difference
        secondFromLastIon.Phi = secondFromLastIon.CL - secondFromLastIon.C0;

        // last ion's phi is calculated using all the phis before it
        lastIon.Phi = -ions.Take(ions.Length - 1).Sum(x => x.Phi * x.Charge) / lastIon.Charge;

        // last ion's cL is calculated from all the cLs before it
        lastIon.CL = -ions.Take(ions.Length - 1).Sum(x => x.CL * x.Charge) / lastIon.Charge;

        return new LjpResult(Ions, ions, temperatureC, ljpVolts, elapsed, iterations, maxError);
    }

    /// <summary>
    /// Calculate LJP for an ion table using a single function call.
    /// For additional options instantiate a <see cref="LjpCalculator"/> and interact with it directly.
    /// </summary>
    /// <param name="ions">ion table to calculate</param>
    /// <param name="temperatureC">temperature (celsius)</param>
    /// <param name="autoSort">if true, automatically sort the ion table to improve chances a solution will be found quickly</param>
    /// <param name="error">Stop solving once CL contrations are all within this percent error of those in the given ion table.</param>
    /// <param name="maxIterations">Stop solving for CL once this number of iterations has been reached.</param>
    public static LjpResult SolvePhisThenCalculateLjp(Ion[] ions, double temperatureC = 25, bool autoSort = true, double error = 1, int maxIterations = 5000)
    {
        LjpCalculator calc = new(ions, temperatureC, autoSort);
        calc.IterateRepeatedly(error, maxIterations);
        return calc.GetBestLjpResult();
    }

    /// <summary>
    /// Solves for LJP using the given phis to calculate new CLs for each ion.
    /// </summary>
    public static (double volts, double[] CLs) CalculateLjp(Ion[] ions, double[] initialPhis, double temperatureC)
    {
        ions = ions.ToArray();

        int ionCount = ions.Length;
        int ionCountMinusOne = ionCount - 1;
        int ionCountMinusTwo = ionCount - 2;
        int indexLastIon = ionCount - 1;
        int indexSecondFromLastIon = ionCount - 2;

        Ion lastIon = ions[indexLastIon];
        Ion secondFromLastIon = ions[indexSecondFromLastIon];

        if (initialPhis.Length != ionCount - 2)
            throw new ArgumentException($"{nameof(initialPhis)} length must be two less than the length of {nameof(ions)}");

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
            phis[j] = initialPhis[j];

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
        for (double rhoK = KC0; dK > 0 ? rhoK <= KCL : rhoK >= KCL; rhoK += dK)
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
}
