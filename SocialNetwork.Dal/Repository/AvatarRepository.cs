using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Logger.Interface;

namespace SocialNetwork.Dal.Repository
{
    /// <summary>
    /// Represent AvatarRepository for file system as storage.
    /// </summary>
    public class AvatarRepository : IRepository<DalAvatar>
    {

        #region Fields

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
        /// <param name="logger">class for log</param>
        public AvatarRepository(ILogger logger)
        {
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

            return GetByPredicateEnumerable(x => true).AsQueryable();
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        public DalAvatar GetById(int id)
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.GetById invoked key = {0}", id);
            string fullAvatarPath = string.Format("{0}//{1}", AvatarsLocation,
                string.Format(AvatarNameMask, id));

            if (!File.Exists(fullAvatarPath))
            {
                logger.Log(LogLevel.Trace,"UserRepository.Avatar for user id = {0} not found", id);
                return GetDefaultAvatar(id);
            }

            try
            {
                return LoadAvatarFromPath(id, fullAvatarPath);
            }
            catch (ArgumentException ex)
            {
                logger.Log(LogLevel.Error,
                    "AvatarRepository.GetUserAvatarStream cant load avatar image from {0} userId = {1} exception: {2}",
                    fullAvatarPath, id, ex.ToString());
                throw new DataException("Can't load user avatar", ex);
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

            return GetByPredicateEnumerable(predicate.Compile()).FirstOrDefault();
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

            return GetByPredicateEnumerable(predicate.Compile()).AsQueryable();
        }

        /// <summary>
        /// Add entity to storage. Id will be selected by storage.
        /// </summary>
        /// <param name="e">new entity without id.</param>
        /// <returns>created entity with new id.</returns>
        public DalAvatar Create(DalAvatar e)
        {
            if (e == null) throw new ArgumentNullException("e");
            if (e.ImageStream == null)
                throw new ArgumentException("e.ImageStream cant be null", "e");
            logger.Log(LogLevel.Trace,"UserRepository.Create invoked id = {0}", e.Id);
            string fullAvatarPath = string.Format("{0}//{1}", AvatarsLocation,
                string.Format(AvatarNameMask, e.UserId));

            try
            {
                return SaveAvatarToPath(e, fullAvatarPath);
            }
            catch (ArgumentException)
            {
                throw new InvalidDataException("Stream dosn't contain image");
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                logger.Log(LogLevel.Error,
                    "UserRepository.SetUserAvatar cant save avatar image to {0} userId = {1} exception: {2}",
                    fullAvatarPath, e.UserId, ex.ToString());
                throw new DataException("Can't save user avatar", ex);
            }
        }
        
        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        public void Delete(DalAvatar e)
        {
            logger.Log(LogLevel.Trace,"AvatarRepository.Delete invoked key = {0}", e.Id);
            string fullAvatarPath = string.Format("{0}//{1}", AvatarsLocation,
                string.Format(AvatarNameMask, e.UserId));

            if (!File.Exists(fullAvatarPath))
            {
                logger.Log(LogLevel.Trace,"UserRepository.Delete avatar for user id = {0} not found", e.Id);
                return;
            }

            try
            {
                File.Delete(fullAvatarPath);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,
                    "AvatarRepository.GetUserAvatarStream cant delete avatar image from {0} userId = {1} exception: {2}",
                    fullAvatarPath, e.Id, ex.ToString());
                throw new DataException("Can't delete user avatar", ex);
            }
        }

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="entity">new value for entity.</param>
        public void Update(DalAvatar entity)
        {
            Create(entity);
        }

        #endregion

        #region Private Methods

        private IEnumerable<DalAvatar> GetByPredicateEnumerable(Func<DalAvatar, bool> predicate)
        {
            Regex avatarNameRegex = GetAvatarNameRegex();

            foreach (string file in Directory.EnumerateFiles(AvatarsLocation))
            {
                Match avatarNameMatch = avatarNameRegex.Match(file);
                if (avatarNameMatch.Success)
                {
                    int id = Int32.Parse(avatarNameMatch.Groups[1].ToString());
                    if (predicate(new DalAvatar() {Id = id, UserId = id}))
                        yield return GetById(id);
                }
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

        private static Regex GetAvatarNameRegex()
        {
            return new Regex(AvatarNameMask.Replace("{0}", "(\\d+)")
                .Replace("\\", "\\\\")
                .Replace(".", "\\."));
        }

        private static DalAvatar LoadAvatarFromPath(int id, string fullAvatarPath)
        {
            MemoryStream avatarStream = new MemoryStream();
            using (Bitmap avatar = new Bitmap(fullAvatarPath))
            {
                avatar.Save(avatarStream, ImageFormat.Png);
                avatarStream.Seek(0, SeekOrigin.Begin);
            }

            return new DalAvatar() {Id = id, ImageStream = avatarStream, UserId = id};
        }

        private static DalAvatar SaveAvatarToPath(DalAvatar e, string fullAvatarPath)
        {
            using (Bitmap avatar = new Bitmap(e.ImageStream))
            {
                avatar.Save(fullAvatarPath, ImageFormat.Png);
            }
            e.Id = e.UserId;
            return e;
        }

        #endregion
    }
}