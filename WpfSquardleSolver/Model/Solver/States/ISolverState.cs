namespace WpfSquaredleSolver.Model;

internal interface ISolverState
{
    public abstract static ISolverState Instance { get; }
    public void OnPuzzleModelChanged(SolverContext context);
    public void OnSolverCompleted(SolverContext context);
    public void StartSolution(SolverContext context);
    public void StopSolution(SolverContext context);
}
