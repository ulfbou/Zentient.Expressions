// <copyright file="ExtensionsTests.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;
using Xunit;
using FluentAssertions;
using Zentient.Extensions.Expressions;

namespace Zentient.Expressions.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void ToCanonicalString_Returns_Canonical()
        {
            var ok = ExpressionRegistry.DefaultParser.TryParse("\"a\\\"b\"", out var expr, out var diags);
            ok.Should().BeTrue();
            expr!.ToCanonicalString().Should().Be(expr!.Canonical);
        }

        [Fact]
        public void EvaluateExpression_String_Evaluates()
        {
            var res = "\"hi\"".EvaluateExpression();
            res.Should().Be("hi");
        }
    }
}
