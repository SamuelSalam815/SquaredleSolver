using System.Collections;

namespace GraphWalking;

public class Path<TNodeId> : IEnumerable<TNodeId>
{
    private List<TNodeId> nodes;

    public Path()
    {
        nodes = new List<TNodeId>();
    }

    public Path(Path<TNodeId> path)
    {
        nodes = new List<TNodeId>(path.nodes);
    }

    public void Add(TNodeId node)
    {
        if (nodes.Contains(node))
        {
            throw new InvalidOperationException("Attempted to revisit a node when building a path.");
        }

        nodes.Add(node);
    }

    public IEnumerator<TNodeId> GetEnumerator() => nodes.GetEnumerator();

    public override string ToString() => "[" + string.Join(", ", nodes) + "]";

    IEnumerator IEnumerable.GetEnumerator() => nodes.GetEnumerator();
}
