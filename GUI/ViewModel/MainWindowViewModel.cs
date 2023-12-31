using GraphWalking.Graphs;
using GUI.Command;
using SquaredleSolver;
using SquaredleSolver.SolverStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModel;

/// <summary>
///     Exposes the required properties for the user interface of the main window.
/// </summary>
internal class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public ICommand FocusPuzzleInput { get; }
    public ICommand ToggleSolverOnOff { get; }
    public ObservableCollection<CharacterGridViewModel> CharacterGridViewModels { get; }
    public NodeFilterGridViewModel NodeFilterGridViewModel { get; }

    public MainWindowViewModel(
        PuzzleModel puzzleModel,
        NodeFilterModel filterModel,
        SolverModel solverModel,
        ICommand focusPuzzleInput)
    {
        this.solverModel = solverModel;
        this.puzzleModel = puzzleModel;

        FocusPuzzleInput = focusPuzzleInput;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel);
        CharacterGridViewModels = new ObservableCollection<CharacterGridViewModel>();
        NodeFilterGridViewModel = new NodeFilterGridViewModel(filterModel, puzzleModel);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFound.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnAnswersFoundChanged(sender, e));
    }

    public uint NumberOfRowsInPuzzle => puzzleModel.NumberOfRows;

    public uint NumberOfColumnsInPuzzle => puzzleModel.NumberOfColumns;

    public ISolverState SolverState => solverModel.CurrentState;

    private List<CharacterNode> PuzzleAsCharacterNodes => puzzleModel.PuzzleAsAdjacencyList.GetAllNodes();

    public string PuzzleAsText
    {
        get => puzzleModel.PuzzleAsText;
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    private int _numberOfAnswersFound = 0;
    public int NumberOfAnswersFound
    {
        get => _numberOfAnswersFound;
        set
        {
            _numberOfAnswersFound = value;
            OnPropertyChanged(nameof(NumberOfAnswersFound));
        }
    }

    private double _wrapPanelWidth = 0;
    public double WrapPanelWidth
    {
        get { return _wrapPanelWidth; }
        set
        {
            _wrapPanelWidth = value;
            OnPropertyChanged(nameof(WrapPanelWidth));
        }
    }

    private TimeSpan _solverRunTime = TimeSpan.Zero;
    public TimeSpan SolverRunTime
    {
        get { return _solverRunTime; }
        set
        {
            _solverRunTime = value;
            OnPropertyChanged(nameof(SolverRunTime));
        }
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));

        if (e.CurrentState is SolverRunning)
        {
            SolverRunTime = TimeSpan.Zero;
        }

        if (e.PreviousState is SolverRunning)
        {
            SolverRunTime = solverModel.TimeSpentSolving;
        }
    }

    private void OnAnswersFoundChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        IEnumerable<AnswerModel> answersToAdd;
        NumberOfAnswersFound = solverModel.AnswersFound.Count;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                answersToAdd = e.NewItems.Cast<AnswerModel>();
                break;
            default:
                CharacterGridViewModels.Clear();
                answersToAdd = solverModel.AnswersFound;
                break;
        }

        foreach (AnswerModel answer in answersToAdd)
        {
            CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, answer));
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
