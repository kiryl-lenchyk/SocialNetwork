using System;
using PagedList;

namespace WebUi.Models
{
    public class DialogViewModel
    {
        public int SecondUserId { get; set; }

        public String SecondUserName { get; set; }

        public String SecondUserSurname { get; set; }

        public IPagedList<MessageViewModel> Messages { get; set; }

    }
}