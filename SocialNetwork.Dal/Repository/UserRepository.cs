﻿using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
    public class UserRepository : IUserRepository
    {

        private readonly DbContext context;

        private static readonly string AvatarLocation = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + ConfigurationManager.AppSettings["AvatarPathMask"];
        private static readonly string DefaultAvatar = ConfigurationManager.AppSettings["DefaultAvatarId"];
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UserRepository(DbContext context)
        {
            this.context = context;
        }

        public IQueryable<DalUser> GetAll()
        {
            Logger.Trace("UserRepository.GetAll ivoked");
            return context.Set<User>().Select(UserMapper.ToDalUserConvertion);
        }

        public DalUser GetById(int key)
        {
            Logger.Trace("UserRepository.GetById invoked key = {0}", key);

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == key);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public DalUser GetByName(String name)
        {
            if(name == null) throw new ArgumentNullException("name");
            Logger.Trace("UserRepository.GetByName invoked name = {0}", name);

            User ormUser = context.Set<User>().FirstOrDefault(x => x.UserName == name);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public void AddToFriends(DalUser currentUser, DalUser newFriend)
        {
            if(currentUser == null) throw new ArgumentNullException("currentUser");
            if (newFriend == null) throw new ArgumentNullException("newFriend");
            Logger.Trace("UserRepository.AddToFriends invoked currentUser = {0}, newFriend = {1} ", currentUser, newFriend);

            User ormCurrentUser = GetOrmUserWithFriends(currentUser);
            User ormNewFriend = GetOrmUserWithFriends(newFriend);
            ormCurrentUser.Friends.Add(ormNewFriend);
            ormNewFriend.Friends.Add(ormCurrentUser);
        }

        private User GetOrmUserWithFriends(DalUser dalUser)
        {
            User ormCurrentUser = context.Set<User>().SingleOrDefault(x => x.Id == dalUser.Id);
            if (ormCurrentUser == null) throw new ArgumentException("User has incorrect id");

            context.Entry(ormCurrentUser).Collection(x => x.Friends).Load();
            return ormCurrentUser;
        }

        public void RemoveFriend(DalUser currentUser, DalUser newFriend)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            if (newFriend == null) throw new ArgumentNullException("newFriend");
            Logger.Trace("UserRepository.AddToFriends invoked currentUser = {0}, newFriend = {1} ", currentUser, newFriend);

            User ormCurrentUser = GetOrmUserWithFriends(currentUser);
            User ormNewFriend = GetOrmUserWithFriends(newFriend);
            ormCurrentUser.Friends.Remove(ormNewFriend);
            ormNewFriend.Friends.Remove(ormCurrentUser);
        }

       
        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("UserRepository.GetByPredicate invoked predicate = {0}",predicate.ToString());

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public IQueryable<DalUser> GetAllByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("UserRepository.GetAllByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            return context.Set<User>().Where( convertedPredicate).Select(UserMapper.ToDalUserConvertion);
        }

        public DalUser Create(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("UserRepository.Create invoked userName = {0}",e.UserName);

            User ormUser = e.ToOrmUser();
            context.Set<User>().Add(ormUser);
            return ormUser.ToDalUser();
        }

        public void Delete(DalUser e)
        {
            Logger.Trace("UserRepository.Delete invoked id = {0}", e.Id);

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == e.Id);
            if (ormUser == null) throw new ArgumentException("User has incorrect id");
            context.Set<User>().Remove(ormUser);
        }

        public void Update(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("UserRepository.Update invoked id = {0}", e.Id);

            context.Set<User>().AddOrUpdate(e.ToOrmUser());
        }

        
        
    }
}
