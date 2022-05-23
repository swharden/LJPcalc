namespace LJPcalc.Core;

/// <summary>
/// Helper methods to calculate LJP from a single function call
/// </summary>
public static class Calculate
{
    public static LjpResult Ljp(Ion[] ions, double temperatureC = 25)
    {
        LjpCalculationOptions defaultOptions = new() { TemperatureC = temperatureC };
        return Ljp(ions, defaultOptions);
    }

    public static LjpResult Ljp(Ion[] ions, LjpCalculationOptions options)
    {
        LjpCalculator calc = new(ions, options.TemperatureC, options.AutoSort);
        calc.IterateRepeatedly(1, options.MaximumIterations);

        if (calc.BestSolutionMaxError > 1 && options.ThrowIfIterationLimitExceeded)
            throw new OperationCanceledException();

        return calc.GetLJP();
    }
}
