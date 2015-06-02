using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Infractracture.Mappers;
using WebUi.Models;
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
            return
                View(
                    messageService.GetUsersDialog(
                        userService.GetById(currentUserId, currentUserId),
                        secondUser).ToDialogViewModel(currentUserId));
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

        protected override void Dispose(bool disposing)
        {
            messageService.Dispose();
            base.Dispose(disposing);
        }

    }
}
