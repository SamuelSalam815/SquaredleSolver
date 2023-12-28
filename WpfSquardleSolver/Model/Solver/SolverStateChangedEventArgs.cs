using System;

namespace WpfSquaredleSolver.Model;

internal class SolverStateChangedEventArgs : EventArgs
{
    public required ISolverState PreviousState;
    public required ISolverState CurrentState;
}
