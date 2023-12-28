using GraphWalking.Graphs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WpfSquaredleSolver.Command;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

internal class MainWindowViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ToggleSolverOnOff { get; }

    public string PuzzleAsText
    {
        get { return puzzleModel.PuzzleAsText; }
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    public uint NumberOfRowsInPuzzle => puzzleModel.NumberOfRows;
    public uint NumberOfColumnsInPuzzle => puzzleModel.NumberOfColumns;

    private bool isSolverRunningBackingField = false;
    public bool IsSolverRunning
    {
        get { return isSolverRunningBackingField; }
        set
        {
            isSolverRunningBackingField = value;
            OnPropertyChanged(nameof(IsSolverRunning));
        }
    }

    private List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public ObservableCollection<CharacterGridViewModel> CharacterGridViewModels { get; }
    public ObservableCollection<AnswerModel> AnswersFoundInPuzzle =>
        solverModel.AnswersFoundInPuzzle;

    private double backingWrapPanelWidth;
    public double WrapPanelWidth
    {
        get { return backingWrapPanelWidth; }
        set
        {
            backingWrapPanelWidth = value;
            OnPropertyChanged(nameof(WrapPanelWidth));
        }
    }

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        CharacterGridViewModels = new ObservableCollection<CharacterGridViewModel>();
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel, this);

        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFoundInPuzzle.CollectionChanged += OnAnswersFoundInPuzzleChanged;
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        IsSolverRunning = e.CurrentState is SolverRunning;
    }

    private void OnAnswersFoundInPuzzleChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        IEnumerable<AnswerModel> answersToAdd;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                answersToAdd = e.NewItems.Cast<AnswerModel>();
                break;
            default:
                CharacterGridViewModels.Clear();
                answersToAdd = solverModel.AnswersFoundInPuzzle;
                break;
        }

        foreach (AnswerModel answerModel in answersToAdd)
        {
            CharacterGridViewModel viewModel = new(puzzleModel, answerModel);
            CharacterGridViewModels.Add(viewModel);
        }
    }

    private void OnPropertyChanged(string nameOfProperty)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nameOfProperty));
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(puzzleModel.PuzzleAsAdjacencyList):
                OnPropertyChanged(nameof(PuzzleAsCharacterNodes));
                break;
            case nameof(puzzleModel.NumberOfRows):
                OnPropertyChanged(nameof(NumberOfRowsInPuzzle));
                break;
            case nameof(puzzleModel.NumberOfColumns):
                OnPropertyChanged(nameof(NumberOfColumnsInPuzzle));
                break;
        }
    }

}
