using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    public interface IRepository<TEntity>  where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(int key);

        TEntity GetByPredicate(Expression<Func<TEntity, bool>> f);

        IQueryable<TEntity> GetAllByPredicate(Expression<Func<TEntity, bool>> f);

        TEntity Create(TEntity e);

        void Delete(TEntity e);

        void Update(TEntity entity);
    }
}
