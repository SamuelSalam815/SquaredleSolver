using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfSquardleSolver;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private PuzzleSolver puzzleSolver;

    public MainWindow()
    {
        InitializeComponent();
        HashSet<string> allowedWords = new();

        using StreamReader reader = new("words_alpha.txt");
        string? line = reader.ReadLine();
        while (line is not null)
        {
            allowedWords.Add(line.ToUpper());
            line = reader.ReadLine();
        }

        puzzleSolver = new(allowedWords);
        ResultsListView.ItemsSource = puzzleSolver.Answers;
        puzzleSolver.PropertyChanged += OnPuzzleSolverStatusChanged;
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

    private void OnPuzzleSolverStatusChanged(object? sender, PropertyChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (puzzleSolver.IsSolverRunning)
            {
                ToggleSolverButton.Content = "Stop Solving Puzzle";
                ToggleSolverButton.Background = new BrushConverter().ConvertFrom("#e74c3c") as SolidColorBrush;
            }
            else
            {
                ToggleSolverButton.Content = "Start Solving Puzzle";
                ToggleSolverButton.Background = new BrushConverter().ConvertFrom("#2ecc71") as SolidColorBrush;
            }
        });
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender == ToggleSolverButton)
        {
            if (puzzleSolver.IsSolverRunning)
            {
                puzzleSolver.StopSolvingPuzzle();
            }
            else
            {
                puzzleSolver.UpdatePuzzle(InputField.Text);
                puzzleSolver.StartSolvingPuzzle();
            }
        }
    }
}
