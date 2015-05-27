using System;

namespace SocialNetwork.Dal.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
