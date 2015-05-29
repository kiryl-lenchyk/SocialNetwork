using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Interface.DTO
{
    public class DalUser : IEntity
    {
       public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime? BirthDay { get; set; }

        public DalSex? Sex { get; set; }

        public string AboutUser { get; set; }

        public string PasswordHash { get; set; }

        public IEnumerable<int> SendedMessagesId { get; set; }

        public IEnumerable<int> GottenMessagesId { get; set; }

        public IEnumerable<int> RolesId { get; set; }

        public IEnumerable<int> FriendsId { get; set; }
 
    }
}
