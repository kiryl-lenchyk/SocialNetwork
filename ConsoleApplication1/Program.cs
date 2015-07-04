using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Repository;
using SocialNetwork.Logger.NLogLogger;
using SocialNetwork.Orm;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SocialNetworkDatabaseEntities socialNetworkDatabase = new SocialNetworkDatabaseEntities())
            {
                UserRepository repository = new UserRepository(socialNetworkDatabase, new NLogLogger());

                    DalUser user = repository.GetByPredicate(x => x.Id == 1 && x.Sex == null);
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", user.Id, user.Name, user.Surname);
                    

            }


            Console.ReadLine();

        }
    }
}
