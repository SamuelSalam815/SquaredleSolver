namespace GraphWalking.Graphs;

public static class AdjacencyListCharacterNodeExtensions
{
    public static IEnumerable<char> GetAdjacentCharacters(this AdjacencyList<CharacterNode> adjacencyList, char character)
    {
        IEnumerable<CharacterNode> matchingNodes = adjacencyList.GetAllNodes().Where(node => node.Character == character);
        IEnumerable<CharacterNode> adjacentNodes = matchingNodes.SelectMany(node => adjacencyList[node]);
        HashSet<char> adjacentCharacters = adjacentNodes.Select(node => node.Character).ToHashSet();
        return adjacentCharacters.ToList();
    }
}