using System.Linq.Expressions;

namespace SocialNetwork.Dal.ExpressionMappers
{

    /// <summary>
    /// Simple perlacer. Replace one Expression in Expression Tree to other.
    /// </summary>
    internal class SimpleExpressionReplacer : ExpressionVisitor
    {

        #region Fields

        private readonly Expression destination;
        private readonly Expression source;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instanse of SimpleExpressionReplacer.
        /// </summary>
        /// <param name="source">Expression to replace.</param>
        /// <param name="destination">new Expression.</param>
        public SimpleExpressionReplacer(Expression source, Expression destination)
        {
            this.source = source;
            this.destination = destination;
        }

        #endregion

        #region Protected Overrided Methods

        /// <summary>
        /// Dispatches the expression to one of the more specialized visit methods in this class.
        /// </summary>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        /// <param name="node">The expression to visit.</param>
        public override Expression Visit(Expression node)
        {
            return node == source ? destination : base.Visit(node);
        }

        #endregion
    }

}
