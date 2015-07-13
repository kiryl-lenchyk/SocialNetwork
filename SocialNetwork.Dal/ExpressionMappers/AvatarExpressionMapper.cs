using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.ExpressionMappers
{

    /// <summary>
    /// ExpressionMapper to change Expression with DalAvatar arguments  Expression with User arguments
    /// </summary>
    class AvatarExpressionMapper : GenericExpressionMapper<DalAvatar, User>
    {
        private static readonly Dictionary<MemberInfo, LambdaExpression> Mappings;

        static AvatarExpressionMapper()
        {
            Mappings = new Dictionary<MemberInfo, LambdaExpression>();

            KeyValuePair<MemberInfo, LambdaExpression> mapping =
                GetMappingFor(dalAvatar => dalAvatar.UserId,
                    user => user.Id);
            Mappings.Add(mapping.Key, mapping.Value);

            mapping = GetMappingFor(dalAvatar => dalAvatar.ImageBytes,
                user => user.Avatar);
            Mappings.Add(mapping.Key, mapping.Value);

        }

        /// <summary>
        /// Create new instanse of AvatarExpressionMapper.
        /// </summary>
        internal AvatarExpressionMapper()
            : base(Mappings)
        {
        }
    }
}
