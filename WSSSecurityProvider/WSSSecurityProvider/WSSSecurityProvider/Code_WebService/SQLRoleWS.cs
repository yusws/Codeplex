using System;
using System.Web.Services;
using System.Web.Configuration;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;

namespace MyLocalBroadband.WSSSecurityProvider.WebServices
{
    [WebService(Namespace = "http://www.mylocalbroadband.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SQLRoleService : System.Web.Services.WebService
    {
        #region Member Data
        private const string _CONNECTIONSTRINGNAME = "SecProviderConnectionString";
        #endregion

        #region Web Methods - Adding Users To Roles
        [WebMethod]
        public void AddUsersToRole(string appName, bool MappedApplication, string[] usernames, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.AddUsersToRole(usernames, roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void AddUsersToRoles(string appName, bool MappedApplication, string[] usernames, string[] roleNames)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.AddUsersToRoles(usernames, roleNames);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void AddUserToRole(string appName, bool MappedApplication, string username, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.AddUserToRole(username, roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void AddUserToRoles(string appName, bool MappedApplication, string username, string[] roleNames)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.AddUserToRoles(username, roleNames);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }
        #endregion

        #region Web Methods - Role Management
        [WebMethod]
        public bool RoleExists(string appName, bool MappedApplication, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            bool ret = false;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.RoleExists(roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }
        [WebMethod]
        public string[] GetAllRoles(string appName, bool MappedApplication)
        {
            string oldApp = Membership.ApplicationName;
            string[] ret = null;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.GetAllRoles();
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }
        [WebMethod]
        public void CreateRole(string appName, bool MappedApplication, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.CreateRole(roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public bool DeleteRole(string appName, bool MappedApplication, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            bool ret = false;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.DeleteRole(roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }
        #endregion

        #region Web Methods - Role Interrogation
        [WebMethod]
        public string[] FindUsersInRole(string appName, bool MappedApplication, string roleName, string usernameToMatch)
        {
            string oldApp = Membership.ApplicationName;
            string[] ret = null;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.FindUsersInRole(roleName, usernameToMatch);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }

        [WebMethod]
        public string[] GetRolesForUser(string appName, bool MappedApplication, string username)
        {
            string oldApp = Membership.ApplicationName;
            string[] ret = null;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.GetRolesForUser(username);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }

        [WebMethod]
        public string[] GetUsersInRole(string appName, bool MappedApplication, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            string[] ret = null;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.GetUsersInRole(roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }

        [WebMethod]
        public bool IsUserInRole(string appName, bool MappedApplication, string username, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            bool ret = false;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                ret = Roles.IsUserInRole(username, roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
            return ret;
        }
        #endregion

        #region Web Methods - Remove Users from Roles
        [WebMethod]
        public void RemoveUserFromRole(string appName, bool MappedApplication, string username, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.RemoveUserFromRole(username, roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void RemoveUserFromRoles(string appName, bool MappedApplication, string username, string[] roleNames)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.RemoveUserFromRoles(username, roleNames);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void RemoveUsersFromRole(string appName, bool MappedApplication, string[] usernames, string roleName)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.RemoveUsersFromRole(usernames, roleName);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }

        [WebMethod]
        public void RemoveUsersFromRoles(string appName, bool MappedApplication, string[] usernames, string[] roleNames)
        {
            string oldApp = Membership.ApplicationName;
            try
            {
                if (MappedApplication)
                    Roles.ApplicationName = GetMapping(appName);
                else
                    Roles.ApplicationName = appName;

                Roles.RemoveUsersFromRoles(usernames, roleNames);
            }
            finally
            {
                Membership.ApplicationName = oldApp;
            }
        }
        #endregion

        #region Web Methods - Site Mapping Management
        /// <summary>
        /// creates the site mapping entry for the site
        /// </summary>
        /// <param name="appName">sql Role provider application name</param>
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

        #region Private Methods
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

        /// <summary>Get the Role provider name from the web.config file</summary>
        /// <returns>Name of the Role provider</returns>
        private string GetRoleProviderName()
        {
            string providerName = string.Empty;

            // Get the provider name from the current Role object
            providerName = Roles.Provider.Name;

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
