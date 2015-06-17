using System;
using System.Data;
using System.Data.Entity;
using NLog;
using SocialNetwork.Dal.Interface;

namespace SocialNetwork.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

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

    }
}
