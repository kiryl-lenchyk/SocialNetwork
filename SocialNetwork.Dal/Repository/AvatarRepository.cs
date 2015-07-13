using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
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
using SocialNetwork.Logger.Interface;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Repository
{
    /// <summary>
    /// Represent AvatarRepository for file system as storage.
    /// </summary>
    public class AvatarRepository : IRepository<DalAvatar>
    {

        #region Fields

        private readonly DbContext context;
        private readonly ILogger logger;

        private static readonly string AvatarsLocation = AppDomain.CurrentDomain.GetData(
            "DataDirectory").ToString()
                                                         +
                                                         ConfigurationManager.AppSettings[
                                                             "AvatarsLocation"];

        private static readonly string AvatarNameMask =
            ConfigurationManager.AppSettings["AvatarNameMask"];

        private static readonly string DefaultAvatar =
            ConfigurationManager.AppSettings["DefaultAvatarId"];

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instanse of AvatarRepository.
        /// </summary>
        /// <param name="context">DbContext for save data</param>
        /// <param name="logger">class for log</param>
        public AvatarRepository(DbContext context, ILogger logger)
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
        public IQueryable<DalAvatar> GetAll()
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.GetAll invoked");

            try
            {
                return context.Set<User>().Select(AvatarMapper.ToDalAvatarConvertion);
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        public DalAvatar GetById(int id)
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.GetById invoked key = {0}", id);

            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == id);
                return GetUserAvatar(id, ormUser);
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
        public DalAvatar GetByPredicate(Expression<Func<DalAvatar, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace,"AvatarRepository.GetByPredicate invoked predicate = {0}",
                predicate.ToString());


            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new AvatarExpressionMapper().Visit(predicate));
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
                return ormUser != null ? ormUser.ToDalAvatar() : null;
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
        public IQueryable<DalAvatar> GetAllByPredicate(Expression<Func<DalAvatar, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            logger.Log(LogLevel.Trace,"AvatarRepository.GetAllByPredicate invoked predicate = {0}",
                predicate.ToString());

            Expression<Func<User, bool>> convertedPredicate =
                (Expression<Func<User, bool>>)(new AvatarExpressionMapper().Visit(predicate));
            try
            {
                return context.Set<User>().Where(convertedPredicate).Select(AvatarMapper.ToDalAvatarConvertion);
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
        public DalAvatar Create(DalAvatar e)
        {
            if (e == null) throw new ArgumentNullException("e");
            if (e.ImageBytes == null)
                throw new ArgumentException("e.ImageStream cant be null", "e");
            logger.Log(LogLevel.Trace,"UserRepository.Create invoked id = {0}", e.Id);

            return SetUserAvatar(e);
        }

        

        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        public void Delete(DalAvatar e)
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.Delete invoked key = {0}", e.Id);

            e.ImageBytes = null;
            SetUserAvatar(e);
        }

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="entity">new value for entity.</param>
        public void Update(DalAvatar entity)
        {
            logger.Log(LogLevel.Trace, "AvatarRepository.Update invoked key = {0}", entity.Id);

            SetUserAvatar(entity);
        }

        #endregion

        #region Private Methods

        private DalAvatar GetUserAvatar(int id, User ormUser)
        {
            if (ormUser == null) return null;
            if (ormUser.Avatar == null)
            {
                logger.Log(LogLevel.Trace, "UserRepository.Avatar for user id = {0} not found", id);
                return GetDefaultAvatar(id);
            }
            return ormUser.ToDalAvatar();
        }

        private DalAvatar SetUserAvatar(DalAvatar e)
        {
            try
            {
                User ormUser = context.Set<User>().FirstOrDefault(x => x.Id == e.UserId);
                if (ormUser == null) throw new ArgumentException("Avatar has incorrect UserId");
                ormUser.Avatar = e.ImageBytes;
                context.Set<User>().AddOrUpdate(ormUser);
                return ormUser.ToDalAvatar();
            }
            catch (EntityException ex)
            {
                throw new DataException("Can't access to database", ex);
            }
        }

        
        private DalAvatar GetDefaultAvatar(int userId)
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.GetDefaultAvatarStream invoked");
            string fullAvatarPath = string.Format("{0}//{1}", AvatarsLocation,
                string.Format(AvatarNameMask, DefaultAvatar));

            try
            {
                return LoadAvatarFromPath(userId, fullAvatarPath);
            }
            catch (FileNotFoundException ex)
            {
                logger.Log(LogLevel.Error,
                    "UserRepository.GetDefaultAvatarStream cant load avatar image from {0} exception: {1}",
                    fullAvatarPath, ex.ToString());
                throw new DataException("Can't load user avatar", ex);
            }

        }

        
        private static DalAvatar LoadAvatarFromPath(int id, string fullAvatarPath)
        {
            MemoryStream avatarStream = new MemoryStream();
            using (Bitmap avatar = new Bitmap(fullAvatarPath))
            {
                avatar.Save(avatarStream, ImageFormat.Png);
                avatarStream.Seek(0, SeekOrigin.Begin);
            }

            return new DalAvatar() {Id = id, ImageBytes = avatarStream.GetBuffer(), UserId = id};
        }
        
        #endregion
    }
}