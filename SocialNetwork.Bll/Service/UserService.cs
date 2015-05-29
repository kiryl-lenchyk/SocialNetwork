using System;
using System.Linq;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;

namespace SocialNetwork.Bll.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;
        private bool isDisposed;


        public UserService(IUnitOfWork uow, IUserRepository repository)
        {
            isDisposed = false;
            this.uow = uow;
            this.userRepository = repository;
        }

        public BllUser GetById(int key, int currentUserId)
        {
            if(isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = userRepository.GetById(key);
            return dalUser == null ? null : dalUser.ToBllUser(currentUserId);
        }

        public BllUser GetByName(string name, int currentUserId)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = userRepository.GetByName(name);
            return dalUser == null ? null : dalUser.ToBllUser(currentUserId);
        }

        public BllUser Create(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            DalUser dalUser = e.ToDalUser();
            dalUser = userRepository.Create(dalUser);
            uow.Commit();
            return dalUser == null ? null : dalUser.ToBllUser();
        }

        public bool IsUserExists(string userName)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            return userRepository.GetByName(userName) != null;
        }

        public void Delete(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            userRepository.Delete(e.ToDalUser());
            uow.Commit();
        }

        public void Update(BllUser e)
        {
            if (isDisposed) throw new ObjectDisposedException("UserService");

            userRepository.Update(e.ToDalUser());
            uow.Commit();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                uow.Dispose();
                userRepository.Dispose();
            }

        }
    }
}
