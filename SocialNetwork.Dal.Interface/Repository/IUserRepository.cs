﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    public interface IUserRepository : IRepository<DalUser>
    {
        DalUser GetByName(String name);

        void AddToFriends(DalUser currentUser, DalUser newFriend);

        void RemoveFriend(DalUser currentUser, DalUser newFriend);
    }
}
