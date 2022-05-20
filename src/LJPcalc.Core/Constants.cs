namespace LJPcalc.Core;

public static class Constants
{
    public const double ZeroCelsiusInKelvin = 273.15;

    // Boltzmann constant = 1.38064852e-23 (m^2 * kg) / (s^2 * K)
    public const double Boltzmann = 1.38064852e-23;

    // Elementary charge = 1.602176634e-19 (cm * g * s)
    public const double e = 1.602176634e-19;

    // Avogadro constant = 6.02214076e23 (no units)
    public const double Nav = 6.02214076e23;

    // Gas Constant
    public const double R = Nav * Boltzmann;

    // Faraday constant (C / mol)
    public const double F = e * Nav;
}