using FluentAssertions;
using GraphWalking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphWalkingTests;

[TestClass]
public class PathTests
{
    [TestMethod]
    public void When_AddingNodeAlreadyVisited_Then_ExceptionIsThrown()
    {
        Path<int> systemUnderTest = new()
        {
            0, 1, 2, 3,
        };

        systemUnderTest.Invoking(x => x.Add(0)).Should().Throw<InvalidOperationException>();
    }
}
