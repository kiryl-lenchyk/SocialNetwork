using System;
using System.Collections.Generic;
using System.IO;
using SocialNetwork.Bll.Interface.Entity;

namespace SocialNetwork.Bll.Interface.Services
{

    /// <summary>
    /// Represent business logic function for Users 
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="key">user id.</param>
        /// <returns>fiunded BllUser or null, if it's not found.</returns>
        BllUser GetById(int key);

        /// <summary>
        /// Get user by name.
        /// </summary>
        /// <param name="name">user name.</param>
        /// <returns>founded BllUser or null, if it's not found.</returns>
        BllUser GetByName(String name);

        /// <summary>
        /// Save new user to storage.
        /// </summary>
        /// <param name="e">new user to save. Id will be selected by storage.</param>
        /// <returns>added user with selected by storage id.</returns>
        BllUser Create(BllUser e);

        /// <summary>
        /// Mark two users as friends.
        /// </summary>
        /// <param name="currentUserId">id of first friend.</param>
        /// <param name="newFriendId">id of second friend.</param>
        void AddFriend(int currentUserId, int newFriendId);

        /// <summary>
        /// Mark that two user are not fiends.
        /// </summary>
        /// <param name="currentUserId">id of first friend.</param>
        /// <param name="newFriendId">id of second friend.</param>
        void RemoveFriend(int currentUserId, int newFriendId);

        /// <summary>
        /// Find users by parametrs.
        /// </summary>
        /// <param name="name">user's name. If it's null search will not use name.</param>
        /// <param name="surname">user's surname. If it's null search will not use surname.</param>
        /// <param name="birthDayMin">min value of user's birthday. If it's null search will not use min value for birthday.</param>
        /// <param name="birthDayMax">max value of user's birthday. If it's null search will not use max value for birthday.</param>
        /// <param name="sex">user's sex. If it's null search will not use sex.</param>
        /// <returns>IEnumerable of founded users.</returns>
        IEnumerable<BllUser> FindUsers(string name, string surname, DateTime? birthDayMin,
            DateTime? birthDayMax, BllSex? sex);

        /// <summary>
        /// Get all users from storage.
        /// </summary>
        /// <returns>IEnumerable of all users.</returns>
        IEnumerable<BllUser> GetAllUsers();

        /// <summary>
        /// Check is user existst by username.
        /// </summary>
        /// <param name="userName">username to found</param>
        /// <returns>true if user with determinated name existst, false else</returns>
        bool IsUserExists(String userName);

        /// <summary>
        /// Check is user existst by id.
        /// </summary>
        /// <param name="id">id to found</param>
        /// <returns>true if user with determinated id existst, false else</returns>
        bool IsUserExists(int id);

        /// <summary>
        /// Remove user from storage.
        /// </summary>
        /// <param name="e">user to delete. User will founded by id.</param>
        void Delete(BllUser e);

        /// <summary>
        /// Update user information.
        /// </summary>
        /// <param name="e">new value of user information. User will be found by id.</param>
        void Update(BllUser e);

        /// <summary>
        /// Set new or update exists avatar for user.
        /// </summary>
        /// <param name="userId">user id for set avatar.</param>
        /// <param name="avatarStream">stream contains new avatar as image.</param>
        void SetUserAvatar(int userId, Stream avatarStream);

        /// <summary>
        /// Get avatar for user.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>stream contains avatar as image.</returns>
        Stream GetUserAvatarStream(int userId);

        /// <summary>
        /// Return true if userId can add friendId to friends and false else.
        /// </summary>
        /// <param name="userId">id of first new friend.</param>
        /// <param name="friendId">id of second new friend.</param>
        /// <returns>true if userId can add friendId to friends and false else.</returns>
        bool CanUserAddToFriends(int userId, int friendId);

        /// <summary>
        /// Return true if senderId can write message to targetId and false else.
        /// </summary>
        /// <param name="targetId">id of message target.</param>
        /// <param name="senderId">id of message sender.</param>
        /// <returns>true if senderId can write message to targetId and false else.</returns>
        bool CanUserWriteMessage(int targetId, int senderId);

    }
}
