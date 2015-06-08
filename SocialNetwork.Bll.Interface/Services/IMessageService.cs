using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IMessageService : IDisposable
    {

        IEnumerable<BllMessage> GetAllMessages();

        BllMessage GetById(int id);

        IEnumerable<BllDialog> GetUserDialogs(int userId);

        BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser);

        void CreateMessage(BllMessage message);

        void EditMessage(BllMessage message, String editorName);

        void DeleteMessage(BllMessage message, String editorName);

        int GetUserNotReadedMessagesCount(int userId);

        void MarkAsReaded(BllMessage message);
    }
}
