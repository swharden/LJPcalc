namespace LJPcalc.Core;

public class LjpResult
{
    public readonly Ion[] Ions;
    public readonly double LjpMillivolts;
    public readonly double TemperatureC;
    public readonly double PhiSolutionM;
    public readonly TimeSpan Elapsed;
    public readonly int Iterations;
    public readonly double Error;

    /// <summary>
    /// Indicates how well the solver found a solution (typically 1.0 indicates a solution was found)
    /// </summary>
    public double MaxPhi => Ions.Select(x => Math.Abs(x.Phi)).Max();

    public LjpResult(Ion[] ions, double tempC, double ljpMv, TimeSpan timePhi, int iterations, double error)
    {
        Ions = ions;
        LjpMillivolts = ljpMv;
        TemperatureC = tempC;
        Elapsed = timePhi;
        Iterations = iterations;
        Error = error;
    }

    public override string ToString()
    {
        return $"LJP = {LjpMillivolts:N5} mV after {Iterations} iterations ({Error}% error) {Elapsed.TotalMilliseconds:N3} ms";
    }

    public string GetSummary()
    {
        System.Text.StringBuilder sb = new();

        sb.AppendLine(ToString());

        foreach (Ion ion in Ions)
        {
            if (double.IsFinite(ion.PercentChangeC0) && ion.PercentChangeC0 > 2)
                sb.AppendLine($"C0 change: {ion.Name} went from {ion.InitialC0:N2} to {ion.C0:N2} ({ion.PercentChangeC0:N2}%)");
            if (double.IsFinite(ion.PercentChangeCL) && ion.PercentChangeCL > 2)
                sb.AppendLine($"CL change: {ion.Name} went from {ion.InitialCL:N2} to {ion.CL:N2} ({ion.PercentChangeCL:N2}%)");
        };

        return sb.ToString();
    }
}