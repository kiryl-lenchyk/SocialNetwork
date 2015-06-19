using System;

namespace SocialNetwork.Dal.Interface.DTO
{
    /// <summary>
    /// Represent message to save in storage.
    /// </summary>
    public class DalMessage : IEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of user who send message.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Id of user who is target for message.
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Message text.
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Date and time of message creating.
        /// </summary>
        public DateTime CreatingTime { get; set; }

        /// <summary>
        /// True if target read this message already.
        /// </summary>
        public bool IsReaded{ get; set; }

    }
}