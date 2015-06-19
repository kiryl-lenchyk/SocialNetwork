using System.IO;

namespace SocialNetwork.Dal.Interface.DTO
{
    /// <summary>
    /// Represent avatar for save in storage.
    /// </summary>
    public class DalAvatar : IEntity
    {
        /// <summary>
        /// Unique identifier for entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id for user who is owher of avatar.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Stream where avatar image saved.
        /// </summary>
        public Stream ImageStream { get; set; }
    }
}
