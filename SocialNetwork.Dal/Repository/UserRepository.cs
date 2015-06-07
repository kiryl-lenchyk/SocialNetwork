﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

        private bool isDisposed;
        private static readonly string AvatarLocation = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "{0}Avatars{0}{1}.png";
        private static readonly String DefaultAvatar = "noavatar";

        public UserRepository(DbContext context)
        {
            isDisposed = false;
            this.context = context;
        }

        public IEnumerable<DalUser> GetAll()
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            foreach (User user in context.Set<User>())
            {
                yield return user.ToDalUser();
            }
        }

        public DalUser GetById(int key)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == key);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public DalUser GetByName(String name)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.UserName == name);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public void AddToFriends(DalUser currentUser, DalUser newFriend)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

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
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            User ormCurrentUser = GetOrmUserWithFriends(currentUser);
            User ormNewFriend = GetOrmUserWithFriends(newFriend);
            ormCurrentUser.Friends.Remove(ormNewFriend);
            ormNewFriend.Friends.Remove(ormCurrentUser);
        }

        public void SetUserAvatar(int userId, Stream avatarStream)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            using (Bitmap avatar = new Bitmap(avatarStream))
            {
                avatar.Save(string.Format(AvatarLocation, Path.DirectorySeparatorChar, userId),
                    ImageFormat.Png);
            }

        }

        public Stream GetUserAvatarStream(int userId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            if (!File.Exists(string.Format(AvatarLocation,
                Path.DirectorySeparatorChar, userId))) return null;

            MemoryStream avatarStream = new MemoryStream();
            using (Bitmap avatar = new Bitmap(string.Format(AvatarLocation,
                    Path.DirectorySeparatorChar, userId)))
            {
                avatar.Save(avatarStream, ImageFormat.Png);
                avatarStream.Seek(0, SeekOrigin.Begin);
            }

            return avatarStream;
        }

        public Stream GetDefaultAvatarStream()
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");
            
            MemoryStream avatarStream = new MemoryStream();
            using (Bitmap avatar = new Bitmap(string.Format(AvatarLocation,
                    Path.DirectorySeparatorChar, DefaultAvatar)))
            {
                avatar.Save(avatarStream, ImageFormat.Png);
                avatarStream.Seek(0, SeekOrigin.Begin);
            }

            return avatarStream;
        }

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public IEnumerable<DalUser> GetAllByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            return context.Set<User>().Where( convertedPredicate).ToList().Select(x => x.ToDalUser());
        }

        public DalUser Create(DalUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            User ormUser = e.ToOrmUser();
            context.Set<User>().Add(ormUser);
            return ormUser.ToDalUser();
        }

        public void Delete(DalUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == e.Id);
            if (ormUser == null) throw new ArgumentException("User has incorrect id");
            context.Set<User>().Remove(ormUser);
        }

        public void Update(DalUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            context.Set<User>().AddOrUpdate(e.ToOrmUser());
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                context.Dispose();
                isDisposed = true;
            }
        }  
        
    }
}
