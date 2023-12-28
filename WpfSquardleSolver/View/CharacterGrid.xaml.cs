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

            // magic number 100 comes from the transparent rectangle in the xaml.
            // this rectangle maintains the aspect ratio of the image
            double cellWidth = 100 / viewModel.NumberOfColumns;
            double cellHeight = 100 / viewModel.NumberOfRows;
            double circleRadius = cellWidth / 3;
            Point GetNodePosition(CharacterNode node)
            {
                return new Point(cellWidth * (0.5 + node.Column), cellHeight * (0.5 + node.Row));
            }

            CharacterNode firstNode = viewModel.CharacterNodes[0];
            Point centreOfFirstNode = GetNodePosition(firstNode);
            EllipseDrawing.Geometry = new EllipseGeometry(
                centreOfFirstNode,
                circleRadius,
                circleRadius);

            PathFigure answerPathFigure = new()
            {
                StartPoint = centreOfFirstNode,
            };

            foreach (CharacterNode node in viewModel.CharacterNodes.Skip(1))
            {
                LineSegment lineSegment = new(GetNodePosition(node), true)
                {
                    IsSmoothJoin = true
                };
                answerPathFigure.Segments.Add(lineSegment);
            }

            PathGeometry answerPathGeometry = new();
            answerPathGeometry.Figures.Add(answerPathFigure);
            LineSegmentDrawing.Geometry = answerPathGeometry;

            Grid.SetRowSpan(HighlightedPathImage, (int)viewModel.NumberOfRows);
            Grid.SetColumnSpan(HighlightedPathImage, (int)viewModel.NumberOfColumns);
        }
    }
}
