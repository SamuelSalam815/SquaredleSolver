using SquaredleSolver.Command;
using SquaredleSolverModel;
using SquaredleSolverModel.SolverStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties for the user interface of the main window.
/// </summary>
internal class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;
    private readonly FilterModel filterModel;

    public ICommand FocusPuzzleInput { get; }
    public ICommand ToggleSolverOnOff { get; }
    public ObservableCollection<CharacterGridViewModel> CharacterGridViewModels { get; }
    public FilterGridViewModel NodeFilterGridViewModel { get; }

    public MainWindowViewModel(
        PuzzleModel puzzleModel,
        FilterModel filterModel,
        SolverModel solverModel,
        ICommand focusPuzzleInput)
    {
        this.solverModel = solverModel;
        this.puzzleModel = puzzleModel;
        this.filterModel = filterModel;

        FocusPuzzleInput = focusPuzzleInput;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel);
        CharacterGridViewModels = new ObservableCollection<CharacterGridViewModel>();
        NodeFilterGridViewModel = new FilterGridViewModel(filterModel, puzzleModel);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFound.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnAnswersFoundChanged(sender, e));
    }

    public uint NumberOfRowsInPuzzle => puzzleModel.NumberOfRows;

    public uint NumberOfColumnsInPuzzle => puzzleModel.NumberOfColumns;

    public ISolverState SolverState => solverModel.CurrentState;

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

    private bool _hasSolverBeenRunBefore = false;
    public bool HasSolverBeenRunBefore
    {
        get { return _hasSolverBeenRunBefore; }
        set
        {
            _hasSolverBeenRunBefore = value;
            OnPropertyChanged(nameof(HasSolverBeenRunBefore));
        }
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));

        if (e.CurrentState is SolverRunning)
        {
            if (!HasSolverBeenRunBefore)
            {
                HasSolverBeenRunBefore = true;
            }

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
            CharacterGridViewModels.Add(new CharacterGridViewModel(answer, puzzleModel, filterModel));
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
