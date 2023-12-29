namespace WpfSquaredleSolver.Model;

/// <summary>
///     Represents state-specific behaviour for the puzzle solver.
/// </summary>
public interface ISolverState
{
    /// <summary>
    ///     Retrieves the static instance for a particular solver state.
    /// </summary>
    public abstract static ISolverState Instance { get; }

    /// <summary>
    ///     Triggers state-specific behaviour for when the puzzle being solved changes.
    /// </summary>
    /// <param name="context">The context for the solver.</param>
    public void OnPuzzleModelChanged(SolverContext context);

    /// <summary>
    ///     Triggers state-specific behaviour for when the puzzle being solved is completed.
    /// </summary>
    /// <param name="context">The context for the solver.</param>
    public void OnSolverCompleted(SolverContext context);

    /// <summary>
    ///     Triggers state-specific behaviour for when the puzzle solver is being commanded to start.
    /// </summary>
    /// <param name="context">The context for the solver.</param>
    public void StartSolution(SolverContext context);

    /// <summary>
    ///     Triggers state-specific behaviour for when the puzzle solver is being commanded to stop.
    /// </summary>
    /// <param name="context">The context for the solver.</param>
    public void StopSolution(SolverContext context);
}
