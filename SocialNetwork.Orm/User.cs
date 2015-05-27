//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetwork.Orm
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.SendedMessages = new HashSet<Message>();
            this.GottenMessages = new HashSet<Message>();
            this.Friends = new HashSet<User>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public Nullable<Sex> Sex { get; set; }
        public string AboutUser { get; set; }
    
        public virtual ICollection<Message> SendedMessages { get; set; }
        public virtual ICollection<Message> GottenMessages { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
