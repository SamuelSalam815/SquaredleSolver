using SquaredleSolver.Command;
using SquaredleSolver.ViewModel;
using SquaredleSolverModel;
using SquaredleSolverModel.Solver;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SquaredleSolver.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PuzzleModel puzzle = new();
        puzzle.LoadValidWords("words_alpha.txt");
        SolverModel solver = new(puzzle);
        FilterViewModel filter = new(puzzle, solver);
        DelegateCommand focusPuzzleInput = new(() =>
            {
                InputField.Visibility = Visibility.Visible;
                InputField.Focus();
            });
        MainWindowViewModel viewModel = new(puzzle, solver, filter, focusPuzzleInput);
        DataContext = viewModel;
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
            case Key.Escape:
                ToggleSolverButton.Focus();
                break;
        }

        e.Handled = true;
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        ToggleSolverButton.Focus();
    }

    private void InputField_LostFocus(object sender, RoutedEventArgs e)
    {
        InputField.Visibility = Visibility.Collapsed;
    }

    private void AttemptedWord_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not TextBlock textBlock)
        {
            return;
        }

        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        viewModel.AttemptedWords.Remove(textBlock.Text);
    }
}
