using System.Windows;
using System.Windows.Input;
using WpfSquardleSolver.Command;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SolverModel solver;
    private readonly PuzzleModel puzzle;
    public ICommand ToggleSolverOnOff;

    public MainWindow()
    {
        puzzle = new PuzzleModel();
        puzzle.LoadValidWords("words_alpha.txt");
        solver = new SolverModel(puzzle);
        ToggleSolverOnOff = new ToggleSolverOnOff(solver);
        InitializeComponent();
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
