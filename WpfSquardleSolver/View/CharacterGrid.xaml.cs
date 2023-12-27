using GraphWalking.Graphs;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            grid.LayoutUpdated += (sender, args) => UpdatePathGeometry();
        }

        private void DisplayNodes()
        {
            if (DataContext is not CharacterGridViewModel viewModel)
            {
                return;
            }

            grid.Children.Clear();
            grid.Children.Add(LineSegmentPath);
            grid.Children.Add(StartOfAnswerPath);
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

            foreach (CharacterNode node in viewModel.CharacterNodes)
            {
                TextBlock newTextBlock = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Text = node.Character.ToString(),
                };
                Grid.SetRow(newTextBlock, node.Row);
                Grid.SetColumn(newTextBlock, node.Column);
                grid.Children.Add(newTextBlock);
            }

            grid.UpdateLayout();
        }

        private void UpdatePathGeometry()
        {
            if (DataContext is not CharacterGridViewModel viewModel)
            {
                return;
            }

            double cellWidth = LineSegmentPath.ActualWidth / viewModel.NumberOfColumns;
            double cellHeight = LineSegmentPath.ActualHeight / viewModel.NumberOfRows;
            double circleRadius = cellWidth / 3;
            Point GetNodePosition(CharacterNode node)
            {
                return new Point(cellWidth * (0.5 + node.Column), cellHeight * (0.5 + node.Row));
            }

            CharacterNode firstNode = viewModel.CharacterNodes[0];
            StartOfAnswerPath.Data = new EllipseGeometry(
                new Point(cellWidth / 2, cellHeight / 2),
                circleRadius,
                circleRadius);
            Grid.SetRow(StartOfAnswerPath, firstNode.Row);
            Grid.SetColumn(StartOfAnswerPath, firstNode.Column);

            PathFigure answerPathFigure = new()
            {
                StartPoint = GetNodePosition(firstNode),
            };

            foreach (CharacterNode node in viewModel.CharacterNodes.Skip(1))
            {
                LineSegment lineSegment = new(GetNodePosition(node), true);
                answerPathFigure.Segments.Add(lineSegment);
            }

            PathGeometry answerPathGeometry = new();
            answerPathGeometry.Figures.Add(answerPathFigure);
            LineSegmentPath.Data = answerPathGeometry;
            Grid.SetRowSpan(LineSegmentPath, (int)viewModel.NumberOfRows);
            Grid.SetColumnSpan(LineSegmentPath, (int)viewModel.NumberOfColumns);
        }
    }
}
