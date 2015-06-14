using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
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

        public MessageRepository(DbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<DalMessage> GetAll()
        {
            return context.Set<Message>().Select(MessageMapper.ToDalMesaageConvertion);
        }

        public DalMessage GetById(int key)
        {
            Message ormMessage = context.Set<Message>().FirstOrDefault(x => x.Id == key);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public DalMessage GetByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            Message ormMessage = context.Set<Message>().FirstOrDefault(convertedPredicate);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public IQueryable<DalMessage> GetAllByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            return context.Set<Message>().Where(convertedPredicate).Select(MessageMapper.ToDalMesaageConvertion);
        }
        
        public DalMessage Create(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");

            Message ormMessage = e.ToOrmMessage();
            context.Set<Message>().Add(ormMessage);
            return ormMessage.ToDalMessage();
        }

        public void Delete(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");

            context.Set<Message>().Remove(e.ToOrmMessage());
        }

        public void Update(DalMessage e)
        {
            if (e == null) throw new ArgumentNullException("e");

            context.Set<Message>().AddOrUpdate(e.ToOrmMessage());
        }

       

       
    }
}
