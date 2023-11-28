using FluentAssertions;
using GraphWalking.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class CharacterGraphTests
{
    [TestMethod]
    public void When_ParsingInvalidGrid_Then_ExceptionIsThrown()
    {
        string input = """
            abc
            de
            fgh
            """;

        Assert.ThrowsException<ArgumentException>(() => CharacterGraph.FromLetterGrid(input));
    }

    [TestMethod]
    public void When_AGridOfCharactersAreParsed_Then_AdjacentCharactersAreCorrect()
    {
        string inputGrid = """
            a1a
            2*3
            a4a
            """;

        CharacterGraph systemUnderTest = CharacterGraph.FromLetterGrid(inputGrid);

        systemUnderTest.GetAdjacentCharacters('a').Should().BeEquivalentTo(new List<char> { '1', '2', '3', '4', '*' });
        systemUnderTest.GetAdjacentCharacters('1').Should().BeEquivalentTo(new List<char> { '2', '3', 'a', '*' });
        systemUnderTest.GetAdjacentCharacters('2').Should().BeEquivalentTo(new List<char> { '1', '4', 'a', '*' });
        systemUnderTest.GetAdjacentCharacters('3').Should().BeEquivalentTo(new List<char> { '1', '4', 'a', '*' });
        systemUnderTest.GetAdjacentCharacters('4').Should().BeEquivalentTo(new List<char> { '2', '3', 'a', '*' });
    }
}