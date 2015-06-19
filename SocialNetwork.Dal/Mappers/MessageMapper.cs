using System;
using System.Linq.Expressions;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Orm;

namespace SocialNetwork.Dal.Mappers
{
    /// <summary>
    /// Contains exstension methods for convertion between DalMessage and Message from ORM Layaer.
    /// </summary>
    public static class MessageMapper
    {

        /// <summary>
        /// Convert to ORM Message.
        /// </summary>
        /// <param name="dalMessage">DalMessage to convert</param>
        /// <returns>ORM Message from this DalMessage</returns>
        public static Message ToOrmMessage(this DalMessage dalMessage)
        {
            return new Message()
            {
                Id = dalMessage.Id,
                Sender = dalMessage.SenderId,
                Target = dalMessage.TargetId,
                Text = dalMessage.Text,
                CreatingTime = dalMessage.CreatingTime,
                IsReaded = dalMessage.IsReaded
            };
        }

        /// <summary>
        /// Convert to DalMessage.
        /// </summary>
        /// <param name="message">ORM Message to convert</param>
        /// <returns>DalMessage from this ORM Message</returns>
        public static DalMessage ToDalMessage(this Message message)
        {
            return ToDalMesaageConvertion.Compile()(message);
        }

        /// <summary>
        /// Expression that convert ORM Message to DalMessage. For using in LINQtoSQL query.
        /// </summary>
        public static Expression<Func<Message, DalMessage>> ToDalMesaageConvertion
        {
            get
            {
                return (Message message) =>
                    new DalMessage()
                    {
                        Id = message.Id,
                        SenderId = message.Sender,
                        TargetId = message.Target,
                        Text = message.Text,
                        CreatingTime = message.CreatingTime,
                        IsReaded = message.IsReaded
                    };
            }
            
        }

    }
}
