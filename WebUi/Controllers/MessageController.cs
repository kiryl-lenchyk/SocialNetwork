using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Infractracture.Mappers;
using WebUi.Providers;

namespace WebUi.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
          private readonly IMessageService messageService;
        private readonly IUserService userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            this.messageService = messageService;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            return View(messageService.GetUserDialogs(currentUserId).Select(x => x.ToDialogPreviewModel(currentUserId)));
        }

        public ActionResult Dialog(int id)
        {
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            BllUser secondUser = userService.GetById(id);
            if (secondUser == null) throw new HttpException(404, "Not found");

            BllDialog dialog = messageService.GetUsersDialog(
                userService.GetById(currentUserId),
                secondUser);
            MarkDialogAsReaded(id, dialog);

            return View(dialog.ToDialogViewModel(currentUserId));
        }

        

        [HttpPost]
        public ActionResult Add(int targetId, String text)
        {
            if (!userService.IsUserExists(targetId)) throw new HttpException(404, "Not found");
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            messageService.CreateMessage(new BllMessage
            {
                CreatingTime = DateTime.Now,
                IsReaded = false,
                SenderId = currentUserId,
                TargetId = targetId,
                Text = text
            });


            return PartialView("_DialogMessages", messageService.GetUsersDialog(
                userService.GetById(currentUserId),
                userService.GetById(targetId))
                .ToDialogViewModel(currentUserId)
                .Messages);
        }

        [ChildActionOnly]
        public ActionResult NotReadedMessages()
        {
            return
                PartialView("_NotReadedMessages",
                    messageService.GetUserNotReadedMessagesCount(
                        userService.GetByName(User.Identity.Name).Id));
        }

        private void MarkDialogAsReaded(int id, BllDialog dialog)
        {
            foreach (BllMessage message in dialog.Messages.Where(x => x.SenderId == id && !x.IsReaded))
            {
                messageService.MarkAsReaded(message);
            }
        }


    }
}
