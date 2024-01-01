using SquaredleSolverModel;

namespace SquaredleSolverModel.SolverStates;

/// <summary>
///     Defines how the puzzle solver behaves when it has solved the current puzzle. - Not used yet
/// </summary>
public class SolverCompleted : ISolverState
{
    public static ISolverState Instance { get; } = new SolverCompleted();

    private SolverCompleted() { }

    public void OnPuzzleChanged(SolverContext context) => context.CurrentState = SolverStopped.Instance;

    public void OnNodeFilterChanged(SolverContext context) => context.CurrentState = SolverStopped.Instance;

    public void OnSolverCompleted(SolverContext context) => throw new InvalidOperationException();

    public void StartSolution(SolverContext context) => throw new InvalidOperationException();

    public void StopSolution(SolverContext context) => throw new InvalidOperationException();
}
