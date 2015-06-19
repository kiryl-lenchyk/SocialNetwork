using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SocialNetwork.Dal.ExpressionMappers
{

    /// <summary>
    /// ExpressionVisitor that convert Expression wich get TSource as argument to 
    /// expression wich get TDestination as argument
    /// </summary> 
    /// <typeparam name="TSource">Type of argument of source Expression</typeparam>
    /// <typeparam name="TDestination">Type of argument of distination Expression</typeparam>
    internal class GenericExpressionMapper<TSource, TDestination> : ExpressionVisitor
    {

        #region Fields
        
        private readonly Dictionary<ParameterExpression, ParameterExpression> parameters;

        private readonly Dictionary<MemberInfo, LambdaExpression> mappings;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instanse of GenericExpressionMapper with empty mapper dictionary. 
        /// Can map only if members name are same in TSource and TDestination
        /// </summary>
        internal GenericExpressionMapper() : this(new Dictionary<MemberInfo, LambdaExpression>())
        {
        }

        /// <summary>
        /// Create new instanse of GenericExpressionMapper with terminated mapper dictionary. 
        /// </summary>
        /// <param name="mappings">rules for mapping TSource's members to TDestination's members.
        ///  If membres have same names rule is not necessary</param>
        internal GenericExpressionMapper(Dictionary<MemberInfo, LambdaExpression> mappings)
        {
            this.mappings = mappings;
            parameters = new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Protected Ovverided Methods

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
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

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MemberExpression"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
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

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.Expression`1"/>.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param><typeparam name="T">The type of the delegate.</typeparam>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda( Visit(node.Body),
                node.Parameters.Select(Visit).Cast<ParameterExpression>());
        }

        #endregion

        #region Protected Static Methods

        /// <summary>
        /// Get new mapper to member.
        /// </summary>
        /// <typeparam name="TValue">type of member result value</typeparam>
        /// <param name="sourceExpression">MemberExpression to acsess to member on TSource</param>
        /// <param name="destinationExpression">Expression represent acsess to same member in TDestination</param>
        /// <returns></returns>
        protected internal static KeyValuePair<MemberInfo, LambdaExpression> 
            GetMappingFor<TValue>
            (Expression<Func<TSource, TValue>> sourceExpression,
                Expression<Func<TDestination, TValue>> destinationExpression)
        {
            return new KeyValuePair<MemberInfo, LambdaExpression>(((MemberExpression)sourceExpression.Body).Member,
                destinationExpression);
        }

        #endregion

    }
}
