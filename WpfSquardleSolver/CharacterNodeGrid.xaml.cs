using GraphWalking.Graphs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfSquardleSolver
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CharacterNodeGrid : UserControl
    {
        private Grid grid;
        public CharacterNodeGrid()
        {
            InitializeComponent();
            grid = new Grid();
            AddChild(grid);
        }

        public void DisplayNodes(List<CharacterNode> nodes)
        {
            grid.Width = Width;
            grid.Height = Height;
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            int maxRow = 0;
            int maxColumn = 0;

            grid.Children.Add(new Border());

            foreach (CharacterNode node in nodes)
            {
                maxRow = Math.Max(maxRow, node.Row);
                maxColumn = Math.Max(maxColumn, node.Column);
            }

            for (int i = 0; i <= maxRow; i++)
            {
                RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i <= maxColumn; i++)
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
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(textBlock, node.Row);
                Grid.SetColumn(textBlock, node.Column);
                grid.Children.Add(textBlock);
            }

            grid.UpdateLayout();
        }
    }
}
