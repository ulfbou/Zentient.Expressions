using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    public class ParserFixture
    {
        public IExpressionParser Parser { get; } = ExpressionRegistry.DefaultParser;
    }
}
