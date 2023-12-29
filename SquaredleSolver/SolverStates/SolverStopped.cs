using GraphWalking;
using GraphWalking.Graphs;

namespace SquaredleSolver.SolverStates;

/// <summary>
///     Defines how the puzzle solver behaves when it is stopped.
/// </summary>
public class SolverStopped : ISolverState
{
    public static ISolverState Instance { get; } = new SolverStopped();

    private SolverStopped() { }

    public void OnPuzzleModelChanged(SolverContext context) { }

    public void StartSolution(SolverContext context)
    {
        if (context.PuzzleModel.PuzzleAsText == string.Empty)
        {
            return;
        }

        context.CancellationTokenSource = new CancellationTokenSource();
        context.AnswersFoundInPuzzle.Clear();
        context.CurrentState = SolverRunning.Instance;
        context.SolverTask = Task.Run(() => SolvePuzzle(context));
    }

    public void StopSolution(SolverContext context) { }

    public void OnSolverCompleted(SolverContext context)
    {
        throw new InvalidOperationException();
    }

    private static void SolvePuzzle(SolverContext context)
    {
        CancellationToken token = context.CancellationTokenSource.Token;
        PuzzleModel puzzleModel = context.PuzzleModel;
        if (token.IsCancellationRequested)
        {
            return;
        }

        IEnumerable<List<CharacterNode>> allPaths =
            PathGenerator<CharacterNode>.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList);
        HashSet<string> wordsAlreadyFound = new();
        foreach (List<CharacterNode> path in allPaths)
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
                context.AnswersFoundInPuzzle.Add(answer);
            }
        }

        context.CurrentState.OnSolverCompleted(context);
    }
}
