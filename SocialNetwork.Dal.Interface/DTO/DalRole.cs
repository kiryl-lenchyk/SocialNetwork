using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Interface.DTO
{
    public class DalRole : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}