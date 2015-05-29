using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Dal.ExpressionMappers
{
    internal class SimpleExpressionReplacer : ExpressionVisitor
    {
        private readonly Expression destination;
        private readonly Expression source;

        public SimpleExpressionReplacer(Expression source, Expression destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override Expression Visit(Expression node)
        {
            return node == source ? destination : base.Visit(node);
        }

        
    }

}
