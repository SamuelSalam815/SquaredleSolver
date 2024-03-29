﻿using GraphWalking;
using GraphWalking.Graphs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolverModel.Solver;

/// <summary>
///     Represents an object used to solve a puzzle defined by a <see cref="PuzzleModel"/>
/// </summary>
public class SolverModel
{
    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    private readonly PuzzleModel puzzle;
    private CancellationTokenSource cancellationTokenSource;
    private Task solverTask;
    private FailFastPathGenerator pathGenerator;
    public ObservableCollection<AnswerModel> Answers { get; } = new();

    private SolverState _state;
    public SolverState State
    {
        get => _state;
        set
        {
            SolverState previousState = _state;
            _state = value;

            StateChanged?.Invoke(this, new SolverStateChangedEventArgs(previousState, _state));
        }
    }


    public SolverModel(PuzzleModel puzzle)
    {
        _state = SolverState.Stopped;
        this.puzzle = puzzle;
        cancellationTokenSource = new CancellationTokenSource();
        solverTask = Task.CompletedTask;
        pathGenerator = new FailFastPathGenerator(puzzle.ValidWords, PuzzleModel.MinimumWordLength);

        puzzle.PropertyChanged += OnPuzzleModelChanged;
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        StopSolvingPuzzle();
        if (args.PropertyName == nameof(puzzle.ValidWords))
        {
            pathGenerator = new FailFastPathGenerator(puzzle.ValidWords, PuzzleModel.MinimumWordLength);
        }
        Answers.Clear();
        State = SolverState.Stopped;
    }

    public void StartSolvingPuzzle()
    {
        switch (State)
        {
            case SolverState.Stopped:
            case SolverState.Completed:
                cancellationTokenSource = new CancellationTokenSource();
                State = SolverState.Running;
                solverTask = Task.Run(() => SolvePuzzle());
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public void StopSolvingPuzzle()
    {
        switch (State)
        {
            case SolverState.Running:
                cancellationTokenSource.Cancel();
                solverTask.Wait();
                State = SolverState.Stopped;
                break;
            default:
                break;
        }
    }

    private void SolvePuzzle()
    {
        CancellationToken token = cancellationTokenSource.Token;
        if (token.IsCancellationRequested)
        {
            return;
        }

        IEnumerable<List<CharacterNode>> allPaths =
            pathGenerator.EnumerateAllPaths(puzzle.PuzzleAsAdjacencyList);
        HashSet<string> wordsAlreadyFound = new();
        foreach (List<CharacterNode> path in allPaths)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            AnswerModel answer = new(path, wordsAlreadyFound.Count);

            if (!puzzle.ValidWords.Contains(answer.Word))
            {
                continue;
            }

            if (!wordsAlreadyFound.Contains(answer.Word))
            {
                wordsAlreadyFound.Add(answer.Word);
                Answers.Add(answer);
            }
        }

        State = SolverState.Completed;
    }
}
