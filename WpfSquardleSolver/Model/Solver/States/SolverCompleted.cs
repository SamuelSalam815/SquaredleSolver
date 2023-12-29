using System;

namespace WpfSquaredleSolver.Model;

/// <summary>
///     Defines how the puzzle solver behaves when it has solved the current puzzle. - Not used yet
/// </summary>
internal class SolverCompleted : ISolverState
{
    public static ISolverState Instance { get; } = new SolverCompleted();

    private SolverCompleted() { }

    public void OnPuzzleModelChanged(SolverContext context)
    {
        context.CurrentState = SolverStopped.Instance;
    }

    public void OnSolverCompleted(SolverContext context)
    {
        throw new InvalidOperationException();
    }

    public void StartSolution(SolverContext context)
    {
        throw new InvalidOperationException();
    }

    public void StopSolution(SolverContext context)
    {
        throw new InvalidOperationException();
    }
}
