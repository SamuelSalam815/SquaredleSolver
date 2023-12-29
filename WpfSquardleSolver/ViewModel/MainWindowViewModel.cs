using GraphWalking.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfSquaredleSolver.Command;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties for the user interface of the main window.
/// </summary>
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

    public ISolverState SolverState => solverModel.CurrentState;

    private List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public BindingList<CharacterGridViewModel> CharacterGridViewModels { get; }

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
    private Task updateSolverRunTimeTask;
    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        CharacterGridViewModels = new BindingList<CharacterGridViewModel>();
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel);

        solverRunTimeCancellationTokenSource = new CancellationTokenSource();
        updateSolverRunTimeTask = Task.CompletedTask;
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFoundInPuzzle.ListChanged += OnAnswersFoundInPuzzleChanged;
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));

        if (e.CurrentState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource = new CancellationTokenSource();
            updateSolverRunTimeTask = UpdateSolverRunTime(
                TimeSpan.FromMilliseconds(50),
                solverRunTimeCancellationTokenSource.Token);
        }

        if (e.PreviousState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource.Cancel();
            SolverRunTime = solverModel.StopTime - solverModel.StartTime;
        }
    }

    private async Task UpdateSolverRunTime(TimeSpan updateInterval, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            SolverRunTime = DateTime.Now - solverModel.StartTime;
            await Task.Delay(updateInterval, cancellationToken);
        }
    }

    private void OnAnswersFoundInPuzzleChanged(object? sender, ListChangedEventArgs e)
    {
        switch (e.ListChangedType)
        {
            case ListChangedType.ItemAdded:
                AnswerModel nextAnswer = solverModel.AnswersFoundInPuzzle[e.NewIndex];
                CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, nextAnswer));
                break;
            default:
                CharacterGridViewModels.Clear();
                foreach (AnswerModel answer in solverModel.AnswersFoundInPuzzle)
                {
                    CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, answer));
                }
                break;
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
