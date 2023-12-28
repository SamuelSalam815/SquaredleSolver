using System;

namespace WpfSquaredleSolver.Model;

internal class SolverCompleted : ISolverState
{
    public static ISolverState Instance { get; } = new SolverCompleted();

    private SolverCompleted() { }

    public void OnPuzzleModelChanged(SolverContext context)
    {
        context.AnswersFoundInPuzzle.Clear();
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
