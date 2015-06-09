using System;
using System.Linq;
using System.Web.Mvc;
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
            int currentUserId = MembershipHelper.GetCurrentUserId(HttpContext.User.Identity.Name);
            return View(messageService.GetUserDialogs(currentUserId).Select(x => x.ToDialogPreviewModel(currentUserId)));
        }

        public ActionResult Dialog(int id)
        {
            int currentUserId = MembershipHelper.GetCurrentUserId(HttpContext.User.Identity.Name);
            BllUser secondUser = userService.GetById(id, currentUserId);
            if (secondUser == null) return HttpNotFound();

            BllDialog dialog = messageService.GetUsersDialog(
                userService.GetById(currentUserId, currentUserId),
                secondUser);
            MarkDialogAsReaded(id, dialog);

            return View(dialog.ToDialogViewModel(currentUserId));
        }

        

        [HttpPost]
        public ActionResult Add(int targetId, String text)
        {
            if (!userService.IsUserExists(targetId)) return HttpNotFound();
            int currentUserId = MembershipHelper.GetCurrentUserId(HttpContext.User.Identity.Name);
            messageService.CreateMessage(new BllMessage
            {
                CreatingTime = DateTime.Now,
                IsReaded = false,
                SenderId = currentUserId,
                TargetId = targetId,
                Text = text
            });


            return PartialView("_DialogMessages", messageService.GetUsersDialog(
                userService.GetById(currentUserId, currentUserId),
                userService.GetById(targetId, currentUserId))
                .ToDialogViewModel(currentUserId)
                .Messages);
        }

        [ChildActionOnly]
        public ActionResult NotReadedMessages()
        {
            return
                PartialView("_NotReadedMessages",
                    messageService.GetUserNotReadedMessagesCount(
                        MembershipHelper.GetCurrentUserId(HttpContext.User.Identity.Name)));
        }

        private void MarkDialogAsReaded(int id, BllDialog dialog)
        {
            foreach (BllMessage message in dialog.Messages.Where(x => x.SenderId == id && !x.IsReaded))
            {
                messageService.MarkAsReaded(message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            messageService.Dispose();
            userService.Dispose();
            base.Dispose(disposing);
        }

    }
}
