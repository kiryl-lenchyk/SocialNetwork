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

        public MessageService(IUnitOfWork uow, IRepository<DalMessage> messageRepository,
            IUserRepository userRepository)
        {
            this.uow = uow;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }

        public BllMessage GetById(int id)
        {
            DalMessage dalMessage = messageRepository.GetById(id);
            return dalMessage == null ? null : dalMessage.ToBllMessage();
        }

        public IEnumerable<BllDialog> GetUserDialogs(int userId)
        {
            DalUser currentDalUser = userRepository.GetById(userId);
            if (currentDalUser == null)
                throw new ArgumentException(string.Format("User with id = {0} not found", userId));
            BllUser currentBllUser = currentDalUser.ToBllUser();

            return messageRepository.GetAllByPredicate(x => x.SenderId == userId)
                .Select(x => x.TargetId)
                .Union(
                    messageRepository.GetAllByPredicate(x => x.TargetId == userId)
                        .Select(x => x.SenderId))
                .ToList()
                .Select(x => GetUsersDialog(currentBllUser, userRepository.GetById(x).ToBllUser()));
        }

        public BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser)
        {
            return new BllDialog
            {
                FirstUser = firstUser,
                SecondUser = secondUser,
                Messages = messageRepository.GetAllByPredicate(
                    x =>
                        x.SenderId == firstUser.Id && x.TargetId == secondUser.Id ||
                        x.TargetId == firstUser.Id && x.SenderId == secondUser.Id)
                    .OrderByDescending(x => x.CreatingTime).ToList().Select(x => x.ToBllMessage())
            };
        }

        public void CreateMessage(BllMessage message)
        {
            messageRepository.Create(message.ToDalMessage());
            uow.Commit();
        }

        public void EditMessage(BllMessage message, string editorName)
        {
            message.Text += String.Format("\r\n[Text edited by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public void DeleteMessage(BllMessage message, string editorName)
        {
            message.Text = String.Format("[Message deleted by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public int GetUserNotReadedMessagesCount(int userId)
        {
            return
                messageRepository.GetAllByPredicate(x => x.TargetId == userId && !x.IsReaded)
                    .Count();
        }

        public void MarkAsReaded(BllMessage message)
        {
            message.IsReaded = true;
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public IEnumerable<BllMessage> GetAllMessages()
        {
            return
                messageRepository.GetAll()
                    .OrderByDescending(x => x.CreatingTime).ToList()
                    .Select(x => x.ToBllMessage());
        }

    }
}
