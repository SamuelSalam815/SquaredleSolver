using GraphWalking.Graphs;
using SquaredleSolverModel;
using SquaredleSolverModel.Solver;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SquaredleSolver.ViewModel;
internal class FilterViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly PuzzleModel puzzle;
    private readonly SolverModel solver;

    private readonly Dictionary<CharacterNode, FilterNodeViewModel> _filterNodes = new();
    public readonly ReadOnlyDictionary<CharacterNode, FilterNodeViewModel> FilterNodes;

    /// <summary>
    ///     Contains answers that were blocked by this filter.
    /// </summary>
    private readonly List<AnswerModel> excludedAnswers = new();

    /// <summary>
    ///     Contains answers that passed this filter.
    /// </summary>
    public readonly ObservableCollection<AnswerModel> IncludedAnswers = new();

    /// <summary>
    ///     Contains answers that have already been attempted by the user.
    /// </summary>
    public readonly ObservableCollection<string> AttemptedWords;

    public FilterViewModel(PuzzleModel puzzle, SolverModel solver)
    {
        this.puzzle = puzzle;
        this.solver = solver;
        FilterNodes = new ReadOnlyDictionary<CharacterNode, FilterNodeViewModel>(_filterNodes);
        AttemptedWords = new ObservableCollection<string>();

        RepopulateFilterNodes();
        puzzle.PropertyChanged += OnPuzzleChanged;
        solver.Answers.CollectionChanged += OnAnswersChanged;
        AttemptedWords.CollectionChanged += OnAttemptedWordsChanged;
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        AttemptedWords.Clear();
        IncludedAnswers.Clear();
        excludedAnswers.Clear();
        RepopulateFilterNodes();
    }

    private void OnAnswersChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is null)
                {
                    break;
                }
                foreach (AnswerModel item in e.NewItems.Cast<AnswerModel>())
                {
                    if (IsAnswerIncluded(item))
                    {
                        IncludedAnswers.Add(item);
                    }
                    else
                    {
                        excludedAnswers.Add(item);
                    }
                }
                break;
            default:
                RefilterAllAnswers();
                break;
        }
    }

    private void OnAttemptedWordsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                RefilterIncludedAnswers();
                break;
            case NotifyCollectionChangedAction.Remove:
                RefilterExcludedAnswers();
                break;
            default:
                RefilterAllAnswers();
                break;
        }
    }

    private void OnFilterNodeChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not FilterNodeViewModel filterNode)
        {
            return;
        }

        if (filterNode.IsIncluded)
        {
            RefilterExcludedAnswers();
        }
        else
        {
            RefilterIncludedAnswers();
        }
    }

    private void RepopulateFilterNodes()
    {
        _filterNodes.Clear();
        foreach (CharacterNode node in puzzle.PuzzleAsNodes)
        {
            FilterNodeViewModel filterNode = new(node);
            filterNode.PropertyChanged += OnFilterNodeChanged;
            _filterNodes.Add(node, filterNode);
        }
        OnPropertyChanged(nameof(FilterNodes));
    }

    private void RefilterAllAnswers()
    {
        excludedAnswers.Clear();
        IncludedAnswers.Clear();
        foreach (AnswerModel item in solver.Answers)
        {
            if (IsAnswerIncluded(item))
            {
                IncludedAnswers.Add(item);
            }
            else
            {
                excludedAnswers.Add(item);
            }
        }
    }

    private void RefilterExcludedAnswers()
    {
        List<AnswerModel> affectedAnswers = new();
        foreach (AnswerModel answer in excludedAnswers)
        {
            if (IsAnswerIncluded(answer))
            {
                affectedAnswers.Add(answer);
            }
        }

        foreach (AnswerModel answer in affectedAnswers)
        {
            excludedAnswers.Remove(answer);
            IncludedAnswers.Add(answer);
        }
    }

    private void RefilterIncludedAnswers()
    {
        List<AnswerModel> affectedAnswers = new();
        foreach (AnswerModel answer in IncludedAnswers)
        {
            if (!IsAnswerIncluded(answer))
            {
                affectedAnswers.Add(answer);
            }
        }

        foreach (AnswerModel answer in affectedAnswers)
        {
            IncludedAnswers.Remove(answer);
            excludedAnswers.Add(answer);
        }
    }

    private bool IsAnswerIncluded(AnswerModel answer)
    {
        if (AttemptedWords.Contains(answer.Word))
        {
            return false;
        }

        if (answer.CharacterNodes.Any(node => !FilterNodes[node].IsIncluded))
        {
            return false;
        }

        return true;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
