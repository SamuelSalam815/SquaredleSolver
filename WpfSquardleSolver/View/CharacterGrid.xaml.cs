using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfSquaredleSolver.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CharacterGrid : UserControl
    {
        public static readonly DependencyProperty DisplayedNodesProperty =
            DependencyProperty.Register(
                nameof(NodesToDisplay),
                typeof(List<CharacterNode>),
                typeof(CharacterGrid),
                new FrameworkPropertyMetadata(defaultValue: null));
        public List<CharacterNode> NodesToDisplay
        {
            get => (List<CharacterNode>)GetValue(DisplayedNodesProperty);
            set => SetValue(DisplayedNodesProperty, value);
        }

        public static readonly DependencyProperty NumberOfRowsProperty =
            DependencyProperty.Register(
                nameof(NumberOfRows),
                typeof(int),
                typeof(CharacterGrid),
                new FrameworkPropertyMetadata(defaultValue: 0));
        public int NumberOfRows
        {
            get => (int)GetValue(NumberOfRowsProperty);
            set => SetValue(NumberOfRowsProperty, value);
        }

        public static readonly DependencyProperty NumberOfColumnsProperty =
            DependencyProperty.Register(
                nameof(NumberOfColumns),
                typeof(int),
                typeof(CharacterGrid),
                new FrameworkPropertyMetadata(defaultValue: 0));
        public int NumberOfColumns
        {
            get => (int)GetValue(NumberOfColumnsProperty);
            set => SetValue(NumberOfColumnsProperty, value);
        }

        public CharacterGrid()
        {
            InitializeComponent();
            DependencyPropertyDescriptor numberOfColumnsDescriptor =
                DependencyPropertyDescriptor.FromProperty(
                    NumberOfColumnsProperty,
                    typeof(CharacterGrid));

            numberOfColumnsDescriptor.AddValueChanged(this, (sender, e) => DisplayNodes());
        }

        public void DisplayNodes()
        {
            if (NodesToDisplay is null || NumberOfRows == 0 || NumberOfColumns == 0)
            {
                return;
            }

            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();

            for (int i = 0; i < NumberOfRows; i++)
            {
                RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
                grid.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < NumberOfColumns; i++)
            {
                ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
                grid.ColumnDefinitions.Add(rowDefinition);
            }

            TextBlock[,] textBlocks = new TextBlock[NumberOfRows, NumberOfColumns];

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int column = 0; column < NumberOfColumns; column++)
                {
                    TextBlock newTextBlock = new()
                    {
                        Text = "-",
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

            foreach (CharacterNode node in NodesToDisplay)
            {
                textBlocks[node.Row, node.Column].Text = node.Character.ToString();
            }

            grid.UpdateLayout();
        }
    }
}
