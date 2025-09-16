// <copyright file="RegistryTests.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using System;
using Zentient.Abstractions.Expressions;
using Xunit;
using FluentAssertions;

namespace Zentient.Expressions.Tests
{
    public class RegistryTests
    {
        [Fact(DisplayName = "DefaultParser.Parse works")]
        public void DefaultParser_Parse_Works()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("42");
            expr.Should().NotBeNull();
            expr.Kind.Should().Be(ExpressionKind.Constant);
        }

        [Fact(DisplayName = "DefaultEvaluator.Evaluate works")]
        public void DefaultEvaluator_Evaluate_Works()
        {
            var expr = ExpressionRegistry.DefaultParser.Parse("\"a\"");
            var result = ExpressionRegistry.DefaultEvaluator.Evaluate(expr);

            result.Should().Be("a");
        }

        [Fact(DisplayName = "OnParsed fires when parsing succeeds")]
        public void OnParsed_FiresWhenExpressionParsed()
        {
            IExpression? captured = null;
            void Handler(IExpression e) => captured = e;

            ExpressionRegistry.OnParsed += Handler;
            try
            {
                ExpressionRegistry.DefaultParser.Parse("100");
                captured.Should().NotBeNull();
                captured!.Canonical.Should().Be("100");
            }
            finally
            {
                ExpressionRegistry.OnParsed -= Handler;
            }
        }

        [Fact(DisplayName = "OnEvaluated fires with result when evaluating")]
        public void OnEvaluated_FiresWhenExpressionEvaluated()
        {
            object? captured = new object();
            IExpression? expr = ExpressionRegistry.DefaultParser.Parse("7");
            void Handler(IExpression e, object? value) => captured = value;

            ExpressionRegistry.OnEvaluated += Handler;
            try
            {
                var result = ExpressionRegistry.DefaultEvaluator.Evaluate(expr);
                captured.Should().Be(result);
            }
            finally
            {
                ExpressionRegistry.OnEvaluated -= Handler;
            }
        }

        [Fact(DisplayName = "Swapping DefaultParser is thread-safe")]
        public void SwappingDefaultParser_IsThreadSafe()
        {
            Action act = () => Parallel.For(0, 1000, i =>
            {
                ExpressionRegistry.DefaultParser = ExpressionRegistry.DefaultParser;
            });

            act.Should().NotThrow();
        }

        [Fact(DisplayName = "Swapping DefaultEvaluator is thread-safe")]
        public void SwappingDefaultEvaluator_IsThreadSafe()
        {
            Action act = () => Parallel.For(0, 1000, i =>
            {
                ExpressionRegistry.DefaultEvaluator = ExpressionRegistry.DefaultEvaluator;
            });

            act.Should().NotThrow();
        }

        [Fact]
        public void OnParsed_Event_IsRaised_AfterSuccessfulParse()
        {
            IExpression? captured = null;

            void Handler(IExpression expr) => captured = expr;

            try
            {
                ExpressionRegistry.OnParsed += Handler;
                var ok = ExpressionRegistry.DefaultParser.TryParse("123", out var expr, out var diags);

                ok.Should().BeTrue();
                captured.Should().NotBeNull();
                captured!.Canonical.Should().Be("123");
            }
            finally
            {
                // unsubscribe (safe)
                ExpressionRegistry.OnParsed -= Handler;
            }
        }

        [Fact]
        public void OnEvaluated_Event_IsRaised_WithResult()
        {
            IExpression? capExpr = null;
            object? capResult = null;
            void Handler(IExpression e, object? r) { capExpr = e; capResult = r; }

            try
            {
                ExpressionRegistry.OnEvaluated += Handler;
                var ok = ExpressionRegistry.DefaultParser.TryParse("123", out var expr, out var diags);
                ok.Should().BeTrue();

                var res = ExpressionRegistry.DefaultEvaluator.Evaluate(expr!, null);

                capExpr.Should().NotBeNull();
                capExpr!.Canonical.Should().Be("123");
                capResult.Should().Be(res);
            }
            finally
            {
                ExpressionRegistry.OnEvaluated -= Handler;
            }
        }
    }
}
