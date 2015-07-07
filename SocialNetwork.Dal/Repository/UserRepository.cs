using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
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
    /// Represent UserRepository for database as storage.
    /// </summary>
    public class UserRepository : IUserRepository
    {

        #region Fields

        private readonly DbContext context;
        private readonly ILogger logger;

        #endregion

        #region Constractor

        /// <summary>
        /// Create new instanse of UserRepository.
        /// </summary>
        /// <param name="context">DbContext for save data</param>
        /// <param name="logger">class for log</param>
        public UserRepository(DbContext context, ILogger logger)
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
        public IQueryable<DalUser> GetAll()
        {
            logger.Log(LogLevel.Trace, "UserRepository.GetAll ivoked");
            try
            {
                return context.Set<User>().Select(UserMapper.ToDalUserConvertion);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="key">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        public DalUser GetById(int key)
        {
            logger.Log(LogLevel.Trace, "UserRepository.GetById invoked key = {0}", key);
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == key);
                return ormUser != null ? ormUser.ToDalUser() : null;
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Get user by name, or null if role not found.
        /// </summary>
        /// <param name="name">user name for search</param>
        /// <returns>founded user, or null if it's not found.</returns>
        public DalUser GetByName(String name)
        {
            if (name == null) throw new ArgumentNullException("name");
            logger.Log(LogLevel.Trace, "UserRepository.GetByName invoked name = {0}", name);
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(x => x.UserName == name);
                return ormUser != null ? ormUser.ToDalUser() : null;
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Mark two users as friend.
        /// </summary>
        /// <param name="currentUser">first user to be friend.</param>
        /// <param name="newFriend">second user to be friend.</param>
        public void AddToFriends(DalUser currentUser, DalUser newFriend)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            if (newFriend == null) throw new ArgumentNullException("newFriend");
            logger.Log(LogLevel.Trace,
                "UserRepository.AddToFriends invoked currentUser = {0}, newFriend = {1} ",
                currentUser, newFriend);
            try
            {
                User ormCurrentUser = GetOrmUserWithFriends(currentUser);
                User ormNewFriend = GetOrmUserWithFriends(newFriend);
                ormCurrentUser.Friends.Add(ormNewFriend);
                ormNewFriend.Friends.Add(ormCurrentUser);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }


        /// <summary>
        /// Mark that users are not friends.
        /// </summary>
        /// <param name="currentUser">first user to delete from friends.</param>
        /// <param name="newFriend">second user to delete from friends.</param>
        public void RemoveFriend(DalUser currentUser, DalUser newFriend)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            if (newFriend == null) throw new ArgumentNullException("newFriend");
            logger.Log(LogLevel.Trace,
                "UserRepository.AddToFriends invoked currentUser = {0}, newFriend = {1} ",
                currentUser, newFriend);
            try
            {
                User ormCurrentUser = GetOrmUserWithFriends(currentUser);
                User ormNewFriend = GetOrmUserWithFriends(newFriend);
                ormCurrentUser.Friends.Remove(ormNewFriend);
                ormNewFriend.Friends.Remove(ormCurrentUser);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Get entity by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>first founded entity or null if it not found.</returns>
        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace, "UserRepository.GetByPredicate invoked predicate = {0}",
                predicate.ToString());

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>) (new UserExpressionMapper().Visit(predicate));
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
                return ormUser != null ? ormUser.ToDalUser() : null;
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        ///  Get all entites by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>IQueryable of entites, you can write long additional query to storage</returns>
        public IQueryable<DalUser> GetAllByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace, "UserRepository.GetAllByPredicate invoked predicate = {0}",
                predicate.ToString());

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>) (new UserExpressionMapper().Visit(predicate));
            try
            {
                return
                    context.Set<User>()
                        .Where(convertedPredicate)
                        .Select(UserMapper.ToDalUserConvertion);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Add entity to storage. Id will be selected by storage.
        /// </summary>
        /// <param name="e">new entity without id.</param>
        /// <returns>created entity with new id.</returns>
        public DalUser Create(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace, "UserRepository.Create invoked userName = {0}", e.UserName);
            try
            {
                User ormUser = e.ToOrmUser();
                context.Set<User>().Add(ormUser);
                return ormUser.ToDalUser();
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        public void Delete(DalUser e)
        {
            logger.Log(LogLevel.Trace, "UserRepository.Delete invoked id = {0}", e.Id);
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == e.Id);
                if (ormUser == null) throw new ArgumentException("User has incorrect id");
                context.Set<User>().Remove(ormUser);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="e">new value for entity.</param>
        public void Update(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace, "UserRepository.Update invoked id = {0}", e.Id);
            try
            {
                context.Set<User>().AddOrUpdate(e.ToOrmUser());
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        #endregion

        #region Private Methods

        private User GetOrmUserWithFriends(DalUser dalUser)
        {
            User ormCurrentUser = context.Set<User>().SingleOrDefault(x => x.Id == dalUser.Id);
            if (ormCurrentUser == null) throw new ArgumentException("User has incorrect id");

            context.Entry(ormCurrentUser).Collection(x => x.Friends).Load();
            return ormCurrentUser;
        }

        #endregion

    }
}
