using GUI.ViewModel;
using SquaredleSolver;
using System;
using System.Windows.Input;

namespace GUI.Command;
internal class ToggleNodeInclusionInFilter : ICommand
{
    private readonly NodeViewModel node;
    private readonly NodeFilterModel filter;

    public event EventHandler? CanExecuteChanged;

    public ToggleNodeInclusionInFilter(
        NodeViewModel node,
        NodeFilterModel filter)
    {
        this.node = node;
        this.filter = filter;
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
    }
}
