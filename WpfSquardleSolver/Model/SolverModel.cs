using GraphWalking;
using GraphWalking.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSquardleSolver.Model;
class SolverModel
{
    public event Action? SolverStarted;
    public event Action? SolverStopped;
    public BindingList<string> ValidWordsFoundInPuzzle;

    private readonly PuzzleModel puzzleModel;
    private CancellationTokenSource cancellationTokenSource;
    private Task puzzleSolvingBackgroundTask;

    public SolverModel(PuzzleModel puzzleModel)
    {
        ValidWordsFoundInPuzzle = new BindingList<string>();
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
        ValidWordsFoundInPuzzle.Clear();
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

        StringBuilder stringBuilder = new();
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

            stringBuilder.Clear();
            foreach (CharacterNode node in path)
            {
                stringBuilder.Append(node.Character);
            }

            string word = stringBuilder.ToString();

            if (!wordsAlreadyFound.Contains(word) && puzzleModel.ValidWords.Contains(word))
            {
                wordsAlreadyFound.Add(word);
                Application.Current.Dispatcher.Invoke(() => ValidWordsFoundInPuzzle.Add(word));
            }
        }

        SolverStopped?.Invoke();
    }
}
