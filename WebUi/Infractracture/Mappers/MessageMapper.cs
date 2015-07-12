using System.Linq;
using PagedList;
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
            BllMessage lastMessage = dialog.Messages.First();
            return new DialogPreviewModel()
            {
                UserId = interlocutor.Id,
                UserName = interlocutor.Name,
                UserSurname = interlocutor.Surname,
                LastMessage = lastMessage.Text,
                IsReaded = lastMessage.SenderId == currentUserId || lastMessage.IsReaded
            };
        }
        
        public static DialogViewModel ToDialogPageViewModel(this BllDialogPage dialog, int currentUserId)
        {
            BllUser interlocutor = dialog.FirstUser.Id == currentUserId
               ? dialog.SecondUser
               : dialog.FirstUser;
            return new DialogViewModel()
            {
                SecondUserId = interlocutor.Id,
                SecondUserName = interlocutor.Name,
                SecondUserSurname = interlocutor.Surname,
                Messages =
                    dialog.Messages.Map(x => x.ToMessageViewModel(currentUserId, dialog.FirstUser,dialog.SecondUser))
                        
            };
        }

        public static MessageViewModel ToMessageViewModel(this BllMessage message, int currentUserId,
            BllUser firstDialogUser, BllUser secondDialogUser)
        {
            BllUser sender = message.SenderId == firstDialogUser.Id
                ? firstDialogUser
                : secondDialogUser;
            return new MessageViewModel()
            {
                CreaingTime = message.CreatingTime,
                Text = message.Text,
                UserId = sender.Id,
                UserName = sender.Name,
                UserSurname = sender.Surname,
                IsSended = sender.Id == currentUserId,
                IsReaded = sender.Id == currentUserId || message.IsReaded
            };
        }

    }
}