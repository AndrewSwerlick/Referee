using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Swerl.Referee.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetMethodName(this LambdaExpression expression)
        {
            if (!(expression.Body is MethodCallExpression))
                throw new ArgumentException("Expression must be a method call", "expression");

            var methodExpression = (MethodCallExpression)expression.Body;
            return methodExpression.Method.Name;
        }

        public static object[] GetMethodParams(this LambdaExpression expression)
        {
            if (!(expression.Body is MethodCallExpression))
                throw new ArgumentException("Expression must be a method call", "expression");

            var methodExpression = (MethodCallExpression)expression.Body;
            return methodExpression.Arguments.Select(GetValue).ToArray();
        }

        public static MethodInfo GetMethodInfor(this LambdaExpression expression)
        {
            if (!(expression.Body is MethodCallExpression))
                throw new ArgumentException("Expression must be a method call", "expression");

            var methodExpression = (MethodCallExpression)expression.Body;
            return methodExpression.Method;
        }

        private static object GetValue(Expression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }
    }
}
