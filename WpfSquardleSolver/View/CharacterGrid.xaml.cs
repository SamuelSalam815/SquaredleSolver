using GraphWalking.Graphs;
using System.Windows;
using System.Windows.Controls;
using WpfSquaredleSolver.ViewModel;

namespace WpfSquaredleSolver.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CharacterGrid : UserControl
    {

        public CharacterGrid()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) => DisplayNodes();
        }

        public void DisplayNodes()
        {
            if (DataContext is not CharacterGridViewModel viewModel)
            {
                return;
            }

            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            for (int i = 0; i < viewModel.NumberOfRows; i++)
            {
                RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < viewModel.NumberOfColumns; i++)
            {
                ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
                grid.ColumnDefinitions.Add(rowDefinition);
            }

            TextBlock[,] textBlocks = new TextBlock[viewModel.NumberOfRows, viewModel.NumberOfColumns];

            for (int row = 0; row < viewModel.NumberOfRows; row++)
            {
                for (int column = 0; column < viewModel.NumberOfColumns; column++)
                {
                    TextBlock newTextBlock = new()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                    };
                    textBlocks[row, column] = newTextBlock;
                    Grid.SetRow(newTextBlock, row);
                    Grid.SetColumn(newTextBlock, column);
                    grid.Children.Add(newTextBlock);
                }
            }

            foreach (CharacterNode node in viewModel.CharacterNodes)
            {
                textBlocks[node.Row, node.Column].Text = node.Character.ToString();
            }

            grid.UpdateLayout();
        }
    }
}
