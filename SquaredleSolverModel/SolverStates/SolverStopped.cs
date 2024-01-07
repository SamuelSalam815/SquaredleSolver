using GraphWalking.Graphs;

namespace SquaredleSolverModel.SolverStates;

/// <summary>
///     Defines how the puzzle solver behaves when it is stopped.
/// </summary>
public class SolverStopped : ISolverState
{
    public static ISolverState Instance { get; } = new SolverStopped();

    private SolverStopped() { }

    public void OnPuzzleChanged(SolverContext context) { }

    public void StartSolution(SolverContext context)
    {
        if (context.PuzzleModel.PuzzleAsText == string.Empty)
        {
            return;
        }

        context.CancellationTokenSource = new CancellationTokenSource();
        context.AnswersFound.Clear();
        context.CurrentState = SolverRunning.Instance;
        context.SolverTask = Task.Run(() => SolvePuzzle(context));
    }

    public void StopSolution(SolverContext context) { }

    public void OnSolverCompleted(SolverContext context) => throw new InvalidOperationException();

    private static void SolvePuzzle(SolverContext context)
    {
        CancellationToken token = context.CancellationTokenSource.Token;
        PuzzleModel puzzleModel = context.PuzzleModel;
        if (token.IsCancellationRequested)
        {
            return;
        }

        IEnumerable<List<CharacterNode>> allPaths =
            context.PathGenerator.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList);
        HashSet<string> wordsAlreadyFound = new();
        foreach (List<CharacterNode> path in allPaths)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            AnswerModel answer = new(path);
            if (!wordsAlreadyFound.Contains(answer.Word))
            {
                wordsAlreadyFound.Add(answer.Word);
                context.AnswersFound.Add(answer);
            }
        }

        context.CurrentState.OnSolverCompleted(context);
    }
}
