using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

class CharacterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public uint NumberOfRows { get; }
    public uint NumberOfColumns { get; }
    public string AnswerAsString { get; }

    public List<CharacterNode> CharacterNodes { get; }

    public CharacterGridViewModel(PuzzleModel puzzleModel, AnswerModel answerModel)
    {
        NumberOfRows = puzzleModel.NumberOfRows;
        NumberOfColumns = puzzleModel.NumberOfColumns;
        AnswerAsString = answerModel.Word;
        CharacterNodes = answerModel.CharacterNodes;

        OnPropertyChanged(nameof(AnswerAsString));
        OnPropertyChanged(nameof(CharacterNodes));
        OnPropertyChanged(nameof(NumberOfRows));
        OnPropertyChanged(nameof(NumberOfColumns));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
