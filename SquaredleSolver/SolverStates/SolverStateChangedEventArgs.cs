namespace SquaredleSolver.SolverStates;

public class SolverStateChangedEventArgs : EventArgs
{
    public required ISolverState PreviousState;
    public required ISolverState CurrentState;
}
