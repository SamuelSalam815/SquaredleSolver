using SquaredleSolver.ViewModel;
using System;
using System.Windows.Input;

namespace SquaredleSolver.Command;
internal class ToggleNodeInclusionInFilter : ICommand
{
    private readonly FilterNodeViewModel node;

    public event EventHandler? CanExecuteChanged;

    public ToggleNodeInclusionInFilter(FilterNodeViewModel node)
    {
        this.node = node;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        node.ToggleInclusion();
    }
}
