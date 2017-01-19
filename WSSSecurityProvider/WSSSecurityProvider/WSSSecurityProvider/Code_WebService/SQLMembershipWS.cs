using System;
using System.Web.Services;
using System.Web.Configuration;
using System.Web.Security;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyLocalBroadband.Logging;

namespace MyLocalBroadband.WSSSecurityProvider.WebServices
{
    [WebService(Namespace = "http://www.mylocalbroadband.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SQLMembershipService : System.Web.Services.WebService
    {
        #region Member Data
        private const string _CONNECTIONSTRINGNAME = "SecProviderConnectionString";
        #endregion

        #region Web Methods - User Management
        /// <summary>
        /// Method to create new user for an application
        /// </summary>
        /// <param name="appName">Membership Provider application name.</param>
        /// <param name="userName">Logon name of the new user to create.</param>
        /// <param name="userPassword">Password of the new user to create.</param>
        /// <param name="userEmail">Email address of the new user to create.</param>
        /// <param name="userPasswordQuestion">The password-question value for the membership user.</param>
        /// <param name="userPasswordAnswer">The password-answer value for the membership user.</param>
        [WebMethod]
        public void CreateUser(string appName, string userName, string userPassword, string userEmail, string userPasswordQuestion, string userPasswordAnswer, bool MappedApplication)
        {
            MembershipCreateStatus userStatus = new MembershipCreateStatus();
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;

                // Create the user in the membership directory
                Membership.CreateUser(userName, userPassword, userEmail, userPasswordQuestion, userPasswordAnswer, true, out userStatus);

                // Check the Status
                if (userStatus != MembershipCreateStatus.Success)
                {
                    throw new Exception("User " + userName + " failed with status " + userStatus.ToString());
                }
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }
        /// <summary>
        /// method to delete a user from a given application
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="userName"></param>
        [WebMethod]
        public void DeleteUser(string appName, string userName, bool MappedApplication)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;

                // Delete the user in the membership database
                DeleteUser(userName);

            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }
        #endregion

        #region Web Methods - Password Management
        /// <summary>
        /// change the user password
        /// </summary>
        /// <param name="appName">sql membership provider application name</param>
        /// <param name="userName">user login</param>
        /// <param name="oldPassword">old user password</param>
        /// <param name="newPassword">new user password</param>
        [WebMethod]
        public void ChangePassword(string appName, string userName, string oldPassword, string newPassword, bool MappedApplication)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;
                ChangePassword(userName, oldPassword, newPassword);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }
        /// <summary>
        /// Reset the users password
        /// </summary>
        /// <param name="appName">sql membership provider application name</param>
        /// <param name="userName">user login</param>
        [WebMethod]
        public string ResetUserPassword(string appName, string userName, bool MappedApplication)
        {
            string oldApp = Membership.ApplicationName;
            string newPassword = string.Empty;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;

                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    ULS.LogError("SQLMembershipWS", "No user found for " + appName + ":" + userName, "SQLMembershipWS");
                    throw new Exception("User (" + userName + ") Could Not Be Found");
                }

                newPassword = user.ResetPassword();
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return newPassword;
        }
        #endregion

        #region Web Methods - User Interrogation
        /// <summary>
        /// Method to get a list of users of a specified application
        /// </summary>
        /// <param name="appName">sql membership provider application name</param>
        /// <returns></returns>
        [WebMethod]
        public string GetUserList(string appName, bool MappedApplication)
        {
            StringBuilder userXml = new StringBuilder();
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;

                userXml.Append("<users>");
                // get users for the given application
                MembershipUserCollection mbrColl = Membership.GetAllUsers();
                foreach (MembershipUser user in mbrColl)
                {
                    userXml.Append("<user>");
                    userXml.Append("<login>");
                    userXml.Append(user.UserName);
                    userXml.Append("</login>");
                    userXml.Append("<email>");
                    userXml.Append(user.Email);
                    userXml.Append("</email>");
                    userXml.Append("</user>");
                }
                userXml.Append("</users>");
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return userXml.ToString();
        }
        /// <summary>
        /// find a specific user and return user information
        /// </summary>
        /// <param name="appName">sql membership provider application name</param>
        /// <param name="userName">user login</param>
        /// <returns>login and email address</returns>
        [WebMethod]
        public string FindUser(string appName, string userName, bool MappedApplication)
        {
            StringBuilder userXml = new StringBuilder();
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Membership.ApplicationName = GetMapping(appName);
                else
                    Membership.ApplicationName = appName;
                MembershipUser user = Membership.GetUser(userName);
                if (user == null)
                {
                    ULS.LogError("FindUser", "No user found for " + appName + ":" + userName, "SQLMembershipWS");
                    throw new Exception("User (" + userName + ") not found");
                }

                userXml.Append("<user>");
                userXml.Append("<login>");
                userXml.Append(user.UserName);
                userXml.Append("</login>");
                userXml.Append("<email>");
                userXml.Append(user.Email);
                userXml.Append("</email>");
                userXml.Append("</user>");
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return userXml.ToString();
        }
        #endregion

        #region Web Methods - Site Map Management
        /// <summary>
        /// creates the site mapping entry for the site
        /// </summary>
        /// <param name="appName">sql membership provider application name</param>
        /// <param name="siteURL">The URL of the site</param>
        [WebMethod]
        public void CreateSiteMapping(string appName, string siteURL)
        {
            string sqlConnectionString = GetConnectionString();
            string fqdn = GetFQDN(siteURL);

            // Create an entry in Sitemaps database to map this Application to this domain name (fqdn)
            CreateMapping(appName, fqdn, sqlConnectionString);
        }

        /// <summary>
        /// Returns mapped app name from fqdn
        /// </summary>
        /// <param name="siteURL">The URL of the site</param>
        [WebMethod]
        public string GetSiteMapping(string siteURL)
        {
            string fqdn = GetFQDN(siteURL);

            // find entry in Sitemaps database for this domain name (fqdn)
            return GetMapping(fqdn);
        }
        #endregion

        #region Web Methods - Application Name Management
        /// <summary>
        /// Set the application name for the membership provider
        /// </summary>
        /// <param name="appName">Membership Provider application name.</param>
        [WebMethod]
        public void SetApplicationName(string appName)
        {
            Membership.ApplicationName = appName;
        }
        /// <summary>
        /// gets the application name for the membership provider
        /// </summary>
        /// <returns>Membership Provider application name.</returns>
        [WebMethod]
        public string GetApplicationName()
        {
            return Membership.ApplicationName;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create a new user in the membership directory
        /// </summary>
        /// <param name="login">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password-question value for the membership user.</param>
        /// <param name="passwordAnswer">The password-answer value for the membership user.</param>
        private static void CreateUser(string login, string password, string email, string passwordQuestion, string passwordAnswer)
        {
            MembershipCreateStatus userStatus = new MembershipCreateStatus();

            // Create the user in the SQL membership database
            Membership.CreateUser(login, password, email, passwordQuestion, passwordAnswer, true, out userStatus);

            // Check the Status
            if (userStatus != MembershipCreateStatus.Success)
            {
                ULS.LogError("CreateUser", "User " + login + " failed with status " + userStatus.ToString(), "SQLMembershipWS");
                throw new Exception("User " + login + " failed with status " + userStatus.ToString());
            }
        }
        /// <summary>
        /// Delete a user from the membership directory
        /// </summary>
        /// <param name="UserName">The user name of the user to delete</param>
        private static void DeleteUser(string UserName)
        {
            bool bDeleted = false;
            bDeleted = Membership.DeleteUser(UserName, true);
            if (bDeleted == false)
            {
                ULS.LogError("DeleteUser", "The user could not be deleted: " + UserName, "SQLMembershipWS");
                throw new Exception("The user could not be deleted.");
            }
        }
        /// <summary>
        /// changes the user's password
        /// </summary>
        /// <param name="UserName">user name</param>
        /// <param name="OldPassword">old password</param>
        /// <param name="NewPassword">new password</param>
        private static void ChangePassword(string UserName, string OldPassword, string NewPassword)
        {
            MembershipProvider defaultProvider;
            bool bChanged = false;

            defaultProvider = Membership.Provider;

            if (defaultProvider.EnablePasswordReset)
            {
                bChanged = defaultProvider.ChangePassword(UserName, OldPassword, NewPassword);
                if (bChanged == false)
                {
                    ULS.LogError("ChangePassword", "Password Not Changed: " + UserName, "SQLMembershipWS");
                    throw new Exception("This password could not be changed.");
                }
            }
            else
            {
                // This provider is not configured to let users change their passwords
                throw new Exception("You do not have permissions to change your password.");
            }
        }
        /// <summary>
        /// Creates a mapping between an application name and a fully qualified domain name
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="fqdn">The fully qualified domain name.</param>
        /// <param name="connString">The connection string for this membership provider.</param>
        private static void CreateMapping(string appName, string fqdn, string connString)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(connString);
                command = new SqlCommand("dbo.aspnet_Sitemaps_CreateMapping", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter1 = command.Parameters.Add("@ApplicationName", SqlDbType.NVarChar);
                SqlParameter parameter2 = command.Parameters.Add("@FQDN", SqlDbType.NVarChar);
                parameter1.Value = appName;
                parameter2.Value = fqdn;
                connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                if (connection != null)
                    connection.Dispose();
                if (command != null)
                    command.Dispose();
            }
        }

        /// <summary>
        /// Deletes the mapping between an application name and a fully qualified domain name
        /// </summary>
        /// <param name="fqdn">The fully qualified domain name.</param>
        /// <param name="connString">The connection string for this membership provider.</param>
        private static void DeleteMapping(string fqdn, string connString)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(connString);
                command = new SqlCommand("dbo.aspnet_Sitemaps_DeleteMapping", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = command.Parameters.Add("@FQDN", SqlDbType.NVarChar);
                parameter.Value = fqdn;
                connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                if (connection != null)
                    connection.Dispose();
                if (command != null)
                    command.Dispose();
            }
        }
        /// <summary>
        /// Get the Fully Qualified Domain Name based on the specified URL
        /// </summary>
        /// <param name="args">Array of arguments</param>
        /// <returns>Fully Qualified Domain Name</returns>
        private string GetFQDN(string siteUrl)
        {
            string sUrl = siteUrl;

            try
            {
                string protocol = "https://";
                if (sUrl.StartsWith(protocol, StringComparison.InvariantCultureIgnoreCase))
                {
                    sUrl = siteUrl.Replace(protocol, string.Empty);
                }
                protocol = "http://";
                if (sUrl.StartsWith(protocol, StringComparison.InvariantCultureIgnoreCase))
                {
                    sUrl = siteUrl.Replace(protocol, string.Empty);
                }
            }
            catch
            {
            }

            return sUrl;
        }

        /// <summary>
        /// Get the connection string from the app.config file
        /// </summary>
        /// <param name="configPath">The path to the configuration section for the membership provider.</param>
        /// <returns>The connection string for this membership provider.</returns>

        private string GetConnectionString()
        {
            string connStr = string.Empty;
            string tempConnStringName;

            // We assume that there is a connection string called SQLConnectionString in this web.config file.
            // This would refer to the web.config of central admin.
            tempConnStringName = _CONNECTIONSTRINGNAME;

            // Get the connection string from the web config
            connStr = WebConfigurationManager.ConnectionStrings[tempConnStringName].ConnectionString;

            // Return the connection string
            return connStr;
        }

        /// <summary>Get the Membership provider name from the web.config file</summary>
        /// <returns>Name of the membership provider</returns>
        private string GetMembershipProviderName()
        {
            string providerName = string.Empty;

            // Get the provider name from the current Membership object
            providerName = Membership.Provider.Name;

            // Return the provider name
            return providerName;
        }

        /// <summary>
        /// Get the application name that corresponds to the fully qualified domain name.
        /// </summary>
        /// <param name="fqdn">The fully qualified domain name.</param>
        /// <param name="connString">The connection string for this provider.</param>
        /// <returns>The application name.</returns>
        private string GetMapping(string fqdn)
        {
            string appName = string.Empty;
            string connString = GetConnectionString();
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(connString);
                command = new SqlCommand("dbo.aspnet_Sitemaps_GetApplicationNameByFQDN", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = command.Parameters.Add("@fqdn", SqlDbType.NVarChar);
                parameter.Value = GetFQDN(fqdn);
                connection.Open();
                appName = command.ExecuteScalar() as string;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                if (connection != null)
                    connection.Dispose();
                if (command != null)
                    command.Dispose();
            }
            return appName;
        }
        #endregion
    }
}
