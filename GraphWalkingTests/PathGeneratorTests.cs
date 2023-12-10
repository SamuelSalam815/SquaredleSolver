using FluentAssertions;
using GraphWalking;
using GraphWalking.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class PathGeneratorTests
{
    [TestMethod]
    public void When_GeneratingAllPaths_Then_CorrectPathsAreReturned()
    {
        AdjacencyList<int> graph = new()
        {
            [1] = new() { 2, 3, 4 },
            [2] = new() { 4 },
            [3] = new() { 4 },
        };
        PathGenerator<int> systemUnderTest = new(graph);

        IEnumerable<List<int>> paths = systemUnderTest.Generate();

        paths.Should().BeEquivalentTo(new List<List<int>>()
        {
            new() { 1 },
            new() { 1, 2 },
            new() { 1, 2, 4 },
            new() { 1, 3 },
            new() { 1, 3, 4 },
            new() { 1, 4 },
            new() { 2 },
            new() { 2, 4 },
            new() { 3 },
            new() { 3, 4 },
            new() { 4 },
        });

        IEnumerable<List<int>> alternatePath = systemUnderTest.FastGenerate();
        alternatePath.Should().BeEquivalentTo(paths);

        alternatePath = systemUnderTest.RecursiveGenerate();
        alternatePath.Should().BeEquivalentTo(paths);
    }

}