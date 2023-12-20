using GraphWalking;
using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSquardleSolver.Model;
class SolverModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool IsSolverRunning { get; private set; }

    private readonly PuzzleModel puzzleModel;
    private readonly SolverModel model;
    private CancellationTokenSource cancellationTokenSource;
    private Task puzzleSolvingBackgroundTask;

    public SolverModel(PuzzleModel puzzle)
    {
        puzzleModel = puzzle;
        model = new SolverModel();

        cancellationTokenSource = new CancellationTokenSource();
        puzzleSolvingBackgroundTask = Task.CompletedTask;
    }

    public void UpdatePuzzle(string puzzleAsText)
    {
        lock (this)
        {
            PuzzleAsCharacterGraph = CharacterGraphBuilder.FromLetterGrid(puzzleAsText);
        }
    }

    public void StartSolvingPuzzle()
    {
        StopSolvingPuzzle();
        if (PuzzleAsCharacterGraph is null)
        {
            return;
        }

        cancellationTokenSource = new CancellationTokenSource();
        Answers.Clear();
        puzzleSolvingBackgroundTask = Task.Run(() => SolvePuzzle(cancellationTokenSource.Token));
        IsSolverRunning = true;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSolverRunning)));
    }

    public void StopSolvingPuzzle()
    {
        cancellationTokenSource.Cancel();
        lock (this)
        {
            puzzleSolvingBackgroundTask.Wait();
        }

        IsSolverRunning = false;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSolverRunning)));
    }

    private void SolvePuzzle(CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        StringBuilder stringBuilder = new();
        foreach (List<CharacterNode> path in PathGenerator<CharacterNode>.EnumerateAllPaths(PuzzleAsCharacterGraph))
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
            if (puzzleModel.Contains(word))
            {
                Application.Current.Dispatcher.Invoke(() => Answers.Add(word));
            }
        }

        IsSolverRunning = false;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSolverRunning)));
    }

}
