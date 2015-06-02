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
            var model = new List<MessageViewModel>
            {
                new MessageViewModel()
                {
                    UserId = 1,
                    UserName = "Sender",
                    UserSurname = "SenderS",
                    Text = new String('A', 50),
                    CreaingTime = DateTime.Now,
                    IsSended = true
                },
                new MessageViewModel()
                {
                    UserId = 2,
                    UserName = "You",
                    UserSurname = "YouS",
                    Text = new String('B', 50),
                    CreaingTime = DateTime.Now.AddSeconds(35),
                    IsSended = false
                },
                new MessageViewModel()
                {
                    UserId = 1,
                    UserName = "Sender",
                    UserSurname = "SenderS",
                    Text = new String('C', 50),
                    CreaingTime = DateTime.Now.AddSeconds(70),
                    IsSended = true
                },
                new MessageViewModel()
                {
                    UserId = 2,
                    UserName = "You",
                    UserSurname = "YouS",
                    Text = new String('D', 50),
                    CreaingTime = DateTime.Now.AddSeconds(105),
                    IsSended = false
                },
                new MessageViewModel()
                {
                    UserId = targetId,
                    UserName = "Sender",
                    UserSurname = "SenderS",
                    Text = new String('E', 50),
                    CreaingTime = DateTime.Now.AddSeconds(140),
                    IsSended = true
                },
                new MessageViewModel()
                {
                    UserId = 2,
                    UserName = "You",
                    UserSurname = "YouS",
                    Text = new String('F', 50),
                    CreaingTime = DateTime.Now.AddSeconds(175),
                    IsSended = false
                },
                new MessageViewModel()
                {
                    UserId =  2,
                    UserName = "You",
                    UserSurname = "YouS",
                    Text = text,
                    CreaingTime = DateTime.Now,
                    IsSended = false
                }
            };


            return PartialView("_DialogMessages", model);
        }

        protected override void Dispose(bool disposing)
        {
            messageService.Dispose();
            base.Dispose(disposing);
        }

    }
}
