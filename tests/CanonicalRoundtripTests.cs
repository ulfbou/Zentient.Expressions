using Xunit;
using FluentAssertions;
using Zentient.Expressions;
using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    [Trait("Category", "CanonicalRoundtrip")]
    public class CanonicalRoundtripTests
    {
        private readonly IExpressionParser _parser = ExpressionRegistry.DefaultParser;

        [Theory(DisplayName = "Round-trip constant numbers")]
        [InlineData("0")]
        [InlineData("123.45")]
        public void ParseAndCanonicalize_RoundtripsForConstants(string input)
        {
            var ok = _parser.TryParse(input, out var expr, out var diags);
            ok.Should().BeTrue();
            diags.Should().BeEmpty();
            expr!.Canonical.Should().Be(input);
        }

        [Theory(DisplayName = "Round-trip identifiers")]
        [InlineData("foo")]
        [InlineData("_id")]
        public void ParseAndCanonicalize_RoundtripsForIdentifiers(string input)
        {
            var ok = _parser.TryParse(input, out var expr, out var diags);
            ok.Should().BeTrue();
            diags.Should().BeEmpty();
            expr!.Canonical.Should().Be(input);
        }

        [Fact(DisplayName = "Round-trip member access")]
        public void ParseAndCanonicalize_RoundtripsForMemberAccess()
        {
            const string input = "obj.Prop";
            _parser.TryParse(input, out var expr, out var diags).Should().BeTrue();
            diags.Should().BeEmpty();
            expr!.Canonical.Should().Be(input);
        }

        [Fact(DisplayName = "Round-trip method calls")]
        public void ParseAndCanonicalize_RoundtripsForMethodCall()
        {
            const string input = "svc.Do(5, \"x\")";
            _parser.TryParse(input, out var expr, out var diags).Should().BeTrue();
            diags.Should().BeEmpty();
            expr!.Canonical.Should().Be(input);
        }

        [Fact(DisplayName = "Round-trip lambdas")]
        public void ParseAndCanonicalize_RoundtripsForLambdas()
        {
            const string input = "a => a.Method()";
            _parser.TryParse(input, out var expr, out var diags).Should().BeTrue();
            diags.Should().BeEmpty();
            expr!.Canonical.Should().Be(input);
        }
    }
}
