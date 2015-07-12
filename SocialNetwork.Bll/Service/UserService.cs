using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Bll.Interface;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Logger.Interface;

namespace SocialNetwork.Bll.Service
{
    /// <summary>
    /// Represent business logic function for Users 
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields

        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;
        private readonly IRepository<DalAvatar> avatarRepository;
        private readonly ILogger logger;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instance of UserService for determinated storage.
        /// </summary>
        /// <param name="uow">unit of work for commit in storage.</param>
        /// <param name="userRepository">user strorage.</param>
        /// <param name="avatarRepository">avatar storage.</param>
        /// <param name="logger">class for log</param>
        public UserService(IUnitOfWork uow, IUserRepository userRepository, IRepository<DalAvatar> avatarRepository, ILogger logger)
        {

            this.uow = uow;
            this.userRepository = userRepository;
            this.avatarRepository = avatarRepository;
            this.logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="key">user id.</param>
        /// <returns>fiunded BllUser or null, if it's not found.</returns>
        public BllUser GetById(int key)
        {
            logger.Log(LogLevel.Trace,"UserService.GetById invoked key = {0}",key);
            DalUser dalUser = userRepository.GetById(key);
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        /// <summary>
        /// Get user by name.
        /// </summary>
        /// <param name="name">user name.</param>
        /// <returns>founded BllUser or null, if it's not found.</returns>
        public BllUser GetByName(string name)
        {
            if(name == null) throw new ArgumentNullException("name");
            logger.Log(LogLevel.Trace,"UserService.GetByName invoked name = {0}", name);

            DalUser dalUser = userRepository.GetByName(name);
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        /// <summary>
        /// Save new user to storage.
        /// </summary>
        /// <param name="e">new user to save. Id will be selected by storage.</param>
        /// <returns>added user with selected by storage id.</returns>
        public BllUser Create(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"UserService.Create invoked userName = {0}", e.UserName);
            
            DalUser dalUser = e.ToDalUser();
            dalUser = userRepository.Create(dalUser);
            uow.Commit();
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        /// <summary>
        /// Mark two users as friends.
        /// </summary>
        /// <param name="currentUserId">id of first friend.</param>
        /// <param name="newFriendId">id of second friend.</param>
        public void AddFriend(int currentUserId, int newFriendId)
        {
            logger.Log(LogLevel.Trace,"UserService.AddFriend invoked currentUserId = {0}, newFriendId = {1}",
                currentUserId, newFriendId);

            DalUser currentUser = GetByIdWithException(currentUserId, "currentUserId");
            DalUser newFriend = GetByIdWithException(newFriendId, "newFriendId");
            
            if (currentUser.FriendsId.Contains(newFriendId))
            {
                logger.Log(LogLevel.Debug,
                    "Try add to friend user, who is friend already currentUserId = {0}, newFriendId = {1}",
                    currentUserId, newFriendId);
                throw new InvalidOperationException("Users are friends");
            }

            userRepository.AddToFriends(currentUser, newFriend);
            uow.Commit();
        }
        
        /// <summary>
        /// Mark that two user are not fiends.
        /// </summary>
        /// <param name="currentUserId">id of first friend.</param>
        /// <param name="newFriendId">id of second friend.</param>
        public void RemoveFriend(int currentUserId, int newFriendId)
        {
            logger.Log(LogLevel.Trace,"UserService.RemoveFriend invoked currentUserId = {0}, newFriendId = {1}", currentUserId, newFriendId);

            DalUser currentUser = GetByIdWithException(currentUserId, "currentUserId");
            DalUser newFriend = GetByIdWithException(newFriendId, "newFriendId");

            if (!currentUser.FriendsId.Contains(newFriendId))
            {
                logger.Log(LogLevel.Debug,
                    "Try remove from friends user, who isnot friend now currentUserId = {0}, newFriendId = {1}",
                    currentUserId, newFriendId);
                throw new InvalidOperationException("Users are not friends");
            }

            userRepository.RemoveFriend(currentUser, newFriend);
            uow.Commit();
        }

        /// <summary>
        /// Find users by parametrs.
        /// </summary>
        /// <param name="name">user's name. If it's null search will not use name.</param>
        /// <param name="surname">user's surname. If it's null search will not use surname.</param>
        /// <param name="birthDayMin">min value of user's birthday. If it's null search will not use min value for birthday.</param>
        /// <param name="birthDayMax">max value of user's birthday. If it's null search will not use max value for birthday.</param>
        /// <param name="sex">user's sex. If it's null search will not use sex.</param>
        /// <returns>IEnumerable of founded users.</returns>
        public IEnumerable<BllUser> FindUsers(string name, string surname, DateTime? birthDayMin,
            DateTime? birthDayMax,
            BllSex? sex)
        {
            logger.Log(LogLevel.Trace,
                "UserService.FindUsers invoked  name = {0}, surname = {1}, birthDayMin = {2}, birthDayMax = {3}, sex = {4}",
                name ?? "null", surname ?? "null",
                birthDayMin == null ? "null" : birthDayMin.ToString(),
                birthDayMax == null ? "null" : birthDayMax.ToString(),
                sex == null ? "null" : sex.ToString());

            Expression<Func<DalUser, bool>> predicate = GetFindPredicate(name, surname, birthDayMin,
                birthDayMax, sex);
            return userRepository.GetAllByPredicate(predicate).ToList().Select(x => x.ToBllUser());
        }

        /// <summary>
        /// Get all users from storage.
        /// </summary>
        /// <returns>IEnumerable of all users.</returns>
        public IEnumerable<BllUser> GetAllUsers()
        {
            logger.Log(LogLevel.Trace,"UserService.GetAllUsers invoked");

            return userRepository.GetAll().ToList().Select(x => x.ToBllUser());
        }

        public IMappedPagedList<BllUser> GetAllUsersPage(int pageSize, int pageNumber)
        {
            logger.Log(LogLevel.Trace, "UserService.GetAllUsersPage invokedpageSize = {0} pageNumber = {1}",
                pageSize, pageNumber);

            return
                PagedList<BllUser>.GetPagedListWithConvert(
                    userRepository.GetAll().OrderBy(x => x.Id), pageSize, pageNumber,
                    x => x.ToBllUser());
        }

        /// <summary>
        /// Check is user existst by username.
        /// </summary>
        /// <param name="userName">username to found</param>
        /// <returns>true if user with determinated name existst, false else</returns>
        public bool IsUserExists(string userName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            logger.Log(LogLevel.Trace,"UserService.IsUserExists invoked userName = {0}", userName);

            return userRepository.GetByName(userName) != null;
        }

        /// <summary>
        /// Check is user existst by id.
        /// </summary>
        /// <param name="id">id to found</param>
        /// <returns>true if user with determinated id existst, false else</returns>
        public bool IsUserExists(int id)
        {
            logger.Log(LogLevel.Trace,"UserService.IsUserExists invoked id = {0}", id);

            return userRepository.GetById(id) != null;
        }

        /// <summary>
        /// Remove user from storage.
        /// </summary>
        /// <param name="e">user to delete. User will founded by id.</param>
        public void Delete(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"UserService.Delete invoked id = {0}", e.Id);

            userRepository.Delete(e.ToDalUser());
            uow.Commit();
        }

        /// <summary>
        /// Update user information.
        /// </summary>
        /// <param name="e">new value of user information. User will be found by id.</param>
        public void Update(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            logger.Log(LogLevel.Trace,"UserService.Update invoked id = {0}", e.Id);

            userRepository.Update(e.ToDalUser());
            uow.Commit();
        }

        /// <summary>
        /// Set new or update exists avatar for user.
        /// </summary>
        /// <param name="userId">user id for set avatar.</param>
        /// <param name="avatarStream">stream contains new avatar as image.</param>
        public void SetUserAvatar(int userId, Stream avatarStream)
        {
            if (avatarStream == null) throw new ArgumentNullException("avatarStream");
            logger.Log(LogLevel.Trace,"UserService.SetUserAvatar invoked userId = {0}", userId);

            avatarRepository.Update(new DalAvatar(){UserId = userId, ImageStream = avatarStream});
            uow.Commit();
        }

        /// <summary>
        /// Get avatar for user.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>stream contains avatar as image.</returns>
        public Stream GetUserAvatarStream(int userId)
        {
            logger.Log(LogLevel.Trace,"UserService.GetUserAvatarStream invoked userId = {0}", userId);

            return avatarRepository.GetById(userId).ImageStream;
        }

        /// <summary>
        /// Return true if userId can add friendId to friends and false else.
        /// </summary>
        /// <param name="userId">id of first new friend.</param>
        /// <param name="friendId">id of second new friend.</param>
        /// <returns>true if userId can add friendId to friends and false else.</returns>
        public bool CanUserAddToFriends(int userId, int friendId)
        {
            logger.Log(LogLevel.Trace,"UserService.CanUserAddToFriends invoked userId = {0}, friendId = {1}",
                userId, friendId);

            BllUser user = GetById(userId);
            if (user == null) return false;
            return !user.FriendsId.Contains(friendId) && userId != friendId;
        }

        /// <summary>
        /// Return true if senderId can write message to targetId and false else.
        /// </summary>
        /// <param name="targetId">id of message target.</param>
        /// <param name="senderId">id of message sender.</param>
        /// <returns>true if senderId can write message to targetId and false else.</returns>
        public bool CanUserWriteMessage(int targetId, int senderId)
        {
            logger.Log(LogLevel.Trace,"UserService.CanUserWriteMessage invoked targetId = {0}, senderId = {1}",
                targetId, senderId);

            return !CanUserAddToFriends(targetId, senderId) && targetId != senderId;
        }

        #endregion

        #region Private Methods

        private DalUser GetByIdWithException(int currentUserId, string exceptionArgumentName)
        {
            DalUser currentUser = userRepository.GetById(currentUserId);
            if (currentUser == null)
                throw new ArgumentException(
                    String.Format("User id = {0} is no existst", currentUserId), exceptionArgumentName);
            return currentUser;
        }

        private static Expression<Func<DalUser, bool>> GetFindPredicate(string name, string surname,
           DateTime? birthDayMin, DateTime? birthDayMax, BllSex? sex)
        {
            ParameterExpression parametr = Expression.Parameter(typeof(DalUser), "dalUser");
            Expression body = Expression.Constant(true, typeof(bool));

            if (!String.IsNullOrEmpty(name))
                body = AddFindPredicateExpression(name, typeof(String), body, parametr, "Name",
                    Expression.Equal);
            if (!String.IsNullOrEmpty(surname))
                body = AddFindPredicateExpression(surname, typeof(String), body, parametr,
                    "Surname", Expression.Equal);
            if (sex != null)
                body = AddFindPredicateExpression(sex.ToDalSex(), typeof(DalSex?), body, parametr,
                    "Sex", Expression.Equal);
            if (birthDayMin != null)
                body = AddFindPredicateExpression(birthDayMin, typeof(DateTime?), body, parametr,
                    "BirthDay", Expression.GreaterThanOrEqual);
            if (birthDayMax != null)
                body = AddFindPredicateExpression(birthDayMax, typeof(DateTime?), body, parametr,
                    "BirthDay",Expression.LessThanOrEqual);

            Expression<Func<DalUser, bool>> predicate =
                Expression.Lambda<Func<DalUser, bool>>(body, parametr);
            return predicate;
        }


        private static Expression AddFindPredicateExpression(object value, Type valuType,
            Expression body, ParameterExpression parametr, string propertyName,
            Func<Expression, Expression, BinaryExpression> compareRule)
        {
            return Expression.And(body,
                compareRule(
                    Expression.MakeMemberAccess(parametr, typeof(DalUser).GetProperty(propertyName)),
                    Expression.Constant(value, valuType)));
        }
        #endregion
    }
}
