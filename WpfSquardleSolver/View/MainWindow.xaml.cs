﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfSquaredleSolver.Model;
using WpfSquaredleSolver.ViewModel;

namespace WpfSquaredleSolver.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ScrollViewer? ResultsListViewScrollViewer = null;
    public MainWindow()
    {
        InitializeComponent();

        PuzzleModel puzzle = new();
        puzzle.LoadValidWords("words_alpha.txt");
        puzzle.PuzzleAsText = """
            PNOC
            RAHE
            GNGT
            IIHU
            """;
        MainWindowViewModel viewModel = new(puzzle, new SolverModel(puzzle));
        DataContext = viewModel;

        viewModel.CharacterGridViewModels.CollectionChanged += UpdateWrapPanelWidth;
    }

    private void UpdateWrapPanelWidth(object? sender, System.EventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        double wrapPanelWidth = ResultsListView.ActualWidth;
        Decorator? border = VisualTreeHelper.GetChild(ResultsListView, 0) as Decorator;
        ScrollViewer? scrollViewer = border.Child as ScrollViewer;
        if (scrollViewer.ComputedVerticalScrollBarVisibility is Visibility.Visible)
        {
            wrapPanelWidth -= SystemParameters.VerticalScrollBarWidth;
        }

        viewModel.WrapPanelWidth = wrapPanelWidth;
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
            case Key.Space:
            case Key.Back:
            case Key.Delete:
            case Key.Home:
            case Key.End:
            case Key.Left:
            case Key.Right:
            case Key.Up:
            case Key.Down:
            case >= Key.A and <= Key.Z:
                return;
        }

        e.Handled = true;
    }

}
