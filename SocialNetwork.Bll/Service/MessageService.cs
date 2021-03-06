﻿using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using SocialNetwork.Bll.Interface;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Mappers;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Logger.Interface;

namespace SocialNetwork.Bll.Service
{
    
    /// <summary>
    /// Represent business logic function for Messages 
    /// </summary>
    public class MessageService : IMessageService
    {

        #region Fields

        private readonly IUnitOfWork uow;
        private readonly IRepository<DalMessage> messageRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger logger;

        #endregion

        #region Constractors

        /// <summary>
        /// Create new instance of MessageService for determinated storage.
        /// </summary>
        /// <param name="uow">unit of work for commit in storage.</param>
        /// <param name="userRepository">user strorage.</param>
        /// <param name="messageRepository">message storage.</param>
        /// <param name="logger">class for log</param>
        public MessageService(IUnitOfWork uow, IRepository<DalMessage> messageRepository,
            IUserRepository userRepository, ILogger logger)
        {
            this.uow = uow;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all messages from storage.
        /// </summary>
        /// <param name="pageSize">elements count on one page</param>
        /// <param name="pageNumber">number of page to draw</param>
        /// <returns>IEnumerable of all messages.</returns>
        public IMappedPagedList<BllMessage> GetAllMessagesPage(int pageSize, int pageNumber)
        {
            logger.Log(LogLevel.Trace,
                "MessageService.GetAllMessagesPage invoked pageSize = {0} pageNumber = {1}",
                pageSize, pageNumber);

            return
                PagedList<BllMessage>.GetPagedListWithConvert(
                    messageRepository.GetAll().OrderByDescending(x => x.CreatingTime), pageSize,
                    pageNumber, x => x.ToBllMessage());
        }

        /// <summary>
        /// Get message by id.
        /// </summary>
        /// <param name="id">message id.</param>
        /// <returns>founded BllMessage or null, if it's not found.</returns>
        public BllMessage GetById(int id)
        {
            logger.Log(LogLevel.Trace,"MessageService.GetById invoked id = {0}", id);

            DalMessage dalMessage = messageRepository.GetById(id);
            return dalMessage == null ? null : dalMessage.ToBllMessage();
        }

        /// <summary>
        /// Get all dialogs for user.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>IEnumerable of all user's dialogs.</returns>
        public IEnumerable<BllDialog> GetUserDialogs(int userId)
        {
            logger.Log(LogLevel.Trace,"MessageService.GetUserDialogs invoked userId = {0}", userId);

            DalUser currentDalUser = userRepository.GetById(userId);
            if (currentDalUser == null)
                throw new ArgumentException(string.Format("User with id = {0} not found", userId));
            BllUser currentBllUser = currentDalUser.ToBllUser();

            return  GetUserInterloctorsIds(userId).ToList()
               .Select(x => GetUsersDialog(currentBllUser, userRepository.GetById(x).ToBllUser()));
        }

       

        public IMappedPagedList<BllDialog> GetUserDialogsPage(int userId, int pageSize, int pageNumber)
        {

            logger.Log(LogLevel.Trace,
                "MessageService.GetUserDialogsPage invoked userId = {0} pageSize = {1} pageNumber = {2}",
                userId, pageSize, pageNumber);

            DalUser currentDalUser = userRepository.GetById(userId);
            if (currentDalUser == null)
                throw new ArgumentException(string.Format("User with id = {0} not found", userId));
            BllUser currentBllUser = currentDalUser.ToBllUser();

            IOrderedQueryable<int> userInterloctorsIds = GetUserInterloctorsIds(userId);
            return PagedList<BllDialog>.GetPagedListWithConvert(userInterloctorsIds, pageSize,
                pageNumber,
                x => GetUsersDialog(currentBllUser, userRepository.GetById(x).ToBllUser()));

        }

        /// <summary>
        /// Get dialog of two users. Result can contains zero messages.
        /// </summary>
        /// <param name="firstUser">first user from dialog</param>
        /// <param name="secondUser">second user from dialog</param>
        /// <returns>Users dialog. Can contains empty list of messages (if users don't write messages)</returns>
        public BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser)
        {
            if(firstUser == null) throw new ArgumentNullException("firstUser");
            if (secondUser == null) throw new ArgumentNullException("secondUser");
            logger.Log(LogLevel.Trace,"MessageService.GetUsersDialog invoked firstUser = {0}, secondUser = {1} ", firstUser, secondUser);

            return new BllDialog
            {
                FirstUser = firstUser,
                SecondUser = secondUser,
                Messages =
                    GetUsersDialogsMessaages(firstUser, secondUser)
                        .ToList()
                        .Select(x => x.ToBllMessage())
            };
        }

        

        /// <summary>
        /// Get dialog of two users separeted to pages. Result can contains zero messages.
        /// </summary>
        /// <param name="firstUser">first user from dialog</param>
        /// <param name="secondUser">second user from dialog</param>
        /// <param name="pageSize">elements count on one page</param>
        /// <param name="pageNumber">number of page to draw</param>
        /// <returns>Users dialog. Can contains empty list of messages (if users don't write messages)</returns>
        public BllDialogPage GetUsersDialogPage(BllUser firstUser, BllUser secondUser, int pageSize, int pageNumber)
        {
            if (firstUser == null) throw new ArgumentNullException("firstUser");
            if (secondUser == null) throw new ArgumentNullException("secondUser");
            logger.Log(LogLevel.Trace, "MessageService.GetUsersDialog invoked firstUser = {0}, secondUser = {1} pageSize = {2}  pageNumber = {3}",
                firstUser, secondUser,pageSize,pageNumber);


            return new BllDialogPage()
            {
                FirstUser = firstUser,
                SecondUser = secondUser,
                Messages =
                    PagedList<BllMessage>.GetPagedListWithConvert(
                        GetUsersDialogsMessaages(firstUser, secondUser), pageSize, pageNumber,
                        x => x.ToBllMessage())
            };

        }

        /// <summary>
        /// Save new message to storage.
        /// </summary>
        /// <param name="message">new message to save. Id will be selected by storage.</param>
        /// <returns>added message with selected by storage id.</returns>
        public void CreateMessage(BllMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            logger.Log(LogLevel.Trace,"MessageService.CreateMessage invoked sender = {0}, target = {1}",
                message.SenderId, message.TargetId);

            messageRepository.Create(message.ToDalMessage());
            uow.Commit();
        }

        /// <summary>
        /// Change message information. Message will be signed by editorName.
        /// </summary>
        /// <param name="message">new value of message. Message will be found by id.</param>
        /// <param name="editorName">editor name to save in significate.</param>
        public void EditMessage(BllMessage message, string editorName)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (editorName == null) throw new ArgumentNullException("editorName");
            logger.Log(LogLevel.Trace,"MessageService.EditMessage invoked id = {0}", message.Id);

            message.Text += String.Format("\r\n[Text edited by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        /// <summary>
        /// Mark message as deleted. Information about deleting will be signed by editorName
        /// </summary>
        /// <param name="message">message to delete. Will be selected by id.</param>
        /// <param name="editorName">editor name to save in significate.</param>
        public void DeleteMessage(BllMessage message, string editorName)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (editorName == null) throw new ArgumentNullException("editorName");
            logger.Log(LogLevel.Trace,"MessageService.DeleteMessage invoked id = {0}", message.Id);

            message.Text = String.Format("[Message deleted by {0} at {1}]", editorName,
                DateTime.Now.ToString("g"));
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        /// <summary>
        /// Get count of messages wich user don't read.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>count of not readed messages.</returns>
        public int GetUserNotReadedMessagesCount(int userId)
        {
            logger.Log(LogLevel.Trace,"MessageService.GetUserNotReadedMessagesCount invoked userId = {0}", userId);

            return
                messageRepository.GetAllByPredicate(x => x.TargetId == userId && !x.IsReaded)
                    .Count();
        }

        /// <summary>
        /// Mark message as readed.
        /// </summary>
        /// <param name="message">message to mark. Will be selected by id.</param>
        public void MarkAsReaded(BllMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            logger.Log(LogLevel.Trace,"MessageService.MarkAsReaded invoked id = {0}", message.Id);

            message.IsReaded = true;
            messageRepository.Update(message.ToDalMessage());
            uow.Commit();
        }

        /// <summary>
        /// Get all messages from storage.
        /// </summary>
        /// <returns>IEnumerable of all messages.</returns>
        public IEnumerable<BllMessage> GetAllMessages()
        {
            logger.Log(LogLevel.Trace,"MessageService.GetAllMessages invoked");

            return
                messageRepository.GetAll()
                    .OrderByDescending(x => x.CreatingTime).ToList()
                    .Select(x => x.ToBllMessage());
        }

        #endregion

        #region Private Methods

        private IOrderedQueryable<int> GetUserInterloctorsIds(int userId)
        {
            return messageRepository.GetAllByPredicate(x => x.SenderId == userId)
                .Select(x => x.TargetId)
                .Union(
                    messageRepository.GetAllByPredicate(x => x.TargetId == userId)
                        .Select(x => x.SenderId)).OrderBy(x => x);
        }

        private IOrderedQueryable<DalMessage> GetUsersDialogsMessaages(BllUser firstUser, BllUser secondUser)
        {
            return messageRepository.GetAllByPredicate(
                x =>
                    x.SenderId == firstUser.Id && x.TargetId == secondUser.Id ||
                    x.TargetId == firstUser.Id && x.SenderId == secondUser.Id)
                .OrderByDescending(x => x.CreatingTime);
        }

        #endregion

    }
}
