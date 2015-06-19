using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

    /// <summary>
    /// Represent MessageRepository for database as storage.
    /// </summary>
    public class MessageRepository : IRepository<DalMessage>
    {

        #region Fields

        private readonly DbContext context;
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instanse of MessageRepository.
        /// </summary>
        /// <param name="context">DbContext for save data</param>
        public MessageRepository(DbContext context)
        {
            this.context = context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all elements in storage. Return IQueryable for wroting long query to storage.
        /// </summary>
        /// <returns>IQuaryable of all elements. You can add LINQ query to it. Quary will be invoked by storage</returns>
        public IQueryable<DalMessage> GetAll()
        {
            Logger.Trace("MessageRepository.GetAll");
            return context.Set<Message>().Select(MessageMapper.ToDalMesaageConvertion);
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="key">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        public DalMessage GetById(int key)
        {
            Logger.Trace("MessageRepository.GetById invoked key = {0}", key);

            Message ormMessage = context.Set<Message>().FirstOrDefault(x => x.Id == key);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        /// <summary>
        /// Get entity by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>first founded entity or null if it not found.</returns>
        public DalMessage GetByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("MessageRepository.GetByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            Message ormMessage = context.Set<Message>().FirstOrDefault(convertedPredicate);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        /// <summary>
        ///  Get all entites by search predicate.
        /// </summary>
        /// <param name="predicate">predicate to search.</param>
        /// <returns>IQueryable of entites, you can write long additional query to storage</returns>
        public IQueryable<DalMessage> GetAllByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("MessageRepository.GetAllByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            return context.Set<Message>().Where(convertedPredicate).Select(MessageMapper.ToDalMesaageConvertion);
        }

        /// <summary>
        /// Add entity to storage. Id will be selected by storage.
        /// </summary>
        /// <param name="e">new entity without id.</param>
        /// <returns>created entity with new id.</returns>
        public DalMessage Create(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Create invoked sender = {0}, target = {1}", e.SenderId, e.TargetId);

            Message ormMessage = e.ToOrmMessage();
            context.Set<Message>().Add(ormMessage);
            return ormMessage.ToDalMessage();
        }

        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        public void Delete(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Delete invoked id = {0}", e.Id);

            context.Set<Message>().Remove(e.ToOrmMessage());
        }

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="e">new value for entity.</param>
        public void Update(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Update invoked id = {0}", e.Id);

            context.Set<Message>().AddOrUpdate(e.ToOrmMessage());
        }

        #endregion

    }
}
