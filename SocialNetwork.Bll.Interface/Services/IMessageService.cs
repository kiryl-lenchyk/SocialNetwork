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
        IEnumerable<BllDialog> GetUserDialogs(int userId);

        BllDialog GetUsersDialog(BllUser firstUser, BllUser secondUser);

        void CreateMessage(BllMessage message);
    }
}
