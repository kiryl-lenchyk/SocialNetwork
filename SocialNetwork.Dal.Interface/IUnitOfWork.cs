using System;

namespace SocialNetwork.Dal.Interface
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
