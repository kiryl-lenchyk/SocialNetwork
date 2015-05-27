using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{
    public interface IUserService : IDisposable
    {
        BllUser GetById(int key);

        BllUser GetByName(String name);

        BllUser Create(BllUser e);

        bool IsUserExists(String userName);

        void Delete(BllUser e);

        void Update(BllUser e);
    }
}
