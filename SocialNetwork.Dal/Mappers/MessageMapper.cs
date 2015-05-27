using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{
    public static class MessageMapper
    {
        public static Message ToOrmMessage(this DalMessage dalMessage)
        {
            return new Message()
            {
                Id = dalMessage.Id,
                Sender = dalMessage.Sender.ToOrmUser(),
                Target = dalMessage.Target.ToOrmUser(),
                Text = dalMessage.Text,
                CreatingTime = dalMessage.CreatingTime
            };
        }

        public static DalMessage ToDalMessage(this Message message)
        {
            return new DalMessage()
            {
                Id = message.Id,
                Sender = message.Sender.ToDalUser(),
                Target = message.Target.ToDalUser(),
                Text = message.Text,
                CreatingTime = message.CreatingTime
            };
        }

    }
}
