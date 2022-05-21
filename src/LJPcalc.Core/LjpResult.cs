namespace LJPcalc.Core;

public class LjpResult
{
    public readonly Ion[] Ions;
    public readonly double LjpMillivolts;
    public readonly double TemperatureC;
    public readonly double PhiSolutionM;
    public readonly TimeSpan TimePhi;
    public readonly TimeSpan TimeLjp;

    /// <summary>
    /// Indicates how well the solver found a solution (typically 1.0 indicates a solution was found)
    /// </summary>
    public double FMax => Ions.Select(x => Math.Abs(x.Phi)).Max();

    public LjpResult(Ion[] ions, double tempC, double ljpMv, TimeSpan timePhi, TimeSpan timeLjp)
    {
        Ions = ions;
        LjpMillivolts = ljpMv;
        TemperatureC = tempC;
        TimePhi = timePhi;
        TimeLjp = timeLjp;
    }

    public string GetSummary()
    {
        System.Text.StringBuilder sb = new();
        foreach (Ion ion in Ions)
        {
            if (double.IsFinite(ion.PercentChangeC0) && ion.PercentChangeC0 > 2)
                sb.AppendLine($"C0 change: {ion.Name} went from {ion.InitialC0:N2} to {ion.C0:N2} ({ion.PercentChangeC0:N2}%)");

            if (double.IsFinite(ion.PercentChangeCL) && ion.PercentChangeCL > 2)
                sb.AppendLine($"CL change: {ion.Name} went from {ion.InitialCL:N2} to {ion.CL:N2} ({ion.PercentChangeCL:N2}%)");
        }
        sb.AppendLine($"Time to balance phis: {TimePhi}");
        sb.AppendLine($"Time to solve for LJP: {TimeLjp}");
        return sb.ToString();
    }
}