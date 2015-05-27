using System;

namespace SocialNetwork.Bll.Interface.Entity
{
    public class BllMessage
    {

        public int Id { get; set; }

        public BllUser Sender { get; set; }

        public BllUser Target { get; set; }

        public String Text { get; set; }

        public DateTime CreatingTime { get; set; }

    }
}