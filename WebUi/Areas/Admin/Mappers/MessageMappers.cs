using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Bll.Interface.Entity;
using WebUi.Areas.Admin.Models;

namespace WebUi.Areas.Admin.Mappers
{
    public static class MessageMappers
    {
        public static MessageViewModel ToMessageViewModel(this BllMessage bllMessage, BllUser sender, BllUser target)
        {
            return new MessageViewModel()
            {
                Id = bllMessage.Id,
                CreatingTime = bllMessage.CreatingTime,
                Sender = sender.ToUserPreviewViewModel(),
                Target = target.ToUserPreviewViewModel(),
                Text = bllMessage.Text
            };
        }
    }
}