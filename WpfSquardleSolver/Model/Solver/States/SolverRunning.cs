using System;

namespace WpfSquaredleSolver.Model;

internal class SolverRunning : ISolverState
{
    public static ISolverState Instance { get; } = new SolverRunning();

    private SolverRunning() { }

    public void OnPuzzleModelChanged(SolverContext context)
    {
        StopSolution(context);
    }

    public void OnSolverCompleted(SolverContext context)
    {
        context.CurrentState = SolverStopped.Instance;
    }

    public void StartSolution(SolverContext context)
    {
        throw new InvalidOperationException();
    }

    public void StopSolution(SolverContext context)
    {
        context.CancellationTokenSource.Cancel();
        context.SolverTask.Wait();
        context.AnswersFoundInPuzzle.Clear();
        context.CurrentState = SolverStopped.Instance;
    }
}
