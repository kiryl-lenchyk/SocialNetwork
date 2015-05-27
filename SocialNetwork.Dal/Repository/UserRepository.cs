using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
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

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> predicate)
        {
            if (isDisposed) throw new ObjectDisposedException("UserRepository");

            Expression<Func<User, DalUser>> convert = user => user.ToDalUser();
            var param = Expression.Parameter(typeof(User), "user");
            var body = Expression.Invoke(predicate,
                  Expression.Invoke(convert, param));
            var convertedPredicate = Expression.Lambda<Func<User, bool>>(body, param);

            User ormUser = context.Set<User>().FirstOrDefault(convertedPredicate);
            return ormUser != null ? ormUser.ToDalUser() : null;
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

            context.Set<User>().Remove(e.ToOrmUser());
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
