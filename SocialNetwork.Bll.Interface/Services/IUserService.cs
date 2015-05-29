using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IUserService : IDisposable
    {
        BllUser GetById(int key, int currentUserId);

        BllUser GetByName(String name, int currentUserId);

        BllUser Create(BllUser e);

        void AddFriend(int currentUserId, int newFriendId);

        void RemoveFriend(int currentUserId, int newFriendId);

        bool IsUserExists(String userName);

        bool IsUserExists(int id);

        void Delete(BllUser e);

        void Update(BllUser e);

    }
}
