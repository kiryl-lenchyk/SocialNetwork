using System.IO;


namespace SocialNetwork.Dal.Interface.DTO
{
    public class DalAvatar : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Stream ImageStream { get; set; }
    }
}
