namespace Zentient.Expressions.Tests
{
    [Trait("Category", "DebugExtensions")]
    public class DebugExtensionsTests
    {
        [Fact(DisplayName = "ToCanonicalString returns Canonical")]
        public void ToCanonicalString_MatchesExpressionCanonical()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("foo.Bar(1)");
            expr.ToCanonicalString().Should().Be(expr.Canonical);
        }

        [Fact(DisplayName = "ToCanonicalString on nested expression")]
        public void ToCanonicalString_OnNestedExpression_MatchesCanonical()
        {
            var lambda = ExpressionRegistry.DefaultParser.Parse("x => x.Y");
            var nested = lambda.Operands[0]; // MemberAccess
            nested.ToCanonicalString().Should().Be(nested.Canonical);
        }
    }
}
