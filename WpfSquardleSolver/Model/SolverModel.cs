using GraphWalking;
using GraphWalking.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSquaredleSolver.Model;
class SolverModel
{
    public event Action? SolverStarted;
    public event Action? SolverStopped;
    public BindingList<AnswerModel> AnswersFoundInPuzzle;

    private readonly PuzzleModel puzzleModel;
    private CancellationTokenSource cancellationTokenSource;
    private Task puzzleSolvingBackgroundTask;

    public SolverModel(PuzzleModel puzzleModel)
    {
        AnswersFoundInPuzzle = new BindingList<AnswerModel>();
        this.puzzleModel = puzzleModel;
        cancellationTokenSource = new CancellationTokenSource();
        puzzleSolvingBackgroundTask = Task.CompletedTask;

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        StopSolvingPuzzle();
    }

    public void StartSolvingPuzzle()
    {
        StopSolvingPuzzle();
        if (puzzleModel.PuzzleAsText.Equals(string.Empty))
        {
            return;
        }

        cancellationTokenSource = new CancellationTokenSource();
        AnswersFoundInPuzzle.Clear();
        puzzleSolvingBackgroundTask = Task.Run(() => SolvePuzzle(cancellationTokenSource.Token));
        SolverStarted?.Invoke();
    }

    public void StopSolvingPuzzle()
    {
        cancellationTokenSource.Cancel();
        lock (this)
        {
            puzzleSolvingBackgroundTask.Wait();
        }

        SolverStopped?.Invoke();
    }

    private void SolvePuzzle(CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        HashSet<string> wordsAlreadyFound = new();
        foreach (List<CharacterNode> path in PathGenerator<CharacterNode>.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            if (path.Count <= 3)
            {
                continue;
            }

            AnswerModel answer = new(path);
            if (!wordsAlreadyFound.Contains(answer.Word) && puzzleModel.ValidWords.Contains(answer.Word))
            {
                wordsAlreadyFound.Add(answer.Word);
                Application.Current.Dispatcher.Invoke(() => AnswersFoundInPuzzle.Add(answer));
            }
        }

        SolverStopped?.Invoke();
    }
}
