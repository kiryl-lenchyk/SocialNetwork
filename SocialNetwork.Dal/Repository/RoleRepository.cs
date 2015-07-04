using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Dal.ExpressionMappers;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Dal.Mappers;
using SocialNetwork.Logger.Interface;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Repository
{

    /// <summary>
    /// Represent RoleRepository for database as storage.
    /// </summary>
    public class RoleRepository : IRoleRepository
    {

        #region Fields

        private readonly DbContext context;
        private readonly ILogger logger;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instanse of RoleRepository.
        /// </summary>
        /// <param name="context">DbContext for save data</param>
        /// <param name="logger">class for log</param>
        public RoleRepository(DbContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all elements in storage. Return IQueryable for wroting long query to storage.
        /// </summary>
        /// <returns>IQuaryable of all elements. You can add LINQ query to it. Quary will be invoked by storage</returns>
        public IQueryable<DalRole> GetAll()
        {
            logger.Log(LogLevel.Trace,"RoleRepository.GetAll ivoked");
            return context.Set<Role>().Select(RoleMapper.ToDalRolExpression);
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="key">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        public DalRole GetById(int key)
        {
            logger.Log(LogLevel.Trace,"RoleRepository.GetById invoked key = {0}", key);

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == key);
            return ormRole == null ? null : ormRole.ToDalRole();

        }

        /// <summary>
        /// Get entity by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>first founded entity or null if it not found.</returns>
        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if(predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace,"RoleRepository.GetByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole,Role>().Visit(predicate));

            Role ormRole = context.Set<Role>().FirstOrDefault(convertedPredicate);
            return ormRole != null ? ormRole.ToDalRole() : null;
        }

        /// <summary>
        ///  Get all entites by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>IQueryable of entites, you can write long additional query to storage</returns>
        public IQueryable<DalRole> GetAllByPredicate(Expression<Func<DalRole, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace,"RoleRepository.GetAllByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Role, bool>> convertedPredicate =
                 (Expression<Func<Role, bool>>)(new GenericExpressionMapper<DalRole, Role>().Visit(predicate));

            return context.Set<Role>().Where(convertedPredicate).Select(RoleMapper.ToDalRolExpression);
        }

        /// <summary>
        /// Add entity to storage. Id will be selected by storage.
        /// </summary>
        /// <param name="e">new entity without id.</param>
        /// <returns>created entity with new id.</returns>
        public DalRole Create(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"RoleRepository.Create invoked roleName = {0}", e.Name);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Add(ormRole);
            return ormRole.ToDalRole();
        }

        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        public void Delete(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"RoleRepository.Delete invoked id = {0}", e.Id);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().Remove(ormRole);
        }

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="e">new value for entity.</param>
        public void Update(DalRole e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"RoleRepository.Update invoked id = {0}", e.Id);

            Role ormRole = e.ToOrmRole();
            context.Set<Role>().AddOrUpdate(ormRole);
        }

        /// <summary>
        /// Get role by name, or null if role not found.
        /// </summary>
        /// <param name="roleName">role name for search</param>
        /// <returns>founded role or null if it's not found</returns>
        public DalRole GetByName(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName");
            logger.Log(LogLevel.Trace,"RoleRepository.GetByName invoked name = {0}", roleName);
            
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Name == roleName);
            return ormRole == null ? null : ormRole.ToDalRole();
        }

        /// <summary>
        /// Get IEnumerable of roles for one user.
        /// </summary>
        /// <param name="user">user to find roles.</param>
        /// <returns>IEnumerable of roles</returns>
        public IEnumerable<DalRole> GetUserRoles(DalUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            logger.Log(LogLevel.Trace,"RoleRepository.GetUserRoles invoked userId = {0}", user.Id);

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == user.Id);
            if (ormUser == null)
            {
                logger.Log(LogLevel.Debug,"RoleRepository.GetUserRoles cant find user userId = {0}", user.Id);
                return new List<DalRole>();
            }
            return ormUser.Roles.Select(x => x.ToDalRole());
        }

        /// <summary>
        /// Get IEnumarable of user for one role.
        /// </summary>
        /// <param name="role">role to find users.</param>
        /// <returns>IEnumerable of users</returns>
        public IEnumerable<DalUser> GetRoleUsers(DalRole role)
        {
            if (role == null) throw new ArgumentNullException("role");
            logger.Log(LogLevel.Trace,"RoleRepository.GetRoleUsers invoked roleId = {0}", role.Id);

            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                logger.Log(LogLevel.Debug,"RoleRepository.GetUserRoles cant find role roleId = {0}", role.Id);
                return new List<DalUser>();
            }
            return
                context.Set<User>().Where(x => x.Roles.Contains(ormRole)).Select(x => x.ToDalUser());
        }

        /// <summary>
        /// Add role to user's roles list.
        /// </summary>
        /// <param name="user">user to add role.</param>
        /// <param name="role">new user role.</param>
        public void AddUserToRole(DalUser user, DalRole role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (role == null) throw new ArgumentNullException("role");
            logger.Log(LogLevel.Trace,"RoleRepository.AddUserToRole invoked roleId = {0} userId = {1}",
                role.Id, user.Id);

            User ormUser = GetOrmUserWithRoles(user, "AddUserToRole");
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                logger.Log(LogLevel.Debug,"RoleRepository.AddUserToRole cant find role roleId = {0}", role.Id);
                throw new ArgumentException("Role has incorrect id");
            }

            ormUser.Roles.Add(ormRole);
        }

        /// <summary>
        /// Remove role from user's roles list.
        /// </summary>
        /// <param name="user">user to remove role.</param>
        /// <param name="role">user role to remove.</param>
        public void RemoveUserFromRole(DalUser user, DalRole role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (role == null) throw new ArgumentNullException("role");
            logger.Log(LogLevel.Trace,"RoleRepository.RemoveUserFromRole invoked roleId = {0} userId = {1}",
                role.Id, user.Id);

            User ormUser = GetOrmUserWithRoles(user, "RemoveUserFromRole");
            Role ormRole = context.Set<Role>().FirstOrDefault(x => x.Id == role.Id);
            if (ormRole == null)
            {
                logger.Log(LogLevel.Debug,"RoleRepository.RemoveUserFromRole cant find role roleId = {0}", role.Id);
                throw new ArgumentException("Role has incorrect id");
            }

            ormUser.Roles.Remove(ormRole);
        }

        #endregion

        #region Private Methods

        private User GetOrmUserWithRoles(DalUser dalUser, string logMethodName)
        {
            User ormCurrentUser = context.Set<User>().SingleOrDefault(x => x.Id == dalUser.Id);
            if (ormCurrentUser == null)
            {
                logger.Log(LogLevel.Debug,"RoleRepository.{0} cant find user id = {1}", logMethodName,dalUser.Id);
                throw new ArgumentException("User has incorrect id");
            }
            context.Entry(ormCurrentUser).Collection(x => x.Roles).Load();
            return ormCurrentUser;
        }

        #endregion
    }
}
