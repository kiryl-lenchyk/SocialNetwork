using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        private bool isDisposed;

        public MessageRepository(DbContext context)
        {
            isDisposed = false;
            this.context = context;
        }
        
        public IEnumerable<DalMessage> GetAll()
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            foreach (Message message in context.Set<Message>())
            {
                yield return message.ToDalMessage();
            }
        }

        public DalMessage GetById(int key)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            Message ormMessage = context.Set<Message>().FirstOrDefault(x => x.Id == key);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public DalMessage GetByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            Message ormMessage = context.Set<Message>().FirstOrDefault(convertedPredicate);
            return ormMessage != null ? ormMessage.ToDalMessage() : null;
        }

        public IEnumerable<DalMessage> GetAllByPredicate(Expression<Func<DalMessage, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            Expression<Func<Message, bool>> convertedPredicate =
                (Expression<Func<Message, bool>>)(new MessageExpressionMapper().Visit(predicate));

            return context.Set<Message>().Where(convertedPredicate).ToList().Select(x => x.ToDalMessage());
        }

        public DalMessage Create(DalMessage e)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            Message ormMessage = e.ToOrmMessage();
            context.Set<Message>().Add(ormMessage);
            return ormMessage.ToDalMessage();
        }

        public void Delete(DalMessage e)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            context.Set<Message>().Remove(e.ToOrmMessage());
        }

        public void Update(DalMessage e)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageRepository");

            context.Set<Message>().AddOrUpdate(e.ToOrmMessage());
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
