using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;

namespace SocialNetwork.Bll.Service
{
    public class MessageService : IMessageService
    {
      
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalMessage> messageRepository;
        private readonly IUserRepository userRepository;
        private bool isDisposed;


        public MessageService(IUnitOfWork uow, IRepository<DalMessage> messageRepository, IUserRepository userRepository)
        {
            isDisposed = false;
            this.uow = uow;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<BllDialog> GetUserDialogs(int userId)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageService");
            
            DalUser currentDalUser = userRepository.GetById(userId);
            if(currentDalUser == null) throw  new ArgumentException(string.Format("User with id = {0} not found", userId));
            BllUser currentBllUser = currentDalUser.ToBllUser();

            return messageRepository.GetAllByPredicate(x => x.SenderId == userId)
                .Select(x => x.TargetId)
                .Union(
                    messageRepository.GetAllByPredicate(x => x.TargetId == userId)
                        .Select(x => x.SenderId))
                .Select(x => GetUsersDialog(currentBllUser, userRepository.GetById(x).ToBllUser()));
        }

        public BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser)
        {
            if (isDisposed) throw new ObjectDisposedException("MessageService");

            return new BllDialog
            {
                FirstUser = firstUser,
                SecondUser = secondUser,
                Messages = messageRepository.GetAllByPredicate(
                    x =>
                        x.SenderId == firstUser.Id && x.TargetId == secondUser.Id ||
                        x.TargetId == firstUser.Id && x.SenderId == secondUser.Id)
                    .OrderByDescending(x => x.CreatingTime).Select(x => x.ToBllMessage())
            };
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                uow.Dispose();
                messageRepository.Dispose();
                userRepository.Dispose();
            }

        }

       
    }
}
