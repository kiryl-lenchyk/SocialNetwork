using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;

namespace WebUi.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MessageController : Controller
    {
        private readonly IUserService userService;

        private readonly IMessageService messageService;

        public MessageController(IUserService userService, IMessageService messageService)
        {
            this.userService = userService;
            this.messageService = messageService;
        }

        public ActionResult Index()
        {
            return
                View(
                    messageService.GetAllMessages().Select(
                            x =>
                                x.ToMessageViewModel(
                                    userService.GetById(x.SenderId),
                                    userService.GetById(x.TargetId))));
        }

        public ActionResult Edit(int id)
        {
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null) throw new HttpException(404, "Not found");

            return View(bllMessage.ToMessageViewModel(
                userService.GetById(bllMessage.SenderId),
                userService.GetById(bllMessage.TargetId)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(MessageViewModel model)
        {
            BllMessage bllMessage = messageService.GetById(model.Id);
            if (bllMessage == null) throw new HttpException(404, "Not found");
            bllMessage.Text = model.Text;
            messageService.EditMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null) throw new HttpException(404, "Not found");
            messageService.DeleteMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }


    }
}
