using System;

namespace WpfSquaredleSolver.Model;

public class SolverStateChangedEventArgs : EventArgs
{
    public required ISolverState PreviousState;
    public required ISolverState CurrentState;
}
