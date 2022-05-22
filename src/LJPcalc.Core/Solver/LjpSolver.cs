namespace LJPcalc.Core.Solver;

public static class LjpSolver
{
    /// <summary>
    /// Solves for LJP using the given phis to calculate new CLs for each ion.
    /// </summary>
    public static LjpSolution CalculateLjp(Ion[] ions, double[] initialPhis, double temperatureC)
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

        return new LjpSolution(V, newCLs);
    }
}
