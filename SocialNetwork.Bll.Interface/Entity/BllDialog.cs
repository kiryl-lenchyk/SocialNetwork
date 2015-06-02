using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Interface.Entity
{
    public class BllDialog
    {
        public BllUser FirstUser { get; set; }

        public BllUser SecondUser { get; set; }

        public IEnumerable<BllMessage> Messages { get; set; } 

    }
}
