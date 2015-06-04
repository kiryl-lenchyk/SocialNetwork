using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
        private bool isDisposed;


        public UserService(IUnitOfWork uow, IUserRepository repository)
        {
            isDisposed = false;
            this.uow = uow;
            this.userRepository = repository;
        }

        public BllUser GetById(int key, int currentUserId)
        {
            if(isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = userRepository.GetById(key);
            return dalUser == null ? null : dalUser.ToBllUser(currentUserId);
        }

        public BllUser GetByName(string name, int currentUserId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = userRepository.GetByName(name);
            return dalUser == null ? null : dalUser.ToBllUser(currentUserId);
        }

        public BllUser Create(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = e.ToDalUser();
            dalUser = userRepository.Create(dalUser);
            uow.Commit();
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        public void AddFriend(int currentUserId, int newFriendId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");


            DalUser currentUser = userRepository.GetById(currentUserId);
            if (currentUser == null) throw new ArgumentException(String.Format("User id = {0} is no existst", currentUserId), "currentUserId");
            DalUser newFriend = userRepository.GetById(newFriendId);
            if (newFriend == null) throw new ArgumentException(String.Format("User id = {0} is no existst", newFriendId),"newFriendId");
            if(currentUser.FriendsId.Contains(newFriendId)) throw new InvalidOperationException("Users are friends");
            
            userRepository.AddToFriends(currentUser, newFriend);
            uow.Commit();
        }

        public void RemoveFriend(int currentUserId, int newFriendId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");
            
            DalUser currentUser = userRepository.GetById(currentUserId);
            if (currentUser == null) throw new ArgumentException(String.Format("User id = {0} is no existst", currentUserId), "currentUserId");
            DalUser newFriend = userRepository.GetById(newFriendId);
            if (newFriend == null) throw new ArgumentException(String.Format("User id = {0} is no existst", newFriendId), "newFriendId");
            if (!currentUser.FriendsId.Contains(newFriendId)) throw new InvalidOperationException("Users are not friends");

            userRepository.RemoveFriend(currentUser, newFriend);
            uow.Commit();
        }

        public IEnumerable<BllUser> FindUsers(string name, string surname, DateTime? birthDayMin, DateTime? birthDayMax,
            BllSex? sex)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            Expression<Func<DalUser, bool>> predicate = GetFindPredicate(name, surname, birthDayMin,
                birthDayMax, sex);
            return userRepository.GetAllByPredicate(predicate).Select(x => x.ToBllUser());
        }

        private static Expression<Func<DalUser, bool>> GetFindPredicate(string name, string surname,
            DateTime? birthDayMin,
            DateTime? birthDayMax, BllSex? sex)
        {
            ParameterExpression parametr = Expression.Parameter(typeof (DalUser), "dalUser");
            Expression body = Expression.Constant(true, typeof (bool));

            if (!String.IsNullOrEmpty(name))
                body = AddFindPredicateExpression(name,typeof(String), body, parametr, "Name", Expression.Equal);
            if (!String.IsNullOrEmpty(surname))
                body = AddFindPredicateExpression(surname,typeof(String), body, parametr, "Surname",
                    Expression.Equal);
            if (sex != null)
                body = AddFindPredicateExpression(sex.ToDalSex(),typeof(DalSex?), body, parametr, "Sex", Expression.Equal);
            if (birthDayMin != null)
                body = AddFindPredicateExpression(birthDayMin,typeof(DateTime?), body, parametr, "BirthDay",
                    Expression.GreaterThanOrEqual);
            if (birthDayMax != null)
                body = AddFindPredicateExpression(birthDayMax, typeof(DateTime?), body, parametr, "BirthDay",
                    Expression.LessThanOrEqual);

            Expression<Func<DalUser, bool>> predicate =
                Expression.Lambda<Func<DalUser, bool>>(
                    body, parametr);
            return predicate;
        }


        private static Expression AddFindPredicateExpression(object value, Type valuType, Expression body,
            ParameterExpression parametr, string propertyName, 
            Func<Expression,Expression,BinaryExpression> compareRule )
        {
            return Expression.And(body,
                    compareRule(
                        Expression.MakeMemberAccess(parametr, typeof(DalUser).GetProperty(propertyName)),
                        Expression.Constant(value, valuType)));
        }

      

        public bool IsUserExists(string userName)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            return userRepository.GetByName(userName) != null;
        }

        public bool IsUserExists(int id)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            return userRepository.GetById(id) != null;
        
        }

        public void Delete(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            userRepository.Delete(e.ToDalUser());
            uow.Commit();
        }

        public void Update(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            userRepository.Update(e.ToDalUser());
            uow.Commit();
        }

        public void SetUserAvatar(int userId, Stream avatarStream)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            userRepository.SetUserAvatar(userId,avatarStream);
            uow.Commit();
        }

        public Stream GetUserAvatarStream(int userId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            Stream avatarStream = userRepository.GetUserAvatarStream(userId);
            return avatarStream ?? userRepository.GetDefaultAvatarStream();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                uow.Dispose();
                userRepository.Dispose();
            }

        }
    }
}
