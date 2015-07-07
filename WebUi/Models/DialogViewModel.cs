using System;
using System.Collections.Generic;
using PagedList;

namespace WebUi.Models
{
    public class DialogViewModel
    {
        public static DialogViewModel Empty
        {
            get
            {
                return new DialogViewModel()
                {
                    Messages = new List<MessageViewModel>().ToPagedList(1, 1)
                };
            }
        }


        public int SecondUserId { get; set; }

        public String SecondUserName { get; set; }

        public String SecondUserSurname { get; set; }

        public IPagedList<MessageViewModel> Messages { get; set; }

    }
}