using System;
using System.Collections.Generic;

namespace SocialNetwork.Bll.Interface.Entity
{
    public class BllUser
    {
       public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime? BirthDay { get; set; }

        public BllSex? Sex { get; set; }

        public string AboutUser { get; set; }

        public string PasswordHash { get; set; }
        
        public bool CanCurrentUserAddToFriends { get; set; }

        public bool CanCurrentUserWriteMessage { get; set; }

        public IEnumerable<int> SendedMessagesId { get; set; }

        public IEnumerable<int> GottenMessagesId { get; set; }

        public IEnumerable<int> RolesId { get; set; }

        public IEnumerable<int> FriendsId { get; set; }
    }
}
