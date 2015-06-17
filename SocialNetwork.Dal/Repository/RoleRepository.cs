using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using SocialNetwork.Dal.ExpressionMappers;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Dal.Mappers;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly DbContext context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RoleRepository(DbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<DalRole> GetAll()
        {
            Logger.Trace("RoleRepository.GetAll ivoked");
            return context.Set<Role>().Select(RoleMapper.ToDalRolExpression);
        }

        public DalRole GetById(int key)
        {
            Logger.Trace("RoleRepository.GetById invoked key = {0}", key);

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == key);
            return ormRole == null ? null : ormRole.ToDalRole();

        }

        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if(predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("RoleRepository.GetByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole,Role>().Visit(predicate));

            Role ormRole = context.Set<Role>().FirstOrDefault(convertedPredicate);
            return ormRole != null ? ormRole.ToDalRole() : null;
        }

        public IQueryable<DalRole> GetAllByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("RoleRepository.GetAllByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole, Role>().Visit(predicate));

            return context.Set<Role>().Where(convertedPredicate).Select(RoleMapper.ToDalRolExpression);
        }

        public DalRole Create(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("RoleRepository.Create invoked roleName = {0}", e.Name);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Add(ormRole);
            return ormRole.ToDalRole();
        }

        public void Delete(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("RoleRepository.Delete invoked id = {0}", e.Id);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Remove(ormRole);
        }

        public void Update(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("RoleRepository.Update invoked id = {0}", e.Id);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().AddOrUpdate(ormRole);
        }

        public DalRole GetByName(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");
            Logger.Trace("RoleRepository.GetByName invoked name = {0}", roleName);
            
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Name == roleName);
            return ormRole == null ? null : ormRole.ToDalRole();
        }

        public IEnumerable<DalRole> GetUserRoles(DalUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            Logger.Trace("RoleRepository.GetUserRoles invoked userId = {0}", user.Id);

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == user.Id);
            if (ormUser == null)
            {
                Logger.Debug("RoleRepository.GetUserRoles cant find user userId = {0}", user.Id);
                return new List<DalRole>();
            }
            return ormUser.Roles.Select(x => x.ToDalRole());
        }

        public IEnumerable<DalUser> GetRoleUsers(DalRole role)
        {
            if (role == null) throw new ArgumentNullException("role");
            Logger.Trace("RoleRepository.GetRoleUsers invoked roleId = {0}", role.Id);

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                Logger.Debug("RoleRepository.GetUserRoles cant find role roleId = {0}", role.Id);
                return new List<DalUser>();
            }
            return
                context.Set<User>().Where(x => x.Roles.Contains(ormRole)).Select(x => x.ToDalUser());
        }

        public void AddUserToRole(DalUser user, DalRole role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (role == null) throw new ArgumentNullException("role");
            Logger.Trace("RoleRepository.AddUserToRole invoked roleId = {0} userId = {1}",
                role.Id, user.Id);

            User ormUser = GetOrmUserWithRoles(user, "AddUserToRole");
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                Logger.Debug("RoleRepository.AddUserToRole cant find role roleId = {0}", role.Id);
                throw new ArgumentException("Role has incorrect id");
            }

            ormUser.Roles.Add(ormRole);
        }

        public void RemoveUserFromRole(DalUser user, DalRole role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (role == null) throw new ArgumentNullException("role");
            Logger.Trace("RoleRepository.RemoveUserFromRole invoked roleId = {0} userId = {1}",
                role.Id, user.Id);

            User ormUser = GetOrmUserWithRoles(user, "RemoveUserFromRole");
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                Logger.Debug("RoleRepository.RemoveUserFromRole cant find role roleId = {0}", role.Id);
                throw new ArgumentException("Role has incorrect id");
            }

            ormUser.Roles.Remove(ormRole);
        }

        private User GetOrmUserWithRoles(DalUser dalUser, string logMethodName)
        {
            User ormCurrentUser = context.Set<User>().SingleOrDefault(x => x.Id == dalUser.Id);
            if (ormCurrentUser == null)
            {
                Logger.Debug("RoleRepository.{0} cant find user id = {1}", logMethodName,dalUser.Id);
                throw new ArgumentException("User has incorrect id");
            }
            context.Entry(ormCurrentUser).Collection(x => x.Roles).Load();
            return ormCurrentUser;
        }
    }
}
