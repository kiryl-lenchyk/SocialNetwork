using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Bll.Mappers
{
    /// <summary>
    /// Contains exstension methods for convertion between DalMessage and BllMessage.
    /// </summary>
    public static class MessageMapper
    {
        /// <summary>
        /// Convert to DalMessage.
        /// </summary>
        /// <param name="bllMessage">BllMessage to convert</param>
        /// <returns>DalMessage from this BllMessage</returns>
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

        /// <summary>
        /// Convert to BllMessage.
        /// </summary>
        /// <param name="dalMessage">DalMessage to convert</param>
        /// <returns>BllMessage from this DalMessage</returns>
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
