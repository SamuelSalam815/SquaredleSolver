namespace SquaredleSolverModel.Solver;

public class SolverStateChangedEventArgs : EventArgs
{
    public readonly SolverState PreviousState;
    public readonly SolverState CurrentState;

    public SolverStateChangedEventArgs(SolverState previousState, SolverState currentState)
    {
        PreviousState = previousState;
        CurrentState = currentState;
    }
}