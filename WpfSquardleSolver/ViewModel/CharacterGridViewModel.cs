using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

class CharacterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly PuzzleModel puzzleModel;
    public uint NumberOfRows => puzzleModel.NumberOfRows;
    public uint NumberOfColumns => puzzleModel.NumberOfColumns;

    public string AnswerAsString { get; }

    public List<CharacterNode> CharacterNodes { get; }

    public CharacterGridViewModel(PuzzleModel puzzleModel, AnswerModel answerModel)
    {
        this.puzzleModel = puzzleModel;
        AnswerAsString = answerModel.Word;
        CharacterNodes = answerModel.CharacterNodes;

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
        OnPropertyChanged(nameof(AnswerAsString));
        OnPropertyChanged(nameof(CharacterNodes));
        OnPropertyChanged(nameof(NumberOfRows));
        OnPropertyChanged(nameof(NumberOfColumns));
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(puzzleModel.NumberOfColumns):
                OnPropertyChanged(nameof(NumberOfColumns));
                break;
            case nameof(puzzleModel.NumberOfRows):
                OnPropertyChanged(nameof(NumberOfRows));
                break;
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
