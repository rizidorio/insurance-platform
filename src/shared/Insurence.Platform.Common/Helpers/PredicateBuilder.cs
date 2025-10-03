using System.Linq.Expressions;

namespace Insurence.Platform.Common.Helpers;

/// <summary>
/// Classe auxiliar para construir expressões de predicados de forma dinâmica
/// </summary>
public static class PredicateBuilder
{
    /// <summary>
    /// Cria um predicado que sempre retorna verdadeiro
    /// </summary>
    public static Expression<Func<T, bool>> True<T>() => param => true;

    /// <summary>
    /// Cria um predicado que sempre retorna falso
    /// </summary>
    public static Expression<Func<T, bool>> False<T>() => param => false;

    /// <summary>
    /// Combina o predicado atual com outro usando o operador AND
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T), "param");

        var visitor = new ParameterReplacer(parameter);
        var firstBody = visitor.Replace(first.Body);
        var secondBody = visitor.Replace(second.Body);

        var body = Expression.AndAlso(firstBody, secondBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    /// <summary>
    /// Combina o predicado atual com outro usando o operador OR
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T), "param");

        var visitor = new ParameterReplacer(parameter);
        var firstBody = visitor.Replace(first.Body);
        var secondBody = visitor.Replace(second.Body);

        var body = Expression.OrElse(firstBody, secondBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private sealed class ParameterReplacer(ParameterExpression parameter)
    {
        private readonly ParameterExpression _parameter = parameter;

        public Expression Replace(Expression expression)
        {
            return new ParameterVisitor(_parameter).Visit(expression);
        }

        private sealed class ParameterVisitor(ParameterExpression parameter) : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter = parameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }
    }
}