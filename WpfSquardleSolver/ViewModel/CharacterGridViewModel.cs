using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties to render a visual representation of a squaredle answer.
/// </summary>
class CharacterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public uint NumberOfRows { get; }
    public uint NumberOfColumns { get; }
    public List<CharacterNode> PuzzleNodes { get; }
    public string AnswerAsString { get; }
    public List<CharacterNode> AnswerAsNodes { get; }

    public CharacterGridViewModel(PuzzleModel puzzleModel, AnswerModel answerModel)
    {
        NumberOfRows = puzzleModel.NumberOfRows;
        NumberOfColumns = puzzleModel.NumberOfColumns;
        PuzzleNodes = puzzleModel.PuzzleAsAdjacencyList.GetAllNodes();
        AnswerAsString = answerModel.Word;
        AnswerAsNodes = answerModel.CharacterNodes;

        OnPropertyChanged(nameof(NumberOfRows));
        OnPropertyChanged(nameof(NumberOfColumns));
        OnPropertyChanged(nameof(PuzzleNodes));
        OnPropertyChanged(nameof(AnswerAsString));
        OnPropertyChanged(nameof(AnswerAsNodes));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
