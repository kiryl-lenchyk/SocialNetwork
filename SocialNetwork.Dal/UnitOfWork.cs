using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using SocialNetwork.Dal.Interface;

namespace SocialNetwork.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        private bool isDisposed = false;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public void Commit()
        {
            if (isDisposed) throw new ObjectDisposedException("UnitOfWork");

            context.SaveChanges();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                context.SaveChanges();
                context.Dispose();
                isDisposed = true;
            }
        }
    }
}
