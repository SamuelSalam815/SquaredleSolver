using SquaredleSolver.Command;
using SquaredleSolverModel;
using SquaredleSolverModel.Solver;
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

    private readonly SolverModel solver;
    private readonly PuzzleModel puzzle;
    private readonly FilterViewModel filter;

    public ICommand FocusPuzzleInput { get; }
    public ICommand ToggleSolverOnOff { get; }

    public ICommand ClearAttemptedWords { get; }

    public ObservableCollection<AnswerTileViewModel> AnswerTilesDisplayed { get; }
    public FilterGridViewModel NodeFilterGridViewModel { get; }
    public ObservableCollection<string> AttemptedWords => filter.AttemptedWords;
    public record struct AnswerCounterStruct(int NumberFound, int NumberDisplayed);
    public AnswerCounterStruct AnswerCounter => new(solver.Answers.Count, AnswerTilesDisplayed.Count);
    public SolverState SolverState => solver.State;

    public string PuzzleAsText
    {
        get => puzzle.PuzzleAsText;
        set
        {
            puzzle.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    public MainWindowViewModel(
        PuzzleModel puzzle,
        SolverModel solver,
        FilterViewModel filter,
        ICommand focusPuzzleInput)
    {
        this.solver = solver;
        this.puzzle = puzzle;
        this.filter = filter;

        FocusPuzzleInput = focusPuzzleInput;
        ToggleSolverOnOff = new DelegateCommand(() =>
            {
                if (solver.State is SolverState.Running)
                {
                    solver.StopSolvingPuzzle();
                }
                else
                {
                    solver.StartSolvingPuzzle();
                }
            },
            () => solver.State is not SolverState.Completed);
        ClearAttemptedWords = new DelegateCommand(ExecuteClearAttemptedWords);
        AnswerTilesDisplayed = new ObservableCollection<AnswerTileViewModel>();
        NodeFilterGridViewModel = new FilterGridViewModel(filter, puzzle);

        puzzle.PropertyChanged += OnPuzzleModelChanged;
        solver.StateChanged += OnSolverStateChanged;
        filter.IncludedAnswers.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnFilteredAnswersChanged(sender, e));
        AnswerTilesDisplayed.CollectionChanged += OnAnswerTilesDisplayedChanged;
    }

    private void ExecuteClearAttemptedWords()
    {
        AttemptedWords.Clear();
    }

    private void OnAnswerTilesDisplayedChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(AnswerCounter));
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));
    }
    private void OnFilteredAnswersChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is null)
                {
                    break;
                }

                foreach (AnswerModel answer in e.NewItems.Cast<AnswerModel>())
                {
                    AnswerTileViewModel tile = new(answer, puzzle, filter);
                    AnswerTilesDisplayed.Add(tile);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is null)
                {
                    break;
                }

                foreach (AnswerModel answer in e.OldItems.Cast<AnswerModel>())
                {
                    List<AnswerTileViewModel> tilesToRemove = new();
                    foreach (AnswerTileViewModel tile in AnswerTilesDisplayed)
                    {
                        if (tile.Answer == answer)
                        {
                            tilesToRemove.Add(tile);
                        }
                    }
                    foreach (AnswerTileViewModel tile in tilesToRemove)
                    {
                        AnswerTilesDisplayed.Remove(tile);
                    }
                }
                break;
            default:
                AnswerTilesDisplayed.Clear();
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
        AnswerTilesDisplayed.Clear();
    }
}
