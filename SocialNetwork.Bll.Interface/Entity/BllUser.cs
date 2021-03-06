﻿using System;
using System.Collections.Generic;

namespace SocialNetwork.Bll.Interface.Entity
{
    /// <summary>
    /// Represent application user for work on business layer.
    /// </summary>
    public class BllUser
    {

        /// <summary>
        /// Unique identifier for user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username for login and as text unique identifier.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User name (user as human). Can be null.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User surname. Can be null.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Birthday date. Can be null. 
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// User sex. Can be null.
        /// </summary>
        public BllSex? Sex { get; set; }

        /// <summary>
        /// Some additional information. Can be null.
        /// </summary>
        public string AboutUser { get; set; }

        /// <summary>
        /// Hash of user password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// IEnumarable of ids of user friends.
        /// </summary>
        public IEnumerable<int> FriendsId { get; set; }
    }
}
