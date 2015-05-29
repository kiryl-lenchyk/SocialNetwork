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
                SenderId = bllMessage.SenderId,
                TargetId = bllMessage.TargetId,
                Text = bllMessage.Text,
                CreatingTime = bllMessage.CreatingTime,
                IsReaded = bllMessage.IsReaded
            };
        }

        public static BllMessage ToBllMessage(this DalMessage dalMessage)
        {
            return new BllMessage()
            {
                Id = dalMessage.Id,
                SenderId = dalMessage.SenderId,
                TargetId = dalMessage.TargetId,
                Text = dalMessage.Text,
                CreatingTime = dalMessage.CreatingTime,
                IsReaded = dalMessage.IsReaded

            };
        }

    }
}
