using FluentAssertions;
using GraphWalking.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class CharacterGraphBuilderTests
{
    [TestMethod]
    public void When_ParsingInvalidGrid_Then_ExceptionIsThrown()
    {
        string input = """
            abc
            de
            fgh
            """;

        Assert.ThrowsException<ArgumentException>(() => CharacterGraphBuilder.FromLetterGrid(input));
    }

    [TestMethod]
    public void When_AGridOfCharactersAreParsed_Then_AdjacentCharactersAreCorrect()
    {
        string inputGrid = """
            a1a
            2*3
            a4a
            """;

        AdjacencyList<CharacterNode> result = CharacterGraphBuilder.FromLetterGrid(inputGrid);

        result.GetAdjacentCharacters('a').Should().BeEquivalentTo(new List<char> { '1', '2', '3', '4', '*' });
        result.GetAdjacentCharacters('1').Should().BeEquivalentTo(new List<char> { '2', '3', 'a', '*' });
        result.GetAdjacentCharacters('2').Should().BeEquivalentTo(new List<char> { '1', '4', 'a', '*' });
        result.GetAdjacentCharacters('3').Should().BeEquivalentTo(new List<char> { '1', '4', 'a', '*' });
        result.GetAdjacentCharacters('4').Should().BeEquivalentTo(new List<char> { '2', '3', 'a', '*' });
    }
}