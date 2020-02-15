using System;
using System.Collections.Generic;
using System.Text;

namespace LJPmath
{
    public static class Constants
    {
		// TODO: swap these to use full precision

		public const double zeroCinK = 273.15;
		public const double temperatureC = 25.0;

		// Boltzmann constant = 1.38064852e-23 (m^2 * kg) / (s^2 * K)
		public const double boltzmann = 1.38064852e-23;
		public const double KT = boltzmann * (temperatureC + zeroCinK);

		// Elementary charge = 1.602176634e-19 (cm * g * s)
		public const double e = 1.602176634e-19;

		// Avogadro constant = 6.02214076e23 (no units)
		public const double Nav = 6.02214076e23;

		// Vacuum permittivity = 8.8541878128e-12 (F / m)
		//public const double epsilon = 8.8541878128e-12;
	}
}
