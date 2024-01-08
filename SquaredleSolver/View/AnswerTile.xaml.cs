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
public partial class AnswerTile : UserControl
{

    public AnswerTile()
    {
        InitializeComponent();
        DataContextChanged += (sender, args) => DisplayNodes();
    }

    private void DisplayNodes()
    {
        if (DataContext is not AnswerTileViewModel viewModel)
        {
            return;
        }

        TileGrid.RowDefinitions.Clear();
        for (int i = 0; i < viewModel.Puzzle.NumberOfRows; i++)
        {
            RowDefinition rowDefinition = new() { Height = new GridLength(1, GridUnitType.Star) };
            TileGrid.RowDefinitions.Add(rowDefinition);
        }

        TileGrid.ColumnDefinitions.Clear();
        for (int i = 0; i < viewModel.Puzzle.NumberOfColumns; i++)
        {
            ColumnDefinition rowDefinition = new() { Width = new GridLength(1, GridUnitType.Star) };
            TileGrid.ColumnDefinitions.Add(rowDefinition);
        }

        foreach (CharacterNode node in viewModel.Puzzle.PuzzleAsNodes)
        {
            FilterNodeViewModel filterNode = viewModel.Filter.FilterNodes[node];
            AnswerTileCharacterNodeViewModel nodeViewModel = new(filterNode, viewModel.Answer);
            Border textBlockBorder = new()
            {
                DataContext = nodeViewModel,
                Child = new TextBlock()
            };
            TileGrid.Children.Add(textBlockBorder);
        }

        UpdatePathGeometry();
        TileGrid.UpdateLayout();
    }

    private void UpdatePathGeometry()
    {
        if (DataContext is not AnswerTileViewModel viewModel)
        {
            return;
        }

        double cellSize = 100;
        TransparentPathLayer.Geometry = new RectangleGeometry
        {
            Rect = new Rect(
                0,
                0,
                cellSize * viewModel.Puzzle.NumberOfColumns,
                cellSize * viewModel.Puzzle.NumberOfRows)
        };

        Point GetNodePosition(CharacterNode node)
        {
            return new Point(cellSize * (0.5 + node.Column), cellSize * (0.5 + node.Row));
        }

        CharacterNode firstNode = viewModel.Answer.CharacterNodes[0];
        Point centreOfFirstNode = GetNodePosition(firstNode);
        double circleRadius = cellSize / 3;
        EllipseDrawing.Geometry = new EllipseGeometry(
            centreOfFirstNode,
            circleRadius,
            circleRadius);

        double penThickness = circleRadius * 0.8;
        LineSegmentDrawing.Pen.Thickness = penThickness;
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
    }

    private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (DataContext is not AnswerTileViewModel viewModel)
        {
            return;
        }

        viewModel.Filter.AttemptedWords.Add(viewModel.Answer.Word);
    }
}
