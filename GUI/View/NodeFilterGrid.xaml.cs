﻿using GraphWalking.Graphs;
using GUI.ViewModel;
using SquaredleSolver;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.View;
/// <summary>
/// Interaction logic for NodeFilterGrid.xaml
/// </summary>
public partial class NodeFilterGrid : UserControl
{
    NodeFilterGridViewModel? viewModel = null;
    public NodeFilterGrid()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is NodeFilterGridViewModel newViewModel)
        {
            if (viewModel is not null)
            {
                viewModel.PuzzleModel.PropertyChanged -= OnPuzzleChanged;
            }

            viewModel = newViewModel;
            viewModel.PuzzleModel.PropertyChanged += OnPuzzleChanged;
            RepopulateGrid();
        }
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        RepopulateGrid();
    }

    private void RepopulateGrid()
    {
        if (viewModel is null)
        {
            return;
        }

        PuzzleModel puzzle = viewModel.PuzzleModel;

        ButtonGrid.Children.Clear();
        ButtonGrid.RowDefinitions.Clear();
        for (int i = 0; i < puzzle.NumberOfRows; i++)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
            ButtonGrid.RowDefinitions.Add(rowDefinition);
        }

        ButtonGrid.ColumnDefinitions.Clear();
        for (int i = 0; i < puzzle.NumberOfColumns; i++)
        {
            ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
            ButtonGrid.ColumnDefinitions.Add(rowDefinition);
        }

        foreach (CharacterNode node in puzzle.PuzzleAsAdjacencyList.GetAllNodes())
        {
            NodeViewModel nodeViewModel = new(node, viewModel.NodeFilterModel);
            Border border = new()
            {
                DataContext = nodeViewModel,
                Child = new TextBlock()
                {
                    DataContext = nodeViewModel
                }
            };
            MouseGesture leftClick = new(MouseAction.LeftClick, ModifierKeys.None);
            MouseBinding toggleInclusionBinding = new(nodeViewModel.ToggleNodeInclusion, leftClick);
            border.InputBindings.Add(toggleInclusionBinding);
            ButtonGrid.Children.Add(border);
        }

        ButtonGrid.UpdateLayout();
    }
}
