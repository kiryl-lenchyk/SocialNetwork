using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IUserService
    {
        BllUser GetById(int key, int currentUserId);

        BllUser GetByName(String name, int currentUserId);

        BllUser Create(BllUser e);

        void AddFriend(int currentUserId, int newFriendId);

        void RemoveFriend(int currentUserId, int newFriendId);

        IEnumerable<BllUser> FindUsers(string name, string surname, DateTime? birthDayMin,
            DateTime? birthDayMax, BllSex? sex);

        IEnumerable<BllUser> GetAllUsers();

        bool IsUserExists(String userName);

        bool IsUserExists(int id);

        void Delete(BllUser e);

        void Update(BllUser e);

        void SetUserAvatar(int userId, Stream avatarStream);

        Stream GetUserAvatarStream(int userId);

    }
}
