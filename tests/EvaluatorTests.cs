// <copyright file="EvaluatorTests.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    [Trait("Category", "Evaluator")]
    public class EvaluatorTests
    {
        private readonly IExpressionEvaluator _evaluator = ExpressionRegistry.DefaultEvaluator;

        [Theory(DisplayName = "Evaluator.ConstantNumber → numeric value")]
        [InlineData("123.0", 123.0)]
        [InlineData("0", 0.0)]
        public void Evaluate_ConstantNumber_ReturnsValue(string text, double expected)
        {
            var expr = ExpressionRegistry.DefaultParser.Parse(text);
            var result = _evaluator.Evaluate(expr);

            result.Should().BeOfType<double>()
                  .Which.Should().Be(expected);
        }

        [Theory(DisplayName = "Evaluator.ConstantString → string value")]
        [InlineData("\"hi\"", "hi")]
        [InlineData("\"\"", "")]
        public void Evaluate_ConstantString_ReturnsValue(string text, string expected)
        {
            var expr = ExpressionRegistry.DefaultParser.Parse(text);
            var result = _evaluator.Evaluate(expr);

            result.Should().BeOfType<string>()
                  .Which.Should().Be(expected);
        }

        [Fact(DisplayName = "Evaluator.NullConstant → null")]
        public void Evaluate_NullConstant_ReturnsNull()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("null");
            var result = _evaluator.Evaluate(expr);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Evaluator.Identifier → null stub")]
        public void Evaluate_Identifier_ReturnsNull()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("foo");
            var result = _evaluator.Evaluate(expr);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Evaluator.Lambda → null stub")]
        public void Evaluate_Lambda_ReturnsNull()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("x => x");
            var result = _evaluator.Evaluate(expr);

            result.Should().BeNull();
        }

        [Fact]
        public void StubEvaluator_Resolves_Identifier_From_Dictionary()
        {
            var ctx = new Dictionary<string, object?> { ["x"] = 42 };
            var ok = ExpressionRegistry.DefaultParser.TryParse("x", out var expr, out var diags);
            ok.Should().BeTrue();

            var res = ExpressionRegistry.DefaultEvaluator.Evaluate(expr!, ctx);
            res.Should().Be(42);
        }

        [Fact]
        public void StubEvaluator_Resolves_MemberAccess_From_Dictionary()
        {
            var inner = new Dictionary<string, object?> { ["Prop"] = "value" };
            var ctx = new Dictionary<string, object?> { ["obj"] = inner };
            var ok = ExpressionRegistry.DefaultParser.TryParse("obj.Prop", out var expr, out var diags);
            ok.Should().BeTrue();

            var res = ExpressionRegistry.DefaultEvaluator.Evaluate(expr!, ctx);
            res.Should().Be("value");
        }

        [Fact]
        public void StubEvaluator_Calls_Delegate_From_Context()
        {
            var ctx = new Dictionary<string, object?> { ["Do"] = (System.Func<int, int>)(n => n * 2) };
            var ok = ExpressionRegistry.DefaultParser.TryParse("obj.Do(21)", out var expr, out var diags);
            ok.Should().BeTrue();

            // create a target object with a property 'obj' that resolves to a context lookup
            var dict = new Dictionary<string, object?> { ["obj"] = ctx };

            var res = ExpressionRegistry.DefaultEvaluator.Evaluate(expr!, dict);
            res.Should().Be(42);
        }
    }
}
