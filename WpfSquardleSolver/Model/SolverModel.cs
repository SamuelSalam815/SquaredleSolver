using GraphWalking;
using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSquardleSolver.Model;
class SolverModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsSolverRunning { get; private set; }
    public BindingList<string> ValidWordsFoundInPuzzle;

    private readonly PuzzleModel puzzleModel;
    private AdjacencyList<CharacterNode> puzzleAsAdjacencyList;
    private CancellationTokenSource cancellationTokenSource;
    private Task puzzleSolvingBackgroundTask;

    public SolverModel(PuzzleModel puzzle)
    {
        puzzleModel = puzzle;

        cancellationTokenSource = new CancellationTokenSource();
        puzzleSolvingBackgroundTask = Task.CompletedTask;
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        StopSolvingPuzzle();
        if (args.PropertyName == nameof(puzzleModel.PuzzleAsText))
        {
            puzzleAsAdjacencyList = CharacterGraphBuilder.FromLetterGrid(puzzleModel.PuzzleAsText);
        }
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
        IsSolverRunning = true;
        OnPropertyChanged(nameof(IsSolverRunning));
    }

    public void StopSolvingPuzzle()
    {
        cancellationTokenSource.Cancel();
        lock (this)
        {
            puzzleSolvingBackgroundTask.Wait();
        }

        IsSolverRunning = false;
        OnPropertyChanged(nameof(IsSolverRunning));
    }

    private void SolvePuzzle(CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        StringBuilder stringBuilder = new();
        foreach (List<CharacterNode> path in PathGenerator<CharacterNode>.EnumerateAllPaths(puzzleAsAdjacencyList))
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
            if (puzzleModel.ValidWords.Contains(word))
            {
                Application.Current.Dispatcher.Invoke(() => ValidWordsFoundInPuzzle.Add(word));
            }
        }

        IsSolverRunning = false;
        OnPropertyChanged(nameof(IsSolverRunning));
    }

    private void OnPropertyChanged(string nameOfProperty)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
    }
}
