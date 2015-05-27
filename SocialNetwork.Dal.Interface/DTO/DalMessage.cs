using System;

namespace SocialNetwork.Dal.Interface.DTO
{
    public class DalMessage : IEntity
    {

        public int Id { get; set; }

        public DalUser Sender { get; set; }

        public DalUser Target { get; set; }

        public String Text { get; set; }

        public DateTime CreatingTime { get; set; }

    }
}