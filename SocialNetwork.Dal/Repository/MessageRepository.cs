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
    public class MessageRepository : IRepository<DalMessage>
    {

        private readonly DbContext context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MessageRepository(DbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<DalMessage> GetAll()
        {
            Logger.Trace("MessageRepository.GetAll");
            return context.Set<Message>().Select(MessageMapper.ToDalMesaageConvertion);
        }

        public DalMessage GetById(int key)
        {
            Logger.Trace("MessageRepository.GetById invoked key = {0}", key);

            Message ormMessage = context.Set<Message>().FirstOrDefault(x => x.Id == key);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public DalMessage GetByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("MessageRepository.GetByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            Message ormMessage = context.Set<Message>().FirstOrDefault(convertedPredicate);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public IQueryable<DalMessage> GetAllByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            Logger.Trace("MessageRepository.GetAllByPredicate invoked predicate = {0}", predicate.ToString());

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            return context.Set<Message>().Where(convertedPredicate).Select(MessageMapper.ToDalMesaageConvertion);
        }
        
        public DalMessage Create(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Create invoked sender = {0}, target = {1}", e.SenderId, e.TargetId);

            Message ormMessage = e.ToOrmMessage();
            context.Set<Message>().Add(ormMessage);
            return ormMessage.ToDalMessage();
        }

        public void Delete(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Delete invoked id = {0}", e.Id);

            context.Set<Message>().Remove(e.ToOrmMessage());
        }

        public void Update(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");
            Logger.Trace("MessageRepository.Update invoked id = {0}", e.Id);

            context.Set<Message>().AddOrUpdate(e.ToOrmMessage());
        }

       

       
    }
}
