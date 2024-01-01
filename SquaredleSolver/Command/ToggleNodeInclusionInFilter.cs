﻿using SquaredleSolver.ViewModel;
using SquaredleSolverModel;
using System;
using System.Windows.Input;

namespace SquaredleSolver.Command;
internal class ToggleNodeInclusionInFilter : ICommand
{
    private readonly FilterNodeViewModel node;
    private readonly FilterModel filter;

    public event EventHandler? CanExecuteChanged;

    public ToggleNodeInclusionInFilter(
        FilterNodeViewModel node,
        FilterModel filter)
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