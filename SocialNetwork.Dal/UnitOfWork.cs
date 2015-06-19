using System;
using System.Data;
using System.Data.Entity;
using NLog;
using SocialNetwork.Dal.Interface;

namespace SocialNetwork.Dal
{
    /// <summary>
    /// Represent IUnitOfWork for Entity Framework 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly DbContext context;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constractor
        
        /// <summary>
        /// Create new instanse of UnitOfWork
        /// </summary>
        /// <param name="context">DbContext for commit</param>
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// End transaction and commit.
        /// </summary>
        public void Commit()
        {
            Logger.Trace("UnitOfWork.Commit ivoked");
            try
            {
                context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("UnitOfWork.Commit cant save data to database exception: {0}",
                    ex.ToString());
                throw new DataException("Can't save data to database",ex);
            }

        }

        #endregion
    }
}
