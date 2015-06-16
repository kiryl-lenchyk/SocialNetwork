using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
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

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MessageService(IUnitOfWork uow, IRepository<DalMessage> messageRepository,
            IUserRepository userRepository)
        {
            this.uow = uow;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }

        public BllMessage GetById(int id)
        {
            Logger.Trace("MessageService.GetById invoked id = {0}", id);

            DalMessage dalMessage = messageRepository.GetById(id);
            return dalMessage == null ? null : dalMessage.ToBllMessage();
        }

        public IEnumerable<BllDialog> GetUserDialogs(int userId)
        {
            Logger.Trace("MessageService.GetUserDialogs invoked userId = {0}", userId);

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
            if(firstUser == null) throw new ArgumentNullException("firstUser");
            if (secondUser == null) throw new ArgumentNullException("secondUser");
            Logger.Trace("MessageService.GetUsersDialog invoked firstUser = {0}, secondUser = {1} ", firstUser, secondUser);

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
            if (message == null) throw new ArgumentNullException("message");
            Logger.Trace("MessageService.CreateMessage invoked sender = {0}, target = {1}", message.SenderId, message.TargetId);

            messageRepository.Create(message.ToDalMessage());
            uow.Commit();
        }

        public void EditMessage(BllMessage message, string editorName)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (editorName == null) throw new ArgumentNullException("editorName");
            Logger.Trace("MessageService.EditMessage invoked id = {0}", message.Id);

            message.Text += String.Format("\r\n[Text edited by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public void DeleteMessage(BllMessage message, string editorName)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (editorName == null) throw new ArgumentNullException("editorName");
            Logger.Trace("MessageService.DeleteMessage invoked id = {0}", message.Id);

            message.Text = String.Format("[Message deleted by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public int GetUserNotReadedMessagesCount(int userId)
        {
            Logger.Trace("MessageService.GetUserNotReadedMessagesCount invoked userId = {0}", userId);

            return
                messageRepository.GetAllByPredicate(x => x.TargetId == userId && !x.IsReaded)
                    .Count();
        }

        public void MarkAsReaded(BllMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Logger.Trace("MessageService.MarkAsReaded invoked id = {0}", message.Id);

            message.IsReaded = true;
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        public IEnumerable<BllMessage> GetAllMessages()
        {
            Logger.Trace("MessageService.GetAllMessages invoked");

            return
                messageRepository.GetAll()
                    .OrderByDescending(x => x.CreatingTime).ToList()
                    .Select(x => x.ToBllMessage());
        }

    }
}
