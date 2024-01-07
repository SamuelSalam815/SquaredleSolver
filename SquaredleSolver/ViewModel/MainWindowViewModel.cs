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
    private readonly Dictionary<string, AnswerTileViewModel> answerTileIndex;

    public ICommand FocusPuzzleInput { get; }
    public ICommand ToggleSolverOnOff { get; }
    public ObservableCollection<AnswerTileViewModel> AnswerTilesDisplayed { get; }
    public FilterGridViewModel NodeFilterGridViewModel { get; }
    public ObservableCollection<string> AttemptedWords => filterModel.AttemptedWords;
    public int NumberOfAnswersFound => solverModel.AnswersFound.Count;
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

    public MainWindowViewModel(
        PuzzleModel puzzleModel,
        FilterModel filterModel,
        SolverModel solverModel,
        ICommand focusPuzzleInput,
        ICommand toggleSolverOnOff)
    {
        this.solverModel = solverModel;
        this.puzzleModel = puzzleModel;
        this.filterModel = filterModel;
        answerTileIndex = new Dictionary<string, AnswerTileViewModel>();

        FocusPuzzleInput = focusPuzzleInput;
        ToggleSolverOnOff = toggleSolverOnOff;
        AnswerTilesDisplayed = new ObservableCollection<AnswerTileViewModel>();
        NodeFilterGridViewModel = new FilterGridViewModel(filterModel, puzzleModel);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFound.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnAnswersFoundChanged(sender, e));
        filterModel.AttemptedWords.CollectionChanged += OnAttemptedWordsChanged;
    }

    private void OnAttemptedWordsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is null)
                {
                    break;
                }

                List<string> newDisallowedWords = e.NewItems.Cast<string>().ToList();
                IEnumerable<AnswerTileViewModel> viewModelsToRemove =
                    AnswerTilesDisplayed
                    .Where(tile => newDisallowedWords.Contains(tile.Answer.Word))
                    .ToList();
                foreach (AnswerTileViewModel tile in viewModelsToRemove)
                {
                    AnswerTilesDisplayed.Remove(tile);
                }

                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is null)
                {
                    break;
                }

                List<string> newAllowedWords = e.OldItems.Cast<string>().ToList();
                List<AnswerModel> answersToDisplay = new();
                foreach (string item in newAllowedWords)
                {
                    if (!answerTileIndex.TryGetValue(item, out AnswerTileViewModel? answerTile) || answerTile is null)
                    {
                        continue;
                    }
                    AnswerTilesDisplayed.Insert(0, answerTile);
                }

                break;
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
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is null)
                {
                    answersToAdd = Enumerable.Empty<AnswerModel>();
                }
                else
                {
                    answersToAdd = e.NewItems.Cast<AnswerModel>();
                }
                break;
            default:
                AnswerTilesDisplayed.Clear();
                answerTileIndex.Clear();
                answersToAdd = solverModel.AnswersFound;
                break;
        }

        foreach (AnswerModel answer in answersToAdd)
        {
            AddAnswerIfNotAttempted(answer);
        }

        OnPropertyChanged(nameof(NumberOfAnswersFound));
    }

    private void AddAnswerIfNotAttempted(AnswerModel answer)
    {
        if (!filterModel.AttemptedWords.Contains(answer.Word))
        {
            AnswerTileViewModel answerTile = new(answer, puzzleModel, filterModel);
            AnswerTilesDisplayed.Add(answerTile);
            answerTileIndex.Add(answer.Word, answerTile);
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
        AnswerTilesDisplayed.Clear();
    }
}
