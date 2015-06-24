﻿using System;
using System.Collections.Generic;

namespace WebUi.Models
{
    public class DialogViewModel
    {
        public int SecondUserId { get; set; }

        public String SecondUserName { get; set; }

        public String SecondUserSurname { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }

    }
}