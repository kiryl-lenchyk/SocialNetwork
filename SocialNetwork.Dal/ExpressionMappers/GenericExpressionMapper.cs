using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Dal.ExpressionMappers
{
    internal class GenericExpressionMapper<TSource, TDestination> : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> parameters;

        private readonly Dictionary<MemberInfo, LambdaExpression> mappings;

        internal GenericExpressionMapper() : this(new Dictionary<MemberInfo, LambdaExpression>())
        {
        }

        internal GenericExpressionMapper(Dictionary<MemberInfo, LambdaExpression> mappings)
        {
            this.mappings = mappings;
            parameters = new Dictionary<ParameterExpression, ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof (TSource))
            {
                ParameterExpression parameter;
                if (!parameters.TryGetValue(node, out parameter))
                {
                    parameters.Add(node,
                        parameter = Expression.Parameter(typeof (TDestination), node.Name));
                }
                return parameter;
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == null || node.Expression.Type != typeof (TSource))
            {
                return base.VisitMember(node);
            }
            Expression expression = Visit(node.Expression);
            if (expression.Type != typeof (TDestination))
            {
                throw new InvalidOperationException("Mapping error");
            }
            LambdaExpression lambdaExpression;
            if (mappings.TryGetValue(node.Member, out lambdaExpression))
            {
                return
                    new SimpleExpressionReplacer(lambdaExpression.Parameters.Single(), expression)
                        .Visit(lambdaExpression.Body);
            }
            return Expression.Property(expression, node.Member.Name);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda( Visit(node.Body),
                node.Parameters.Select(Visit).Cast<ParameterExpression>());
        }

        protected internal static KeyValuePair<MemberInfo, LambdaExpression> 
            GetMappingFor<TValue>
            (Expression<Func<TSource, TValue>> sourceExpression,
                Expression<Func<TDestination, TValue>> destinationExpression)
        {
            return new KeyValuePair<MemberInfo, LambdaExpression>(((MemberExpression)sourceExpression.Body).Member,
                destinationExpression);
        }


    }
}
