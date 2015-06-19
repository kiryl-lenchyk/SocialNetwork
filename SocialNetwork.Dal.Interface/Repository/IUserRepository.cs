using System;
using SocialNetwork.Dal.Interface.DTO;

namespace SocialNetwork.Dal.Interface.Repository
{
    /// <summary>
    /// Represent storage of DalUser
    /// </summary>
    public interface IUserRepository : IRepository<DalUser>
    {
        /// <summary>
        /// Get user by name, or null if role not found.
        /// </summary>
        /// <param name="name">user name for search</param>
        /// <returns>founded user, or null if it's not found.</returns>
        DalUser GetByName(String name);

        /// <summary>
        /// Mark two users as friend.
        /// </summary>
        /// <param name="currentUser">first user to be friend.</param>
        /// <param name="newFriend">second user to be friend.</param>
        void AddToFriends(DalUser currentUser, DalUser newFriend);

        /// <summary>
        /// Mark that users are not friends.
        /// </summary>
        /// <param name="currentUser">first user to delete from friends.</param>
        /// <param name="newFriend">second user to delete from friends.</param>
        void RemoveFriend(DalUser currentUser, DalUser newFriend);
    }
}
