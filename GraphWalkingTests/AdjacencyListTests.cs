using FluentAssertions;
using GraphWalking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class AdjacencyListTests
{
    [TestMethod]
    public void When_NodeHasNoAdjacencies_Then_AllNodesStillIncludesThatNode()
    {
        AdjacencyList<int> systemUnderTest = new()
        {
            [1] = new() { 3 },
            [2] = new() { 3 },
        };

        systemUnderTest.AllNodes.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
    }
}