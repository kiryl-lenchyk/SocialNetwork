using System;

namespace SocialNetwork.Dal.Interface.DTO
{
    public class DalMessage : IEntity
    {

        public int Id { get; set; }

        public int SenderId { get; set; }

        public int TargetId { get; set; }

        public String Text { get; set; }

        public DateTime CreatingTime { get; set; }

        public bool IsReaded{ get; set; }

    }
}