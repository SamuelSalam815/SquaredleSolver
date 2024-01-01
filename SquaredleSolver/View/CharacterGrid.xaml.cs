using GraphWalking.Graphs;
using SquaredleSolver.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SquaredleSolver.View;

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

    private void DisplayNodes()
    {
        if (DataContext is not CharacterGridViewModel viewModel)
        {
            return;
        }

        for (int i = 0; i < viewModel.Puzzle.NumberOfRows; i++)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
            grid.RowDefinitions.Add(rowDefinition);
        }

        for (int i = 0; i < viewModel.Puzzle.NumberOfColumns; i++)
        {
            ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
            grid.ColumnDefinitions.Add(rowDefinition);
        }

        foreach (CharacterNode node in viewModel.Puzzle.PuzzleAsNodes)
        {
            CharacterNodeViewModel nodeViewModel = new(node, viewModel.Answer, viewModel.Filter);
            Border textBlockBorder = new()
            {
                DataContext = nodeViewModel,
                Child = new Viewbox()
                {
                    Child = new TextBlock()
                    {
                        Text = node.Character.ToString(),
                    }
                }
            };
            Grid.SetRow(textBlockBorder, node.Row);
            Grid.SetColumn(textBlockBorder, node.Column);
            grid.Children.Add(textBlockBorder);
        }

        grid.UpdateLayout();
        UpdatePathGeometry();
    }

    private void UpdatePathGeometry()
    {
        if (DataContext is not CharacterGridViewModel viewModel)
        {
            return;
        }

        // magic number 100 comes from the transparent rectangle in the xaml.
        // this rectangle maintains the aspect ratio of the image
        double cellWidth = 100 / viewModel.Puzzle.NumberOfColumns;
        double cellHeight = 100 / viewModel.Puzzle.NumberOfRows;
        double circleRadius = cellWidth / 3;
        Point GetNodePosition(CharacterNode node)
        {
            return new Point(cellWidth * (0.5 + node.Column), cellHeight * (0.5 + node.Row));
        }

        CharacterNode firstNode = viewModel.Answer.CharacterNodes[0];
        Point centreOfFirstNode = GetNodePosition(firstNode);
        EllipseDrawing.Geometry = new EllipseGeometry(
            centreOfFirstNode,
            circleRadius,
            circleRadius);

        PathFigure answerPathFigure = new()
        {
            StartPoint = centreOfFirstNode,
        };

        foreach (CharacterNode node in viewModel.Answer.CharacterNodes.Skip(1))
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

        Grid.SetRowSpan(HighlightedPathImage, (int)viewModel.Puzzle.NumberOfRows);
        Grid.SetColumnSpan(HighlightedPathImage, (int)viewModel.Puzzle.NumberOfColumns);
    }
}
