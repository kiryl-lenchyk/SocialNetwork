using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.ExpressionMappers
{

    /// <summary>
    /// ExpressionMapper to change Expression with DalUser arguments  Expression with User arguments
    /// </summary>
    class UserExpressionMapper : GenericExpressionMapper<DalUser, User>
    {
        private static readonly Dictionary<MemberInfo, LambdaExpression> Mappings;

        static UserExpressionMapper()
        {
            Mappings = new Dictionary<MemberInfo, LambdaExpression>();
            KeyValuePair<MemberInfo, LambdaExpression> mapping =
                GetMappingFor(dalUser => dalUser.Sex,
                    user => user.Sex != null ? (DalSex?) (int) user.Sex.Value : null);
            Mappings.Add(mapping.Key,mapping.Value);

        }

        /// <summary>
        /// Create new instanse of UserExpressionMapper.
        /// </summary>
        internal UserExpressionMapper()
            : base(Mappings)
        {
        }
    }
}
