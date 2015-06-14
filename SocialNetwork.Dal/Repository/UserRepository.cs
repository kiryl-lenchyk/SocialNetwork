using System;
using System.Data;
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

        private static readonly string AvatarLocation = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "{0}Avatars{0}{1}.png";
        private static readonly String DefaultAvatar = "noavatar";

        public UserRepository(DbContext context)
        {
            this.context = context;
        }

        public IQueryable<DalUser> GetAll()
        {
            return context.Set<User>().Select(UserMapper.ToDalUserConvertion);
        }

        public DalUser GetById(int key)
        {
            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == key);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public DalUser GetByName(String name)
        {
            if(name == null) throw new ArgumentNullException("name");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.UserName == name);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public void AddToFriends(DalUser currentUser, DalUser newFriend)
        {
            if(currentUser == null) throw new ArgumentNullException("currentUser");
            if (newFriend == null) throw new ArgumentNullException("newFriend");

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


            User ormCurrentUser = GetOrmUserWithFriends(currentUser);
            User ormNewFriend = GetOrmUserWithFriends(newFriend);
            ormCurrentUser.Friends.Remove(ormNewFriend);
            ormNewFriend.Friends.Remove(ormCurrentUser);
        }

        public void SetUserAvatar(int userId, Stream avatarStream)
        {
            if (avatarStream == null) throw new ArgumentNullException("avatarStream");
            
            try
            {
                using (Bitmap avatar = new Bitmap(avatarStream))
                {
                    avatar.Save(string.Format(AvatarLocation, Path.DirectorySeparatorChar, userId),
                        ImageFormat.Png);
                }
            }
            catch (ArgumentException)
            {
                throw new InvalidDataException("Stream dosn't contain image");
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                //TODO: Log
                throw new DataException("Can't save user avatar", ex);
            }


        }

        public Stream GetUserAvatarStream(int userId)
        {
            if (!File.Exists(string.Format(AvatarLocation,
                Path.DirectorySeparatorChar, userId))) return null;

            try
            {
                MemoryStream avatarStream = new MemoryStream();
                using (Bitmap avatar = new Bitmap(string.Format(AvatarLocation,
                    Path.DirectorySeparatorChar, userId)))
                {
                    avatar.Save(avatarStream, ImageFormat.Png);
                    avatarStream.Seek(0, SeekOrigin.Begin);
                }

                return avatarStream;
            }
            catch (FileNotFoundException ex)
            {

                throw new DataException("Can't load user avatar", ex);
            }

        }

        public Stream GetDefaultAvatarStream()
        {
            try
            {
                MemoryStream avatarStream = new MemoryStream();
                using (Bitmap avatar = new Bitmap(string.Format(AvatarLocation,
                    Path.DirectorySeparatorChar, DefaultAvatar)))
                {
                    avatar.Save(avatarStream, ImageFormat.Png);
                    avatarStream.Seek(0, SeekOrigin.Begin);
                }

                return avatarStream;
            }
            catch (FileNotFoundException ex)
            {

                throw new DataException("Can't load user avatar", ex);
            }

        }

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
            return ormUser != null ? ormUser.ToDalUser() : null;
        }

        public IQueryable<DalUser> GetAllByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new UserExpressionMapper().Visit(predicate));

            return context.Set<User>().Where( convertedPredicate).Select(UserMapper.ToDalUserConvertion);
        }

        public DalUser Create(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");

            User ormUser = e.ToOrmUser();
            context.Set<User>().Add(ormUser);
            return ormUser.ToDalUser();
        }

        public void Delete(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");

            User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == e.Id);
            if (ormUser == null) throw new ArgumentException("User has incorrect id");
            context.Set<User>().Remove(ormUser);
        }

        public void Update(DalUser e)
        {
            if (e == null) throw new ArgumentNullException("e");

            context.Set<User>().AddOrUpdate(e.ToOrmUser());
        }

        
        
    }
}
