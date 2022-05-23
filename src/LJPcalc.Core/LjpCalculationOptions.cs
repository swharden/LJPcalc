namespace LJPcalc.Core;

public class LjpCalculationOptions
{
    public double TemperatureC { get; set; } = 25;

    /// <summary>
    /// If enabled, the given list of ions will be sorted to maximize likelyhood of balancing phis quickly.
    /// The ion with the largest difference between C0 and CL will be placed last
    /// and the remaining ion with the largest CL will be placed second to last.
    /// </summary>
    public bool AutoSort { get; set; } = true;

    /// <summary>
    /// Solving the set of equations to find ideal CLs which achieve electroneutrality can be difficult.
    /// A solution is approached iteratively, but it is possible no solution will be found.
    /// Cancel trying to find a solution after this many iterations.
    /// </summary>
    public int MaximumIterations { get; set; } = 3000;

    /// <summary>
    /// Controls whether a hard exception is thrown if a solution is not found within the iteration limit.
    /// If false, calculation of LJP will continue using the best solution that was found so far.
    /// </summary>
    public bool ThrowIfIterationLimitExceeded { get; set; } = false;

    /// <summary>
    /// This event is invoked after each iteration of the phi solver.
    /// The sender is the <seealso cref="Solver.EquationSolver"/>.
    /// </summary>
    [Obsolete("DO NOT USE EVENT HANDLING")]
    public event EventHandler? IterationFinished;

    /// <summary>
    /// This function is called by the solver when new iterations complete
    /// </summary>
    public void OnIterationFinished(object? sender, EventArgs e) => IterationFinished?.Invoke(sender, e);
}
