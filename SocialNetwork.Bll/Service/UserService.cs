using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;

namespace SocialNetwork.Bll.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;
        private readonly IRepository<DalAvatar> avatarRepository;


        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UserService(IUnitOfWork uow, IUserRepository userRepository, IRepository<DalAvatar> avatarRepository)
        {

            this.uow = uow;
            this.userRepository = userRepository;
            this.avatarRepository = avatarRepository;
        }

        public BllUser GetById(int key)
        {
            Logger.Trace("UserService.GetById invoked key = {0}",key);
            DalUser dalUser = userRepository.GetById(key);
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        public BllUser GetByName(string name)
        {
            if(name == null) throw new ArgumentNullException("name");
            Logger.Trace("UserService.GetByName invoked name = {0}", name);

            DalUser dalUser = userRepository.GetByName(name);
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        public BllUser Create(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("UserService.Create invoked userName = {0}", e.UserName);
            
            DalUser dalUser = e.ToDalUser();
            dalUser = userRepository.Create(dalUser);
            uow.Commit();
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        public void AddFriend(int currentUserId, int newFriendId)
        {
            Logger.Trace("UserService.AddFriend invoked currentUserId = {0}, newFriendId = {1}", currentUserId, newFriendId);

            DalUser currentUser = userRepository.GetById(currentUserId);
            if (currentUser == null)
                throw new ArgumentException(
                    String.Format("User id = {0} is no existst", currentUserId), "currentUserId");

            DalUser newFriend = userRepository.GetById(newFriendId);
            if (newFriend == null)
                throw new ArgumentException(
                    String.Format("User id = {0} is no existst", newFriendId), "newFriendId");

            if (currentUser.FriendsId.Contains(newFriendId))
            {
                Logger.Debug("Try add to friend user, who is friend already currentUserId = {0}, newFriendId = {1}", currentUserId, newFriendId);
                throw new InvalidOperationException("Users are friends");
            }

            userRepository.AddToFriends(currentUser, newFriend);
            uow.Commit();
        }

        public void RemoveFriend(int currentUserId, int newFriendId)
        {
            Logger.Trace("UserService.RemoveFriend invoked currentUserId = {0}, newFriendId = {1}", currentUserId, newFriendId);

            DalUser currentUser = userRepository.GetById(currentUserId);
            if (currentUser == null)
                throw new ArgumentException(
                    String.Format("User id = {0} is no existst", currentUserId), "currentUserId");

            DalUser newFriend = userRepository.GetById(newFriendId);
            if (newFriend == null)
                throw new ArgumentException(
                    String.Format("User id = {0} is no existst", newFriendId), "newFriendId");

            if (!currentUser.FriendsId.Contains(newFriendId))
            {
                Logger.Debug("Try remove from friends user, who isnot friend now currentUserId = {0}, newFriendId = {1}", currentUserId, newFriendId);
                throw new InvalidOperationException("Users are not friends");
            }

            userRepository.RemoveFriend(currentUser, newFriend);
            uow.Commit();
        }

        public IEnumerable<BllUser> FindUsers(string name, string surname, DateTime? birthDayMin,
            DateTime? birthDayMax,
            BllSex? sex)
        {
            Logger.Trace(
                "UserService.FindUsers invoked  name = {0}, surname = {1}, birthDayMin = {2}, birthDayMax = {3}, sex = {4}",
                name ?? "null", surname ?? "null",
                birthDayMin == null ? "null" : birthDayMin.ToString(),
                birthDayMax == null ? "null" : birthDayMax.ToString(),
                sex == null ? "null" : sex.ToString());

            Expression<Func<DalUser, bool>> predicate = GetFindPredicate(name, surname, birthDayMin,
                birthDayMax, sex);
            return userRepository.GetAllByPredicate(predicate).ToList().Select(x => x.ToBllUser());
        }

        public IEnumerable<BllUser> GetAllUsers()
        {
            Logger.Trace("UserService.GetAllUsers invoked");

            return userRepository.GetAll().ToList().Select(x => x.ToBllUser());
        }

        private static Expression<Func<DalUser, bool>> GetFindPredicate(string name, string surname,
            DateTime? birthDayMin,
            DateTime? birthDayMax, BllSex? sex)
        {
            ParameterExpression parametr = Expression.Parameter(typeof (DalUser), "dalUser");
            Expression body = Expression.Constant(true, typeof (bool));

            if (!String.IsNullOrEmpty(name))
                body = AddFindPredicateExpression(name, typeof (String), body, parametr, "Name",
                    Expression.Equal);
            if (!String.IsNullOrEmpty(surname))
                body = AddFindPredicateExpression(surname, typeof (String), body, parametr,
                    "Surname",
                    Expression.Equal);
            if (sex != null)
                body = AddFindPredicateExpression(sex.ToDalSex(), typeof (DalSex?), body, parametr,
                    "Sex", Expression.Equal);
            if (birthDayMin != null)
                body = AddFindPredicateExpression(birthDayMin, typeof (DateTime?), body, parametr,
                    "BirthDay",
                    Expression.GreaterThanOrEqual);
            if (birthDayMax != null)
                body = AddFindPredicateExpression(birthDayMax, typeof (DateTime?), body, parametr,
                    "BirthDay",
                    Expression.LessThanOrEqual);

            Expression<Func<DalUser, bool>> predicate =
                Expression.Lambda<Func<DalUser, bool>>(
                    body, parametr);
            return predicate;
        }


        private static Expression AddFindPredicateExpression(object value, Type valuType,
            Expression body,
            ParameterExpression parametr, string propertyName,
            Func<Expression, Expression, BinaryExpression> compareRule)
        {
            return Expression.And(body,
                compareRule(
                    Expression.MakeMemberAccess(parametr, typeof (DalUser).GetProperty(propertyName)),
                    Expression.Constant(value, valuType)));
        }



        public bool IsUserExists(string userName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            Logger.Trace("UserService.IsUserExists invoked userName = {0}", userName);

            return userRepository.GetByName(userName) != null;
        }

        public bool IsUserExists(int id)
        {
            Logger.Trace("UserService.IsUserExists invoked id = {0}", id);

            return userRepository.GetById(id) != null;
        }

        public void Delete(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("UserService.Delete invoked id = {0}", e.Id);

            userRepository.Delete(e.ToDalUser());
            uow.Commit();
        }

        public void Update(BllUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("UserService.Update invoked id = {0}", e.Id);

            userRepository.Update(e.ToDalUser());
            uow.Commit();
        }

        public void SetUserAvatar(int userId, Stream avatarStream)
        {
            if (avatarStream == null) throw new ArgumentNullException("avatarStream");
            Logger.Trace("UserService.SetUserAvatar invoked userId = {0}", userId);

            avatarRepository.Update(new DalAvatar(){UserId = userId, ImageStream = avatarStream});
            uow.Commit();
        }

        public Stream GetUserAvatarStream(int userId)
        {
            Logger.Trace("UserService.GetUserAvatarStream invoked userId = {0}", userId);

            return avatarRepository.GetById(userId).ImageStream;
        }

        public bool CanUserAddToFriends(int userId, int friendId)
        {
            Logger.Trace("UserService.CanUserAddToFriends invoked userId = {0}, friendId = {1}", userId, friendId);

            BllUser user = GetById(userId);
            if (user == null) return false;
            return !user.FriendsId.Contains(friendId) && userId != friendId;
        }

        public bool CanUserWriteMesage(int targetId, int senderId)
        {
            Logger.Trace("UserService.CanUserWrieMesage invoked targetId = {0}, senderId = {1}", targetId, senderId);

            return !CanUserAddToFriends(targetId, senderId) && targetId != senderId;
        }
    }
}
