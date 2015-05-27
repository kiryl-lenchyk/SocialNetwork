using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{
    public static class MessageMapper
    {
        public static DalMessage ToDalMessage(this BllMessage bllMessage)
        {
            return new DalMessage()
            {
                Id = bllMessage.Id,
                Sender = bllMessage.Sender.ToDalUser(),
                Target = bllMessage.Target.ToDalUser(),
                Text = bllMessage.Text,
                CreatingTime = bllMessage.CreatingTime
            };
        }

        public static BllMessage ToBllMessage(this DalMessage dalMessage)
        {
            return new BllMessage()
            {
                Id = dalMessage.Id,
                Sender = dalMessage.Sender.ToBllUser(),
                Target = dalMessage.Target.ToBllUser(),
                Text = dalMessage.Text,
                CreatingTime = dalMessage.CreatingTime
            };
        }

    }
}
