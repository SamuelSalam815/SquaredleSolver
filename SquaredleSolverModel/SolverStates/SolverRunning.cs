namespace SquaredleSolverModel.SolverStates;

/// <summary>
///     Defines how the solver behaves when it is running.
/// </summary>
public class SolverRunning : ISolverState
{
    public static ISolverState Instance { get; } = new SolverRunning();

    private SolverRunning() { }

    public void OnPuzzleChanged(SolverContext context) => StopSolution(context);

    public void OnSolverCompleted(SolverContext context) => context.CurrentState = SolverCompleted.Instance;

    public void StartSolution(SolverContext context) => throw new InvalidOperationException();

    public void StopSolution(SolverContext context)
    {
        context.CancellationTokenSource.Cancel();
        context.SolverTask.Wait();
        context.CurrentState = SolverStopped.Instance;
    }
}
