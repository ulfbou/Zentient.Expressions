using Xunit;

namespace Zentient.Expressions.Tests;

public class ExpressionParserTests
{
    [Fact]
    public void Version_ReturnsExpectedVersion()
    {
        // Arrange & Act
        var version = ExpressionParser.Version;

        // Assert
        Assert.NotNull(version);
        Assert.NotEmpty(version);
    }
}