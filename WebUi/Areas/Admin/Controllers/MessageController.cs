using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;
using WebUi.Areas.Admin.Mappers;
using WebUi.Areas.Admin.Models;
using WebUi.Filters;

namespace WebUi.Areas.Admin.Controllers
{
    [AutorizeRolesFromConfig("AdminRoleName")]
    public class MessageController : Controller
    {

        #region Fields

        private readonly IUserService userService;
        private readonly IMessageService messageService;
        private readonly ILogger logger;

        private static readonly int MessagesListPageSize;
        private static readonly int DefaultPageSize = 3;

        #endregion

        #region Constractors

        static MessageController()
        {
            if (!Int32.TryParse(WebConfigurationManager.AppSettings["MessagesListPageSize"],
                out MessagesListPageSize))
            {
                MessagesListPageSize = DefaultPageSize;
                ((ILogger) DependencyResolver.Current.GetService(typeof (ILogger))).Log(
                    LogLevel.Error,
                    "web.config contains incorrect date for MessagesListPageSize. Value: {0}",
                    WebConfigurationManager.AppSettings["MessagesListPageSize"]);
            }
        }

        public MessageController(IUserService userService, IMessageService messageService,
            ILogger logger)
        {
            this.userService = userService;
            this.messageService = messageService;
            this.logger = logger;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            logger.Log(LogLevel.Trace, "Request messages list for admin");
            return View();
        }

        public ActionResult MessagesListPage(int? page)
        {
            int pageNumber = page ?? 1;
            IPagedList<MessageViewModel> messageViewModels;

            try
            {
                messageViewModels = messageService.GetAllMessagesPage(MessagesListPageSize,pageNumber).Map(
                    x => x.ToMessageViewModel(
                        userService.GetById(x.SenderId),
                        userService.GetById(x.TargetId)));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Fatal, ex.ToString());
                messageViewModels = new List<MessageViewModel>().ToPagedList(1, 1);
            }

            return PartialView("_MessagesListPage", messageViewModels);

        }

        public ActionResult Edit(int id)
        {
            logger.Log(LogLevel.Trace, "Request message edit page for admin id = {0}", id.ToString());
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null)
                throw new HttpException(404,
                    string.Format("Message id = {0} Not found. Edit message", id));

            MessageViewModel messageViewModel;

            try
            {
                messageViewModel = bllMessage.ToMessageViewModel(
                    userService.GetById(bllMessage.SenderId),
                    userService.GetById(bllMessage.TargetId));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Fatal, ex.ToString());
                messageViewModel = new MessageViewModel()
                {
                    Sender = new UserPreviewViewModel(),
                    Target = new UserPreviewViewModel()
                };
            }

            return View(messageViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(MessageViewModel model)
        {
            BllMessage bllMessage = messageService.GetById(model.Id);
            if (bllMessage == null)
                throw new HttpException(404,
                    string.Format("Message id = {0} Not found. Edit message", model.Id));

            bllMessage.Text = model.Text;
            messageService.EditMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BllMessage bllMessage = messageService.GetById(id);
            if (bllMessage == null)
                throw new HttpException(404,
                    string.Format("Message id = {0} Not found. Delete message", id));

            messageService.DeleteMessage(bllMessage, User.Identity.Name);
            return RedirectToAction("Index");
        }

        #endregion


    }
}
