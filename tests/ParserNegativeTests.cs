using Xunit;
using FluentAssertions;
using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    public class ParserNegativeTests
    {
        [Fact]
        public void TryParse_UnterminatedString_YieldsDiagnostic()
        {
            var ok = ExpressionRegistry.DefaultParser.TryParse("\"unclosed", out var expr, out var diags);
            ok.Should().BeFalse();
            diags.Should().ContainSingle(d => d.Message.Contains("Unterminated string literal"));
            expr.Should().NotBeNull();
        }

        [Fact]
        public void TryParse_InvalidNumber_YieldsDiagnostic()
        {
            var ok = ExpressionRegistry.DefaultParser.TryParse("123.45.67", out var expr, out var diags);
            ok.Should().BeFalse();
            diags.Should().Contain(d => d.Message.Contains("Invalid number"));
            expr.Should().NotBeNull();
        }
    }
}
