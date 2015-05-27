using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal;
using SocialNetwork.Dal.Interface.DTO;
using SocialNetwork.Dal.Repository;
using SocialNetwork.Orm;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SocialNetworkDatabase socialNetworkDatabase = new SocialNetworkDatabase())
            {
                using (UserRepository repository = new UserRepository(socialNetworkDatabase))
                {
                //    DalUser user = repository.GetById(1);
                //Console.WriteLine("Id {0}\nName {1}\nSurname {2}", user.Id,user.Name,user.Surname);
                //foreach (var dalUser in user.Friends)
                //{
                //    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalUser.Id, dalUser.Name, dalUser.Surname);
                //}

                //foreach (var dalMessage in user.GottenMessages)
                //{
                // Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalMessage.Sender.Id, dalMessage.Sender.Name, dalMessage.Sender.Surname);
                //}

                //    repository.Create(new DalUser(){UserName = "User3",Name = "User3", Surname = "Surname3", PasswordHash = "333"});
                //    socialNetworkDatabase.SaveChanges();
                //    DalUser user = repository.GetById(4);
                //    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", user.Id, user.Name,
                //        user.Surname);
                foreach (var dalUser in repository.GetAll())
                {
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalUser.Id, dalUser.Name, dalUser.Surname);
                }

                Console.ReadLine();
                }
            }

            using (SocialNetworkDatabase socialNetworkDatabase = new SocialNetworkDatabase())
            {
                using (UserRepository repository = new UserRepository(socialNetworkDatabase))
                {
                    /*DalUser user = repository.GetById(1);
                Console.WriteLine("Id {0}\nName {1}\nSurname {2}", user.Id,user.Name,user.Surname);
                foreach (var dalUser in user.Friends)
                {
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalUser.Id, dalUser.Name, dalUser.Surname);
                }

                foreach (var dalMessage in user.GottenMessages)
                {
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalMessage.Sender.Id, dalMessage.Sender.Name, dalMessage.Sender.Surname);
                }*/

                   /* repository.Create(new DalUser() { UserName = "User3", Name = "User3", Surname = "Surname3", PasswordHash = "333" });
                    socialNetworkDatabase.SaveChanges();*/
                    DalUser user = repository.GetById(4);
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", user.Id, user.Name,
                        user.Surname);
                    /*  foreach (var dalUser in repository.GetAll())
                {
                    Console.WriteLine("Id {0}\nName {1}\nSurname {2}", dalUser.Id, dalUser.Name, dalUser.Surname);
                }*/


                }
            }
            Console.ReadLine();

        }
    }
}
