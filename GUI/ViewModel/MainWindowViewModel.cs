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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModel;

/// <summary>
///     Exposes the required properties for the user interface of the main window.
/// </summary>
internal class MainWindowViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ToggleSolverOnOff { get; }

    public string PuzzleAsText
    {
        get => puzzleModel.PuzzleAsText;
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    private int _numberOfAnswersFound;
    public int NumberOfAnswersFound
    {
        get => _numberOfAnswersFound;
        set
        {
            _numberOfAnswersFound = value;
            OnPropertyChanged(nameof(NumberOfAnswersFound));
        }
    }
    public uint NumberOfRowsInPuzzle => puzzleModel.NumberOfRows;
    public uint NumberOfColumnsInPuzzle => puzzleModel.NumberOfColumns;


    public ISolverState SolverState => solverModel.CurrentState;

    private List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public ObservableCollection<CharacterGridViewModel> CharacterGridViewModels { get; }

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

    private CancellationTokenSource solverRunTimeCancellationTokenSource;
    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        CharacterGridViewModels = new ObservableCollection<CharacterGridViewModel>();
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel);

        solverRunTimeCancellationTokenSource = new CancellationTokenSource();
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFound.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnAnswersFoundChanged(sender, e));
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));

        if (e.CurrentState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource = new CancellationTokenSource();
            UpdateSolverRunTime(
                TimeSpan.FromMilliseconds(50),
                solverRunTimeCancellationTokenSource.Token);
        }

        if (e.PreviousState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource.Cancel();
            SolverRunTime = solverModel.TimeSpentSolving;
        }
    }

    private void UpdateSolverRunTime(TimeSpan updateInterval, CancellationToken cancellationToken)
    {
        Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                SolverRunTime = solverModel.TimeSpentSolving;
                Thread.Sleep(updateInterval);
            }
        }, CancellationToken.None);
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
