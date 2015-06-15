using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using NLog;
using SocialNetwork.Bll.Interface.Entity;
using SocialNetwork.Bll.Interface.Services;

namespace WebUi.Providers
{
    public class UserMembershipProvider : MembershipProvider
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUserService userService;

        public MembershipUser CreateUser(string username, string password, string name,
            string surname, string aboutUser, DateTime? birthDay, BllSex? sex)
        {
            MembershipUser membershipUser = GetUser(username, false);
            if (membershipUser != null)
            {
                Logger.Debug("Try create user with exsist username = {0}", username);
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
            Logger.Trace("User username = {0} created", username);
            return membershipUser;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            userService =
               (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                Logger.Debug("Try change password not exist user username = {0}", username);
                return false;
            }

            if (!Crypto.VerifyHashedPassword(bllUser.PasswordHash, oldPassword))
            {
                Logger.Debug("Try change password with incorrect password user username = {0}", username);
                return false;
            }
            bllUser.PasswordHash = Crypto.HashPassword(newPassword);
            userService.Update(bllUser);
            Logger.Trace("Password cnahged. Username = {0}", username);
            return true;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            userService =
                (IUserService) DependencyResolver.Current.GetService(typeof (IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                Logger.Debug("Try get not exist user username = {0}", username);
                return null;
            }

            return new MembershipUser("UserMembershipProvider", bllUser.UserName, bllUser.Id, null,
                null,
                null, false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue);

        }

        


        public override bool ValidateUser(string username, string password)
        {
            userService =
                (IUserService) DependencyResolver.Current.GetService(typeof (IUserService));

            BllUser bllUser = userService.GetByName(username);
            if (bllUser == null)
            {
                Logger.Debug("Try validate not exist user username = {0}", username);
                return false;
            }
            if (Crypto.VerifyHashedPassword(bllUser.PasswordHash, password)) return true;
            Logger.Debug("User validate fault username = {0}", username);
            return false;

        }


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