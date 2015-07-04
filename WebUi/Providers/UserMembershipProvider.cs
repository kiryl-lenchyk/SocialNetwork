using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Logger.Interface;

namespace WebUi.Providers
{
    /// <summary>
    /// MembershipProvider for SocialNetwork
    /// </summary>
    public class UserMembershipProvider : MembershipProvider
    {

        #region Fields

        private IUserService userService;
        private ILogger logger;

        #endregion

        #region Realized Methods

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="username">username for login</param>
        /// <param name="password">user's password</param>
        /// <param name="name">name of user</param>
        /// <param name="surname">user's surname</param>
        /// <param name="aboutUser">information about user</param>
        /// <param name="birthDay">user's birthday</param>
        /// <param name="sex">user's sex</param>
        /// <returns>created MembershipUser</returns>
        public MembershipUser CreateUser(string username, string password, string name,
            string surname, string aboutUser, DateTime? birthDay, BllSex? sex)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));

            MembershipUser membershipUser = GetUser(username, false);
            if (membershipUser != null)
            {
                logger.Log(LogLevel.Debug,"Try create user with exsist username = {0}", username);
                return null;
            }


            userService =
                (IUserService) DependencyResolver.Current.GetService(typeof (IUserService));

            BllUser newUser = userService.Create(new BllUser()
            {
                UserName = username,
                Name = name,
                Surname = surname,
                AboutUser = aboutUser,
                BirthDay = birthDay,
                Sex = sex,
                PasswordHash = Crypto.HashPassword(password),
            });

            membershipUser = GetUser(username, false);
            logger.Log(LogLevel.Trace,"User username = {0} created", username);
            return membershipUser;
        }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        /// <param name="username">The user to update the password for. </param><param name="oldPassword">The current password for the specified user. </param><param name="newPassword">The new password for the specified user. </param>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            userService =
               (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                logger.Log(LogLevel.Debug,"Try change password not exist user username = {0}", username);
                return false;
            }

            if (!Crypto.VerifyHashedPassword(bllUser.PasswordHash, oldPassword))
            {
                logger.Log(LogLevel.Debug,"Try change password with incorrect password user username = {0}", username);
                return false;
            }
            bllUser.PasswordHash = Crypto.HashPassword(newPassword);
            userService.Update(bllUser);
            logger.Log(LogLevel.Trace,"Password cnahged. Username = {0}", username);
            return true;
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        /// <param name="username">The name of the user to get information for. </param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            userService =
                (IUserService) DependencyResolver.Current.GetService(typeof (IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                logger.Log(LogLevel.Debug,"Try get not exist user username = {0}", username);
                return null;
            }

            return new MembershipUser("UserMembershipProvider", bllUser.UserName, bllUser.Id, null,
                null,
                null, false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue);

        }
        
        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        /// <param name="username">The name of the user to validate. </param><param name="password">The password for the specified user. </param>
        public override bool ValidateUser(string username, string password)
        {
            logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));
            userService =
                (IUserService) DependencyResolver.Current.GetService(typeof (IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                logger.Log(LogLevel.Debug,"Try validate not exist user username = {0}", username);
                return false;
            }
            if (Crypto.VerifyHashedPassword(bllUser.PasswordHash, password)) return true;
            logger.Log(LogLevel.Debug,"User validate fault username = {0}", username);
            return false;

        }

        #endregion

        #region Not Implemented

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new System.NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new System.NotImplementedException();
        }


        public override bool UnlockUser(string userName)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new System.NotImplementedException();
        }


        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch,
            int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string ApplicationName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new System.NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}