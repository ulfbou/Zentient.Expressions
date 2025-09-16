// <copyright file="StubEvaluator.cs" authors="Zentient Framework Team">
// Copyright © 2025 Zentient Framework Team. All rights reserved.
// </copyright>

using Zentient.Abstractions.Expressions;

namespace Zentient.Expressions
{
    /// <summary>
    /// Minimal evaluator used internally by tests/stubs. Supports constant expressions and
    /// simple identifier/member lookup from IDictionary<string, object?> contexts.
    /// </summary>
    internal static class StubEvaluator
    {
        /// <summary>
        /// Evaluates the specified expression and returns a result.
        /// </summary>
        /// <param name="expr">The expression to evaluate.</param>
        /// <param name="context">An optional evaluation context. If a IDictionary&lt;string, object?&gt; is provided identifiers and member access will be resolved from it.</param>
        /// <returns>The evaluated value for supported node types; otherwise <c>null</c>.</returns>
        public static object? Evaluate(IExpression expr, object? context)
        {
            return expr switch
            {
                ConstantExpression c => c.Value,
                IdentifierExpression id => ResolveIdentifier(id.Name, context),
                MemberAccessExpression m => ResolveMember(m.Target, m.MemberName, context),
                MethodCallExpression mc => EvaluateMethodCall(mc, context),
                _ => null
            };
        }

        private static object? ResolveIdentifier(string name, object? context)
        {
            if (context is System.Collections.IDictionary dict)
            {
                if (dict.Contains(name)) return dict[name];
            }
            else if (context is System.Collections.Generic.IDictionary<string, object?> gen)
            {
                if (gen.TryGetValue(name, out var v)) return v;
            }
            return null;
        }

        private static object? ResolveMember(IExpression targetExpr, string memberName, object? context)
        {
            var targetVal = Evaluate(targetExpr, context);
            if (targetVal is System.Collections.IDictionary dict)
            {
                if (dict.Contains(memberName)) return dict[memberName];
            }
            else if (targetVal is System.Collections.Generic.IDictionary<string, object?> gen)
            {
                if (gen.TryGetValue(memberName, out var v)) return v;
            }

            // As a fallback, reflect public properties
            if (targetVal is not null)
            {
                var t = targetVal.GetType();
                var prop = t.GetProperty(memberName);
                if (prop != null) return prop.GetValue(targetVal);
            }

            return null;
        }

        private static object? EvaluateMethodCall(MethodCallExpression mc, object? context)
        {
            var target = Evaluate(mc.Target, context);
            var rawArgs = mc.Arguments.Select(a => Evaluate(a, context)).ToArray();

            // If target is a delegate, invoke with coerced args
            if (target is Delegate d)
            {
                var adapted = AdaptArgumentsForDelegate(d, rawArgs);
                return adapted is null ? null : InvokeDelegate(d, adapted);
            }

            // If target is a dictionary containing the method name as a delegate, invoke it
            if (target is System.Collections.Generic.IDictionary<string, object?> targetGen && targetGen.TryGetValue(mc.MethodName, out var maybeDel) && maybeDel is Delegate del2)
            {
                var adapted = AdaptArgumentsForDelegate(del2, rawArgs);
                return adapted is null ? null : InvokeDelegate(del2, adapted);
            }

            if (target is System.Collections.IDictionary targetDict && targetDict.Contains(mc.MethodName) && targetDict[mc.MethodName] is Delegate del3)
            {
                var adapted = AdaptArgumentsForDelegate(del3, rawArgs);
                return adapted is null ? null : InvokeDelegate(del3, adapted);
            }

            // Try lookup in context by method name when target didn't provide it
            if (context is System.Collections.Generic.IDictionary<string, object?> dict && dict.TryGetValue(mc.MethodName, out var maybeDel2) && maybeDel2 is Delegate dd)
            {
                var adapted = AdaptArgumentsForDelegate(dd, rawArgs);
                return adapted is null ? null : InvokeDelegate(dd, adapted);
            }

            if (context is System.Collections.IDictionary ctxDict && ctxDict.Contains(mc.MethodName) && ctxDict[mc.MethodName] is Delegate dd2)
            {
                var adapted = AdaptArgumentsForDelegate(dd2, rawArgs);
                return adapted is null ? null : InvokeDelegate(dd2, adapted);
            }

            // As a final fallback, try reflection on the target for a delegate-valued property
            if (target is not null)
            {
                var t = target.GetType();
                var prop = t.GetProperty(mc.MethodName);
                if (prop != null)
                {
                    var val = prop.GetValue(target);
                    if (val is Delegate ddd)
                    {
                        var adapted = AdaptArgumentsForDelegate(ddd, rawArgs);
                        return adapted is null ? null : InvokeDelegate(ddd, adapted);
                    }
                }
            }

            // Deep search: find delegate by key in nested dictionaries or properties
            var found = FindDelegateInObject(target, mc.MethodName) ?? FindDelegateInObject(context, mc.MethodName);
            if (found is Delegate fdel)
            {
                var adapted = AdaptArgumentsForDelegate(fdel, rawArgs);
                return adapted is null ? null : InvokeDelegate(fdel, adapted);
            }

            return null;
        }

        private static object? InvokeDelegate(Delegate del, object[] args)
        {
            // Allow exceptions to surface so calling tests can detect invocation issues
            return del.DynamicInvoke(args);
        }

        private static object[]? AdaptArgumentsForDelegate(Delegate del, object?[] rawArgs)
        {
            var parameters = del.Method.GetParameters();
            if (parameters.Length != rawArgs.Length) return null;
            var adapted = new object?[rawArgs.Length];
            for (int i = 0; i < rawArgs.Length; i++)
            {
                var targetType = parameters[i].ParameterType;
                var val = rawArgs[i];
                if (val == null)
                {
                    adapted[i] = null;
                    continue;
                }

                if (targetType.IsInstanceOfType(val))
                {
                    adapted[i] = val;
                    continue;
                }

                try
                {
                    adapted[i] = Convert.ChangeType(val, targetType);
                }
                catch
                {
                    // Failed to convert
                    return null;
                }
            }

            return adapted.Cast<object>().ToArray();
        }

        private static object? FindDelegateInObject(object? obj, string methodName)
        {
            if (obj is null) return null;
            if (obj is Delegate d) return d;

            if (obj is System.Collections.Generic.IDictionary<string, object?> gen)
            {
                if (gen.TryGetValue(methodName, out var v) && v is Delegate dv) return dv;
                foreach (var val in gen.Values)
                {
                    var found = FindDelegateInObject(val, methodName);
                    if (found is Delegate) return found;
                }
            }

            if (obj is System.Collections.IDictionary nonGen)
            {
                if (nonGen.Contains(methodName) && nonGen[methodName] is Delegate dv2) return dv2;
                foreach (System.Collections.DictionaryEntry entry in nonGen)
                {
                    var found = FindDelegateInObject(entry.Value, methodName);
                    if (found is Delegate) return found;
                }
            }

            // reflect properties
            var t = obj.GetType();
            foreach (var prop in t.GetProperties())
            {
                var val = prop.GetValue(obj);
                var found = FindDelegateInObject(val, methodName);
                if (found is Delegate) return found;
            }

            return null;
        }
    }
}
