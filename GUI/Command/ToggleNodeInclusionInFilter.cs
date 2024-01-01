using GUI.ViewModel;
using SquaredleSolver;
using System;
using System.Windows.Input;

namespace GUI.Command;
internal class ToggleNodeInclusionInFilter : ICommand
{
    private readonly FilterNodeViewModel node;
    private readonly FilterModel filter;
    private readonly SolverModel solver;

    public event EventHandler? CanExecuteChanged;

    public ToggleNodeInclusionInFilter(
        FilterNodeViewModel node,
        FilterModel filter,
        SolverModel solver)
    {
        this.node = node;
        this.filter = filter;
        this.solver = solver;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (node.IsIncluded)
        {
            filter.ExcludeNode(node.CharacterNode);
            node.IsIncluded = false;
        }
        else
        {
            filter.IncludeNode(node.CharacterNode);
            node.IsIncluded = true;
        }

        solver.StartSolvingPuzzle();
    }
}
