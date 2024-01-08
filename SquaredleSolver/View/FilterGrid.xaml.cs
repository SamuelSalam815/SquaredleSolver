using GraphWalking.Graphs;
using SquaredleSolver.ViewModel;
using SquaredleSolverModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SquaredleSolver.View;
/// <summary>
/// Interaction logic for NodeFilterGrid.xaml
/// </summary>
public partial class FilterGrid : UserControl
{
    FilterGridViewModel? viewModel = null;
    public FilterGrid()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is FilterGridViewModel newViewModel)
        {
            if (viewModel is not null)
            {
                viewModel.Puzzle.PropertyChanged -= OnPuzzleChanged;
            }

            viewModel = newViewModel;
            viewModel.Puzzle.PropertyChanged += OnPuzzleChanged;
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

        PuzzleModel puzzle = viewModel.Puzzle;

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

        foreach (CharacterNode node in puzzle.PuzzleAsNodes)
        {
            FilterNodeViewModel nodeViewModel = viewModel.Filter.FilterNodes[node];
            Border border = new()
            {
                DataContext = nodeViewModel,
                Child = new TextBlock()
            };
            MouseGesture leftClick = new(MouseAction.LeftClick, ModifierKeys.None);
            MouseBinding toggleInclusionBinding = new(nodeViewModel.ToggleInclusionCommand, leftClick);
            border.InputBindings.Add(toggleInclusionBinding);
            ButtonGrid.Children.Add(border);
        }

        ButtonGrid.UpdateLayout();
    }
}
