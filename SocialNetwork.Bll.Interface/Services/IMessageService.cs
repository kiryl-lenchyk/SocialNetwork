using System;
using System.Collections.Generic;
using PagedList;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{

    /// <summary>
    /// Represent business logic function for Messages 
    /// </summary>
    public interface IMessageService 
    {
        /// <summary>
        /// Get all messages from storage.
        /// </summary>
        /// <returns>IEnumerable of all messages.</returns>
        IEnumerable<BllMessage> GetAllMessages();

        /// <summary>
        /// Get all messages from storage.
        /// </summary>
        /// <param name="pageSize">elements count on one page</param>
        /// <param name="pageNumber">number of page to draw</param>
        /// <returns>IEnumerable of all messages.</returns>
        IMappedPagedList<BllMessage> GetAllMessagesPage(int pageSize, int pageNumber);

        /// <summary>
        /// Get message by id.
        /// </summary>
        /// <param name="id">message id.</param>
        /// <returns>founded BllMessage or null, if it's not found.</returns>
        BllMessage GetById(int id);

        /// <summary>
        /// Get all dialogs for user.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <returns>IEnumerable of all user's dialogs.</returns>
        IEnumerable<BllDialog> GetUserDialogs(int userId);


        /// <summary>
        /// Get all dialogs for user separeted to pages. 
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <param name="pageSize">elements count on one page</param>
        /// <param name="pageNumber">number of page to draw</param>
        /// <returns>IEnumerable of all user's dialogs.</returns>
        IMappedPagedList<BllDialog> GetUserDialogsPage(int userId, int pageSize, int pageNumber);

        /// <summary>
        /// Get dialog of two users. Result can contains zero messages.
        /// </summary>
        /// <param name="firstUser">first user from dialog</param>
        /// <param name="secondUser">second user from dialog</param>
        /// <returns>Users dialog. Can contains empty list of messages (if users don't write messages)</returns>
        BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser);

        /// <summary>
        /// Get dialog of two users separeted to pages. Result can contains zero messages.
        /// </summary>
        /// <param name="firstUser">first user from dialog</param>
        /// <param name="secondUser">second user from dialog</param>
        /// <param name="pageSize">elements count on one page</param>
        /// <param name="pageNumber">number of page to draw</param>
        /// <returns>Users dialog. Can contains empty list of messages (if users don't write messages)</returns>
        BllDialogPage GetUsersDialogPage(BllUser firstUser, BllUser secondUser, int pageSize, int pageNumber);

        /// <summary>
        /// Save new message to storage.
        /// </summary>
        /// <param name="message">new message to save. Id will be selected by storage.</param>
        /// <returns>added message with selected by storage id.</returns>
        void CreateMessage(BllMessage message);

        /// <summary>
        /// Change message information. Message will be signed by editorName.
        /// </summary>
        /// <param name="message">new value of message. Message will be found by id.</param>
        /// <param name="editorName">editor name to save in significate.</param>
        void EditMessage(BllMessage message, String editorName);

        /// <summary>
        /// Mark message as deleted. Information about deleting will be signed by editorName
        /// </summary>
        /// <param name="message">message to delete. Will be selected by id.</param>
        /// <param name="editorName">editor name to save in significate.</param>
        void DeleteMessage(BllMessage message, String editorName);

        /// <summary>
        /// Get count of messages wich user don't read.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>count of not readed messages.</returns>
        int GetUserNotReadedMessagesCount(int userId);

        /// <summary>
        /// Mark message as readed.
        /// </summary>
        /// <param name="message">message to mark. Will be selected by id.</param>
        void MarkAsReaded(BllMessage message);
    }
}
