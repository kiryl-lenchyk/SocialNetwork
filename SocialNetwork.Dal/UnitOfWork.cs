using System;
using System.Data;
using System.Data.Entity;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Logger.Interface;

namespace SocialNetwork.Dal
{
    /// <summary>
    /// Represent IUnitOfWork for Entity Framework 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private readonly DbContext context;
        private readonly ILogger logger;

        #endregion

        #region Constractor
        
        /// <summary>
        /// Create new instanse of UnitOfWork
        /// </summary>
        /// <param name="context">DbContext for commit</param>
        /// <param name="logger">class for log</param>
        public UnitOfWork(DbContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// End transaction and commit.
        /// </summary>
        public void Commit()
        {
            logger.Log(LogLevel.Trace,"UnitOfWork.Commit ivoked");
            try
            {
                context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                logger.Log(LogLevel.Error,"UnitOfWork.Commit cant save data to database exception: {0}",
                    ex.ToString());
                throw new DataException("Can't save data to database",ex);
            }

        }

        #endregion
    }
}
