using System.Windows;
using System.Windows.Input;
using WpfSquardleSolver.Model;
using WpfSquardleSolver.ViewModel;

namespace WpfSquardleSolver.View;
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
        puzzle.PuzzleAsText = """
            PNOC
            RAHE
            GNGT
            IIHU
            """;
        DataContext = new MainWindowViewModel(puzzle, new SolverModel(puzzle));
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
