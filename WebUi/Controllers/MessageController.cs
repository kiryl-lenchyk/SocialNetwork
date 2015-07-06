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
using WebUi.Infractracture.Mappers;
using WebUi.Models;

namespace WebUi.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
    
        #region Fields

        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly ILogger logger;

        private static readonly int DialogsListPageSize;

        #endregion

        #region Constractors

        static MessageController ()
        {
            if (!Int32.TryParse(WebConfigurationManager.AppSettings["DialogsListPageSize"],
                out DialogsListPageSize))
            {
                DialogsListPageSize = 3;
                ((ILogger) DependencyResolver.Current.GetService(typeof (ILogger))).Log(
                    LogLevel.Error,
                    "web.config contains incorrect date for DialogsListPageSize. Value: {0}",
                    WebConfigurationManager.AppSettings["DialogsListPageSize"]);
            }
        }

        public MessageController(IMessageService messageService, IUserService userService, ILogger logger)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.logger = logger;
        }

        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DialogsListPage(int? page)
        {
            int pageNumber = page ?? 1;
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            logger.Log(LogLevel.Trace, "Request dialog list page . Current user id = {0}", currentUserId.ToString());

            IEnumerable<DialogPreviewModel> dialogPreviewModels = messageService.GetUserDialogs(currentUserId)
                .Select(x => x.ToDialogPreviewModel(currentUserId));

            return PartialView("_DialogsListPage",
                dialogPreviewModels.ToPagedList(pageNumber, DialogsListPageSize));
        }

        public ActionResult Dialog(int id)
        {
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            BllUser secondUser = userService.GetById(id);
            logger.Log(LogLevel.Trace,"Request dialog page id = {0}. Current user id = {1}", id.ToString(), currentUserId.ToString());
            if (secondUser == null) throw new HttpException(404, string.Format("User id = {0} Not found", id));

            BllDialog dialog = messageService.GetUsersDialog(
                userService.GetById(currentUserId),
                secondUser);
            MarkDialogAsReaded(id, dialog);

            return View(dialog.ToDialogViewModel(currentUserId,1,5));
        }
        
        [HttpPost]
        public ActionResult Add(int targetId, String text)
        {
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            logger.Log(LogLevel.Trace,"Request send message to id = {0}. Current user id = {1}", targetId.ToString(), currentUserId.ToString());
            if (!userService.IsUserExists(targetId)) throw new HttpException(404, string.Format("User id = {0} Not found", targetId));
       
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
                .ToDialogViewModel(currentUserId,1,5));
        }


        public ActionResult DialogPage(int? page, int targetId)
        {
            int pageNumber = page ?? 1;
            int currentUserId = userService.GetByName(User.Identity.Name).Id;
            if (!userService.IsUserExists(targetId)) throw new HttpException(404, string.Format("User id = {0} Not found", targetId));

            return PartialView("_DialogMessages", messageService.GetUsersDialog(
               userService.GetById(currentUserId),
               userService.GetById(targetId))
               .ToDialogViewModel(currentUserId, pageNumber,5));
        }


        [ChildActionOnly]
        public ActionResult NotReadedMessages()
        {
            return
                PartialView("_NotReadedMessages",
                    messageService.GetUserNotReadedMessagesCount(
                        userService.GetByName(User.Identity.Name).Id));
        }

        #endregion

        #region Private Methods

        private void MarkDialogAsReaded(int id, BllDialog dialog)
        {
            foreach (BllMessage message in dialog.Messages.Where(x => x.SenderId == id && !x.IsReaded))
            {
                messageService.MarkAsReaded(message);
            }
        }

        #endregion

    }
}
