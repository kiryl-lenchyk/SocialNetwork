using System;
using System.Data;
using System.Data.Entity;
using SocialNetwork.Dal.Interface;

namespace SocialNetwork.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            try
            {
                context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                //TODO: Log
                throw new DataException("Can't save data to database",ex);
            }
            
        }

    }
}
