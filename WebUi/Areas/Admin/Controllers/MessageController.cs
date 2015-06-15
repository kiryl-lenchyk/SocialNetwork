using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;
using WebUi.Filters;

namespace WebUi.Areas.Admin.Controllers
{
   [AutorizeRolesFromConfig("AdminRoleName")]
    public class MessageController : Controller
    {
       private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService userService;

        private readonly IMessageService messageService;

        public MessageController(IUserService userService, IMessageService messageService)
        {
            this.userService = userService;
            this.messageService = messageService;
        }

        public ActionResult Index()
        {
            Logger.Trace("Request messages list for admin");
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
            Logger.Trace("Request message edit page for admin id = {0}", id.ToString());
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null) throw new HttpException(404, string.Format("Message id = {0} Not found. Edit message", id));

            return View(bllMessage.ToMessageViewModel(
                userService.GetById(bllMessage.SenderId),
                userService.GetById(bllMessage.TargetId)));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(MessageViewModel model)
        {
            BllMessage bllMessage = messageService.GetById(model.Id);
            if (bllMessage == null) throw new HttpException(404, string.Format("Message id = {0} Not found. Edit message", id));
            bllMessage.Text = model.Text;
            messageService.EditMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null) throw new HttpException(404, string.Format("Message id = {0} Not found. Delete message", id));
            messageService.DeleteMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }


    }
}
