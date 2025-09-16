using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    [Trait("Category", "Parser")]
    public class ParserTests : IClassFixture<ParserFixture>
    {
        private readonly IExpressionParser _parser;

        public ParserTests(ParserFixture fixture)
        {
            _parser = fixture.Parser;
        }

        [Fact(DisplayName = "Parser.TryParse(empty) yields diagnostic")]
        public void Parse_EmptyInput_ReturnsDiagnostic()
        {
            var succeeded = _parser.TryParse("", out var expr, out var diagnostics);

            succeeded.Should().BeFalse();
            expr.Should().BeNull();
            diagnostics.Should().ContainSingle(d =>
                d.Position == 0 &&
                d.Message == "Expression is empty or whitespace.");
        }

        [Theory(DisplayName = "Parser.TryParse(number) → ConstantExpression")]
        [InlineData("123.45", ExpressionKind.Constant, "123.45")]
        [InlineData("0", ExpressionKind.Constant, "0")]
        public void Parse_ConstantNumber_ReturnsConstantExpression(
            string input,
            ExpressionKind expectedKind,
            string expectedCanonical)
        {
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(expectedKind);
            expr.Canonical.Should().Be(expectedCanonical);
        }

        [Theory(DisplayName = "Parser.TryParse(string) → ConstantExpression")]
        [InlineData("\"abc\"", ExpressionKind.Constant, "\"abc\"")]
        [InlineData("\"\"", ExpressionKind.Constant, "\"\"")]
        public void Parse_ConstantString_ReturnsConstantExpression(
            string input,
            ExpressionKind expectedKind,
            string expectedCanonical)
        {
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(expectedKind);
            expr.Canonical.Should().Be(expectedCanonical);
        }

        [Theory(DisplayName = "Parser.TryParse(identifier) → IdentifierExpression")]
        [InlineData("foo", ExpressionKind.Identifier, "foo")]
        [InlineData("_x", ExpressionKind.Identifier, "_x")]
        public void Parse_Identifier_ReturnsIdentifierExpression(
            string input,
            ExpressionKind expectedKind,
            string expectedCanonical)
        {
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(expectedKind);
            expr.Canonical.Should().Be(expectedCanonical);
        }

        [Fact(DisplayName = "Parser.TryParse(member access) → MemberAccessExpression")]
        public void Parse_MemberAccess_ReturnsMemberAccessExpression()
        {
            const string input = "obj.Prop";
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(ExpressionKind.MemberAccess);
            expr.Canonical.Should().Be(input);
            expr.Operands.Should().ContainSingle()
                .Which.Canonical.Should().Be("obj");
        }

        [Fact(DisplayName = "Parser.TryParse(method call) → MethodCallExpression")]
        public void Parse_MethodCall_ReturnsMethodCallExpression()
        {
            const string input = "svc.Do(42, \"hi\")";
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(ExpressionKind.MethodCall);
            expr.Canonical.Should().Be(input);
            expr.Operands.Should().HaveCount(3);
            expr.Operands[0].Canonical.Should().Be("svc");
            expr.Operands[1].Canonical.Should().Be("42");
            expr.Operands[2].Canonical.Should().Be("\"hi\"");
        }

        [Fact(DisplayName = "Parser.TryParse(lambda) → LambdaExpression")]
        public void Parse_Lambda_ReturnsLambdaExpression()
        {
            const string input = "x => x.Property";
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeTrue();
            diagnostics.Should().BeEmpty();
            expr.Should().NotBeNull();
            expr!.Kind.Should().Be(ExpressionKind.Lambda);
            expr.Canonical.Should().Be(input);

            var body = expr.Operands.Single();
            body.Kind.Should().Be(ExpressionKind.MemberAccess);
            body.Canonical.Should().Be("x.Property");
        }

        [Fact(DisplayName = "Parser.TryParse(invalid) yields diagnostic")]
        public void Parse_InvalidToken_ReturnsDiagnostic()
        {
            const string input = "123 xyz";
            var succeeded = _parser.TryParse(input, out var expr, out var diagnostics);

            succeeded.Should().BeFalse();
            expr.Should().NotBeNull();
            diagnostics.Should().ContainSingle(d =>
                d.Message.Contains("Unexpected token 'xyz'"));
        }
    }
}
