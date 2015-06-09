﻿using System;
using System.Linq.Expressions;
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
                Sender = dalMessage.SenderId,
                Target = dalMessage.TargetId,
                Text = dalMessage.Text,
                CreatingTime = dalMessage.CreatingTime,
                IsReaded = dalMessage.IsReaded
            };
        }

        public static DalMessage ToDalMessage(this Message message)
        {
            return ToDalMesaageConvertion.Compile()(message);
        }

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
