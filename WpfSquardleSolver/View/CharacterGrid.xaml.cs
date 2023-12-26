using GraphWalking.Graphs;
using System.Collections.Generic;
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
                new FrameworkPropertyMetadata());
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
            //DisplayNodes();
        }

        public void DisplayNodes()
        {
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

            foreach (CharacterNode node in NodesToDisplay)
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
