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

        public IEnumerable<int> FriendsId { get; set; }
    }
}
