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

        public string SecurityStamp { get; set; }

        public IEnumerable<DalMessage> SendedMessages { get; set; }

        public IEnumerable<DalMessage> GottenMessages { get; set; }

        public IEnumerable<DalRole> Roles { get; set; }

        public IEnumerable<DalUser> Friends { get; set; }
 
    }
}
