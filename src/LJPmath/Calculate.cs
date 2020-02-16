using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public class Calculate
    {
        public static double Ljp(List<Ion> ionSet)
        {

            foreach (Ion ion in ionSet)
            {
                if (ion.charge == 0)
                    throw new ArgumentException("ion charge cannot be zero");
                if (ion.mu == 0)
                    throw new ArgumentException("ion mu cannot be zero");
            }

            int ionCount = ionSet.Count;
            int ionCountMinusOne = ionCount - 1;
            int ionCountMinusTwo = ionCount - 2;

            Ion secondFromLastIon = ionSet[ionSet.Count - 2];
            Ion LastIon = ionSet[ionSet.Count - 1];

            if (secondFromLastIon.c0 == secondFromLastIon.cL)
                throw new InvalidOperationException("second from last ion concentrations cannot be equal");

            // phis will be solved for all ions except the last two
            double[] phis = new double[ionCountMinusTwo];

            // phis to solve are initialized to the concentration difference
            for (int j = 0; j < phis.Length; j++)
            {
                Ion ion = ionSet[j];
                phis[j] = ion.cL - ion.c0;
            }

            // all phis except the last two get solved
            if (ionCount > 2)
            {
                var phiEquations = new PhiEquations(ionSet) as IEquationSystem;
                Solver s = new Solver(phiEquations);
                s.solve(phis);
            }

            // calculate LJP
            double[] cLs = new double[phis.Length];
            double ljp_V = Ljp(ionSet, phis, cLs);
            if (ljp_V == Double.NaN)
                throw new Exception("ERROR: Singularity (calculation aborted)");

            // update ions based on what was just calculated
            for (int j = 0; j < phis.Length; j++)
            {
                Ion ion = ionSet[j];
                ion.phi = phis[j];
                ion.cL = cLs[j];
            }

            // second from last ion phi is concentration difference
            secondFromLastIon.phi = secondFromLastIon.cL - secondFromLastIon.c0;

            // last ion's phi is calculated from all the phis before it
            double totalChargeWeightedPhi = 0.0;
            for (int j = 0; j < ionCountMinusOne; j++)
            {
                Ion ion = ionSet[j];
                totalChargeWeightedPhi += ion.phi * ion.charge;
            }
            LastIon.phi = -totalChargeWeightedPhi / LastIon.charge;

            // last ion's cL is calculated from all the cLs before it
            double totalChargeWeightedCL = 0.0;
            for (int j = 0; j < ionCountMinusOne; j++)
            {
                Ion ion = ionSet[j];
                totalChargeWeightedCL += ion.cL * ion.charge;
            }
            LastIon.cL = -totalChargeWeightedCL / LastIon.charge;

            return ljp_V;
        }

        public static double Ljp(List<Ion> list, double[] startingPhis, double[] startingCLs)
        {

            double cdadc = 1.0; // fine for low concentrations

            int ionCount = list.Count;
            int ionCountMinusOne = ionCount - 1;
            int ionCountMinusTwo = ionCount - 2;
            int indexLastIon = ionCount - 1;
            int indexSecondFromLastIon = ionCount - 2;

            Ion lastIon = list[indexLastIon];
            Ion secondFromLastIon = list[indexSecondFromLastIon];

            if (startingPhis.Length != ionCount - 2)
                throw new ArgumentException();

            if (startingCLs.Length != ionCount - 2)
                throw new ArgumentException();

            // populate charges, mus, and rhos from all ions except the last one
            double[] charges = new double[ionCountMinusOne];
            double[] mus = new double[ionCountMinusOne];
            double[] rhos = new double[ionCountMinusOne];
            for (int j = 0; j < ionCountMinusOne; j++)
            {
                Ion ion = list[j];
                charges[j] = ion.charge;
                mus[j] = ion.mu;
                rhos[j] = ion.c0;
            }

            // populate phis from all ions except the last two
            double[] phis = new double[ionCountMinusOne];
            for (int j = 0; j < ionCountMinusTwo; j++)
                phis[j] = startingPhis[j];

            // second from last phi is the concentration difference
            phis[indexSecondFromLastIon] = secondFromLastIon.cL - secondFromLastIon.c0;

            // prepare info about second from last ion concentration difference for loop
            if (secondFromLastIon.c0 == secondFromLastIon.cL)
                throw new InvalidOperationException("second from last ion concentrations cannot be equal");

            double KC0 = secondFromLastIon.c0;
            double KCL = secondFromLastIon.cL;
            double dK = (KCL - KC0) / 1000.0;

            // set last ion C0 based on charges, rhos, and linear algebra
            double zCl = list[indexLastIon].charge;
            double rhoCl = -Linalg.ScalarProduct(charges, rhos) / zCl;
            lastIon.c0 = rhoCl;

            // cycle to determine junction voltage

            double V = 0.0;
            for (double rhoK = KC0; ((dK > 0) ? rhoK <= KCL : rhoK >= KCL); rhoK += dK)
            {

                rhoCl = -Linalg.ScalarProduct(rhos, charges) / zCl;

                double DCl = lastIon.mu * Constants.KT * cdadc;
                double vCl = zCl * Constants.e * lastIon.mu * rhoCl;

                double[] v = new double[ionCountMinusOne];
                double[,] mD = new double[ionCountMinusOne, ionCountMinusOne];

                for (int j = 0; j < ionCountMinusOne; j++)
                    for (int k = 0; k < ionCountMinusOne; k++)
                        mD[j, k] = 0.0;

                for (int j = 0; j < ionCountMinusOne; j++)
                {
                    for (int k = 0; k < ionCountMinusOne; k++)
                        mD[j, k] = 0.0;
                    mD[j, j] = mus[j] * Constants.KT * cdadc;
                    v[j] = charges[j] * Constants.e * mus[j] * rhos[j];
                }

                if (Linalg.ScalarProduct(charges, v) + zCl * vCl == 0.0)
                {
                    return Double.NaN; // Singularity; abort calculation
                }

                double[,] identity = Linalg.Identity(ionCountMinusOne);
                double[,] linAlgSum = Linalg.Sum(1, mD, -DCl, identity);
                double[] sumCharge = Linalg.Product(linAlgSum, charges);
                double chargesProd = Linalg.ScalarProduct(charges, v);
                double chargesProdPlusCl = chargesProd + zCl * vCl;
                double[] delta = Linalg.ScalarMultiply(sumCharge, 1.0 / chargesProdPlusCl);

                double[,] mDyadic = new double[ionCountMinusOne, ionCountMinusOne];
                for (int j = 0; j < ionCountMinusOne; j++)
                    for (int k = 0; k < ionCountMinusOne; k++)
                        mDyadic[j, k] = v[j] * delta[k];
                double[,] mM = Linalg.Sum(1, mDyadic, -1, mD);

                double[] rhop = Linalg.Solve(mM, phis);
                double rhopK = rhop[indexSecondFromLastIon];
                rhos = Linalg.Sum(1, rhos, dK / rhopK, rhop);

                double E = Linalg.ScalarProduct(delta, rhop);
                V -= E * dK / rhopK;

            }

            // modify CLs based on the rhos we calculated
            for (int j = 0; j < ionCountMinusTwo; j++)
                startingCLs[j] = rhos[j];

            return V;
        }
    }
}
