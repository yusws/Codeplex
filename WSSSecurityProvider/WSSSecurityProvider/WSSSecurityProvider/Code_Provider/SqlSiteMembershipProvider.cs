using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using MyLocalBroadband.Logging;

namespace MyLocalBroadband.WSSSecurityProvider
{
    class SqlSiteMembershipProvider : SqlMembershipProvider
    {
        #region Member Data
        private string _SQLCONNECTIONSTRING;
        private string _PROVIDERNAME;
        #endregion

        #region Public Overrides
        /// <summary>
        /// Initialize the membership provider
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="config">Configuration parameters for this provider</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Store the name of this provider instance
            _PROVIDERNAME = name;

            // Get the connection string.
            _SQLCONNECTIONSTRING = config["connectionStringName"];

            // Initialize this provider
            base.Initialize(name, config);

            // Get the connection string.
            if (string.IsNullOrEmpty(_SQLCONNECTIONSTRING))
                _SQLCONNECTIONSTRING = config["connectionStringName"];
        }

        /// <summary>
        /// The application name associated with this provider.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                string fqdn = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                string connString = this.GetConnectionString(_PROVIDERNAME);
                string appName = string.Empty;

                if (!string.IsNullOrEmpty(connString))
                {
                    appName = this.GetMappedAppName(fqdn, connString);
                }
                else
                {
                    ULS.LogError("SqlSiteMembershipProvider.ApplicationName", "Connection String is empty or null", "Membership");
                    throw new ProviderException("Connection string not defined for membership db.");
                }

                if (string.IsNullOrEmpty(appName))
                {
                    ULS.LogError("SqlSiteMembershipProvider.ApplicationName", "No AppName retrieved from mapping", "Membership");
                    throw new ProviderException(string.Format("SiteMap not defined for {0}.", fqdn));
                }

                return appName;
            }
            set
            {
                base.ApplicationName = value;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the connection string for the provider.
        /// </summary>
        /// <param name="providerName">The name of the provider.</param>
        /// <returns>The connection string.</returns>
        private string GetConnectionString(string providerName)
        {
            string connStr = string.Empty;
            try
            {
                MembershipSection configSection = (MembershipSection)HttpContext.Current.GetSection("system.web/membership");

                ProviderSettings providerItem = configSection.Providers[providerName];
                if (providerItem != null)
                {
                    string connStrName = providerItem.Parameters["connectionStringName"];
                    if (!string.IsNullOrEmpty(connStrName))
                    {
                        connStr = WebConfigurationManager.ConnectionStrings[connStrName].ConnectionString;
                    }
                }
            }
            catch (Exception e)
            {
                ULS.LogError("SQLSiteMembershipProvider.GetConnectionString", "No Connection String Retrieved for provider name: " + providerName + "\n" + e.ToString(), "Membership");
                throw new ProviderException(e.ToString());
            }

            return connStr;
        }

        /// <summary>
        /// Get the application name that corresponds to the fully qualified domain name.
        /// </summary>
        /// <param name="fqdn">The fully qualified domain name.</param>
        /// <param name="connString">The connection string for this provider.</param>
        /// <returns>The application name.</returns>
        private string GetMappedAppName(string fqdn, string connString)
        {
            string appName = string.Empty;
            SqlConnection connection = null;
            SqlCommand command = null;
            try
            {
                connection = new SqlConnection(connString);
                command = new SqlCommand("dbo.aspnet_Sitemaps_GetApplicationNameByFQDN", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = command.Parameters.Add("@fqdn", SqlDbType.NVarChar);
                parameter.Value = fqdn;

                // elevate permissions to allow user to read database.
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    connection.Open();
                    appName = command.ExecuteScalar() as string;
                });
                if (string.IsNullOrEmpty(appName))
                    ULS.LogMessage("SQLSiteMembershipProvider.GetMappedAppName", "Could Not Get Mapped Application Name for FQDN: " + fqdn, "Membership", TraceProvider.TraceSeverity.CriticalEvent);
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
