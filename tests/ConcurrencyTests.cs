using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions.Tests
{
    public class ConcurrencyTests
    {
        [Fact(DisplayName = "Registry concurrent subscribe/unsubscribe and parse")]
        public void Registry_Concurrent_Subscribe_Unsubscribe_Parse()
        {
            var parsedCount = 0;
            Action<IExpression> handler = _ => System.Threading.Interlocked.Increment(ref parsedCount);

            var tasks = new List<Task>();

            // concurrently subscribe/unsubscribe
            for (int i = 0; i < 50; i++)
            {
                tasks.Add(Task.Run(() => ExpressionRegistry.OnParsed += handler));
            }

            for (int i = 0; i < 50; i++)
            {
                tasks.Add(Task.Run(() => ExpressionRegistry.OnParsed -= handler));
            }

            // concurrently perform parses
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    ExpressionRegistry.DefaultParser.TryParse("123", out var expr, out var diags);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            // After concurrent operations, subscribing/unsubscribing should not cause crashes and registry remains usable
            var ok = ExpressionRegistry.DefaultParser.TryParse("456", out var e2, out var ds2);
            ok.Should().BeTrue();
            e2!.Canonical.Should().Be("456");
        }
    }
}
