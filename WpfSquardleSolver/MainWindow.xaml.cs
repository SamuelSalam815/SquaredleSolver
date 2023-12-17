using GraphWalking.Graphs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfSquardleSolver;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
            case Key.Space:
            case Key.Back:
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

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (NodeDisplay is not null && sender is TextBox textBox)
        {
            AdjacencyList<CharacterNode> nodeGraph = CharacterGraphBuilder.FromLetterGrid(textBox.Text);
            System.Collections.Generic.List<CharacterNode> nodes = nodeGraph.GetAllNodes();
            NodeDisplay.DisplayNodes(nodes);
        }
    }
}
