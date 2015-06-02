using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using WebUi.Models;

namespace WebUi.Infractracture.Mappers
{
    public static class MessageMapper
    {
        public static DialogPreviewModel ToDialogPreviewModel(this BllDialog dialog, int currentUserId)
        {
            BllUser interlocutor = dialog.FirstUser.Id == currentUserId
                ? dialog.SecondUser
                : dialog.FirstUser;
            return new DialogPreviewModel()
            {
                UserId = interlocutor.Id,
                UserName = interlocutor.Name,
                UserSurname = interlocutor.Surname,
                LastMessage = dialog.Messages.First().Text
            };
        }

    }
}