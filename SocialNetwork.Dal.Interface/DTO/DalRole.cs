
namespace SocialNetwork.Dal.Interface.DTO
{
    /// <summary>
    /// Represent role to save in storage.
    /// </summary>
    public class DalRole : IEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role name as text unique identifier.
        /// </summary>
        public string Name { get; set; }

    }
}