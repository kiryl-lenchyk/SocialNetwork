

namespace SocialNetwork.Dal.Interface.DTO
{

    /// <summary>
    /// Represent some abstract entity to save in storage.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        int Id { get; set; }
    }
}
