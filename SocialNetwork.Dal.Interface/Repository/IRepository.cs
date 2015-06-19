using System;
using System.Linq;
using System.Linq.Expressions;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    /// <summary>
    /// Represent storage of T.
    /// </summary>
    /// <typeparam name="TEntity">Type of storage elements.</typeparam>
    public interface IRepository<TEntity>  where TEntity : IEntity
    {
        /// <summary>
        /// Get all elements in storage. Return IQueryable for wroting long query to storage.
        /// </summary>
        /// <returns>IQuaryable of all elements. You can add LINQ query to it. Quary will be invoked by storage</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="key">entity id.</param>
        /// <returns>found entity or null if it not found.</returns>
        TEntity GetById(int key);

        /// <summary>
        /// Get entity by search predicate.
        /// </summary>
        /// <param name="f">predicate to search.</param>
        /// <returns>first founded entity or null if it not found.</returns>
        TEntity GetByPredicate(Expression<Func<TEntity, bool>> f);

        /// <summary>
        ///  Get all entites by search predicate.
        /// </summary>
        /// <param name="f">predicate to search.</param>
        /// <returns>IQueryable of entites, you can write long additional query to storage</returns>
        IQueryable<TEntity> GetAllByPredicate(Expression<Func<TEntity, bool>> f);

        /// <summary>
        /// Add entity to storage. Id will be selected by storage.
        /// </summary>
        /// <param name="e">new entity without id.</param>
        /// <returns>created entity with new id.</returns>
        TEntity Create(TEntity e);

        /// <summary>
        /// Delete entity from storage by id.
        /// </summary>
        /// <param name="e">entity to delete.</param>
        void Delete(TEntity e);

        /// <summary>
        /// Apdate entity value. Old value selected by id.
        /// </summary>
        /// <param name="entity">new value for entity.</param>
        void Update(TEntity entity);
    }
}
