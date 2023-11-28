using FluentAssertions;
using GraphWalking.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class AdjacencyListTests
{
    [TestMethod]
    public void When_NodeHasNoAdjacencies_Then_ItIsALeafNode()
    {
        AdjacencyList<int> systemUnderTest = new()
        {
            [1] = new() { 3 },
            [2] = new() { 3 },
        };

        systemUnderTest.GetLeafNodes().Should().BeEquivalentTo(new List<int> { 3 });
    }

    [TestMethod]
    public void When_NodeHasEmptyAdjacencyLIst_Then_ItIsNotABranchNode()
    {
        AdjacencyList<int> systemUnderTest = new()
        {
            [1] = new() { 3 },
            [2] = new() { 3 },
            [100] = new() { }
        };

        systemUnderTest.GetBranchNodes().Should().BeEquivalentTo(new List<int> { 1, 2 });
    }
}