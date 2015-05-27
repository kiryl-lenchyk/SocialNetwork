using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUi.Models;

namespace WebUi.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {


        public ActionResult Index()
        {
            List<DialogPreviewModel> model = new List<DialogPreviewModel>()
            {
                new DialogPreviewModel(){UserId = 1, UserName = "Name1", UserSurname = "Surname1", LastMessage = new String('A',50)},
                new DialogPreviewModel(){UserId = 2, UserName = "Name2", UserSurname = "Surname2", LastMessage = new String('B',50)},
                new DialogPreviewModel(){UserId = 3, UserName = "Name3", UserSurname = "Surname3", LastMessage = new String('C',50)},
                new DialogPreviewModel(){UserId = 4, UserName = "Name4", UserSurname = "Surname4", LastMessage = new String('D',50)}
            };

            return View(model);
        }

        public ActionResult Dialog(int id)
        {
            DialogViewModel model = new DialogViewModel();
            model.Messages = new List<MessageViewModel>
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
                    UserId = 1,
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
            };

            model.SecondUserId = 1;
            model.SecondUserName = "Sender";
            model.SecondUserSurname = "SenderS";
            return View(model);
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
    }
}
