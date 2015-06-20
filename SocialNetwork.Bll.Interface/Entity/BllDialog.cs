using System.Collections.Generic;

namespace SocialNetwork.Bll.Interface.Entity
{
    /// <summary>
    /// Represent dialog for work on business layer.
    /// </summary>
    public class BllDialog
    {
        /// <summary>
        /// First user in dialog.
        /// </summary>
        public BllUser FirstUser { get; set; }

        /// <summary>
        /// Second user in dialog.
        /// </summary>
        public BllUser SecondUser { get; set; }

        /// <summary>
        /// IEnumarable of messages was sended between FirstUser and SecondUser.
        /// </summary>
        public IEnumerable<BllMessage> Messages { get; set; } 

    }
}
