using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.ExpressionMappers
{
    class MessageExpressionMapper : GenericExpressionMapper<DalMessage, Message>
    {
        private static readonly Dictionary<MemberInfo, LambdaExpression> Mappings;

        static MessageExpressionMapper()
        {
            Mappings = new Dictionary<MemberInfo, LambdaExpression>();
            KeyValuePair<MemberInfo, LambdaExpression> mapping =
                GetMappingFor(dalMessage => dalMessage.SenderId,
                    message => message.Sender);
            Mappings.Add(mapping.Key,mapping.Value);

            mapping = GetMappingFor(dalMessage => dalMessage.TargetId,
                   message => message.Target);
            Mappings.Add(mapping.Key, mapping.Value);

        }

        internal MessageExpressionMapper()
            : base(Mappings)
        {
        }
    }
}
