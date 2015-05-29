using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.ExpressionMappers
{
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

        internal UserExpressionMapper()
            : base(Mappings)
        {
        }
    }
}
