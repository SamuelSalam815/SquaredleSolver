using GraphWalking.Graphs;
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
    public int NumberOfAnswersFound => AnswerTilesDisplayed.Count;
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
        filterModel.ExcludedNodes.CollectionChanged += OnExcludedNodesChanged;
        AnswerTilesDisplayed.CollectionChanged += OnAnswerTilesDisplayedChanged;
    }

    private void OnExcludedNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (AnswerTileViewModel? tile in answerTileIndex.Values.Where(tile => tile.IsExcluded))
                {
                    AnswerTilesDisplayed.Remove(tile);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is null)
                {
                    break;
                }

                CharacterNode[] newlyIncludedNodes = e.OldItems.Cast<CharacterNode>().ToArray();
                IEnumerable<AnswerTileViewModel> tilesToInclude = answerTileIndex.Values
                    .Where(tile => tile.IsIncluded)
                    .Where(tile => tile.Answer.CharacterNodes.Any(newlyIncludedNodes.Contains));
                foreach (AnswerTileViewModel? tile in tilesToInclude)
                {
                    AnswerTilesDisplayed.Add(tile);
                }
                break;
            default:
                AnswerTilesDisplayed.Clear();
                foreach (AnswerTileViewModel? tile in answerTileIndex.Values.Where(tile => tile.IsIncluded))
                {
                    AnswerTilesDisplayed.Add(tile);
                }
                break;
        }
    }

    private void OnAnswerTilesDisplayedChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(NumberOfAnswersFound));
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

                string[] newDisallowedWords = e.NewItems.Cast<string>().ToArray();
                foreach ((string word, AnswerTileViewModel tile) in answerTileIndex)
                {
                    if (newDisallowedWords.Contains(word))
                    {
                        AnswerTilesDisplayed.Remove(tile);
                    }
                }

                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is null)
                {
                    break;
                }

                string[] newAllowedWords = e.OldItems.Cast<string>().ToArray();
                foreach (string item in newAllowedWords)
                {
                    if (!answerTileIndex.TryGetValue(item, out AnswerTileViewModel? tile) || tile is null)
                    {
                        continue;
                    }

                    if (tile.IsIncluded)
                    {
                        AnswerTilesDisplayed.Add(tile);
                    }
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
            AnswerTileViewModel tile = new(answer, puzzleModel, filterModel);
            answerTileIndex.Add(answer.Word, tile);

            if (tile.IsIncluded)
            {
                AnswerTilesDisplayed.Add(tile);
            }
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
