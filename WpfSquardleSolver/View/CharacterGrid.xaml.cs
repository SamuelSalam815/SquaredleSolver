using GraphWalking.Graphs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfSquardleSolver
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CharacterGrid : UserControl
    {
        public CharacterGrid()
        {
            InitializeComponent();
        }

        public void DisplayNodes(List<CharacterNode> nodes, int numRows, int numColumns)
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            for (int i = 0; i < numRows; i++)
            {
                RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < numColumns; i++)
            {
                ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
                grid.ColumnDefinitions.Add(rowDefinition);
            }

            foreach (CharacterNode node in nodes)
            {
                TextBlock textBlock = new()
                {
                    Text = node.Character.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                };
                Grid.SetRow(textBlock, node.Row);
                Grid.SetColumn(textBlock, node.Column);
                grid.Children.Add(textBlock);
            }

            grid.UpdateLayout();
        }
    }
}
