using System;
using System.IO;
using System.Xml;
using System.Web.Services;
using System.Web.Configuration;

namespace MyLocalBroadband.WSSSecurityProvider.WebServices
{
    [WebService(Namespace = "http://www.mylocalbroadband.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HostingTools : System.Web.Services.WebService
    {
        #region Constructors
        public HostingTools()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        #endregion

        #region Web Methods - Site Creation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteUrl">new site collection i.e. http://www.sitea.com</param>
        /// <param name="SiteTitle">sitecollection title</param>
        /// <param name="SiteDescription">sitecollection description</param>
        /// <param name="SiteTemplate">site template i.e. STS#0</param>
        /// <param name="OwnerFirstName">user first name</param>
        /// <param name="OwnerLastName">user last name</param>
        /// <param name="OwnerLogin">user login</param>
        /// <param name="OwnerPassword">password i.e. pass@word1</param>
        /// <param name="PwdQuestion">question i.e. color</param>
        /// <param name="PwdAnswer">answer i.e. red</param>
        /// <param name="OwnerEmail">email address</param>
        /// <param name="AppName">application name for SQLMembership</param>
        /// <param name="WebAppUrl">Web App i.e. http://moss </param>
        /// <param name="ProviderName">what's in web.config i.e. aspnetsqlmembershipprovider</param>
        /// <param name="nLCID">language i.e. 1033 </param>
        [WebMethod]
        public void CreateNewSiteNewSQLOwner(string SiteUrl,
                                            string SiteTitle,
                                            string SiteDescription,
                                            string SiteTemplate,
                                            string OwnerFirstName,
                                            string OwnerLastName,
                                            string OwnerLogin,
                                            string OwnerPassword,
                                            string PwdQuestion,
                                            string PwdAnswer,
                                            string OwnerEmail,
                                            string AppName,
                                            string WebAppUrl,
                                            string ProviderName,
                                            uint nLCID)
        {
            SQLMembershipService SQLService = null;
            SharePointService SPService = null;
            try
            {
                SQLService = new SQLMembershipService();
                SPService = new SharePointService();

                string siteownerLogin = ProviderName + ":" + OwnerLogin;
                string siteownerName = OwnerFirstName + " " + OwnerLastName;
                string question = PwdQuestion;
                string answer = PwdAnswer;

                // create SQL user
                SQLService.CreateUser(AppName, OwnerLogin,
                                                OwnerPassword, OwnerEmail, question, answer, false);

                // create mapping for new site
                SQLService.CreateSiteMapping(AppName, SiteUrl);

                // set application name for sqlmembership provider.
                string oldAppName = SQLService.GetApplicationName();
                SQLService.SetApplicationName(AppName);

                //create site with user as site owner
                SPService.hstCreateSite(WebAppUrl, SiteUrl, SiteTitle, SiteDescription, nLCID, SiteTemplate,
                                siteownerLogin, siteownerName, OwnerEmail, null, null, null, true);

                // restore application name for sqlmembership provider.
                SQLService.SetApplicationName(oldAppName);
            }
            finally
            {
                if (SQLService != null)
                    SQLService.Dispose();
                if (SPService != null)
                    SPService.Dispose();
            }

        }
        /// <summary>
        /// creates a new site a and makes an existing sql user the site owner
        /// </summary>
        /// <param name="SiteUrl">The URL of the site</param>
        /// <param name="SiteTitle">Title of the new site</param>
        /// <param name="SiteDescription">Description of the site</param>
        /// <param name="SiteTemplate">Site template to create site with</param>
        /// <param name="OwnerLogin">User login for the site owner</param>
        /// <param name="AppName">SQL membership provider's ApplicationName</param>
        /// <param name="WebAppUrl">Host header parent URL to create this new site in</param>
        /// <param name="ProviderName">Name of the sql provider used in the web.config file</param>
        /// <param name="nLCID"></param>
        [WebMethod]
        public void CreateNewSiteExistingSQLOwner(string SiteUrl,
                                            string SiteTitle,
                                            string SiteDescription,
                                            string SiteTemplate,
                                            string OwnerLogin,
                                            string AppName,
                                            string WebAppUrl,
                                            string ProviderName,
                                            uint nLCID)
        {
            SQLMembershipService SQLService = null;
            SharePointService SPService = null;
            try
            {
                SQLService = new SQLMembershipService();
                SPService = new SharePointService();
                string siteownerLogin = ProviderName + ":" + OwnerLogin;
                string userEmail = String.Empty;

                // Validate sql user
                string userXml = SQLService.FindUser(AppName, OwnerLogin, false);
                if (userXml == string.Empty)
                    throw new Exception("User: " + OwnerLogin + " does not exists");

                XmlDocument xmlDoc = new XmlDocument();
                // Try to load the user xml into an XML document
                xmlDoc.LoadXml(userXml);

                XmlNode userNode = xmlDoc.FirstChild;

                // extract the full name and email address
                for (int i = 0; i < userNode.ChildNodes.Count; i++)
                {
                    if (userNode.ChildNodes[i].Name == "email")
                    {
                        userEmail = userNode.ChildNodes[i].InnerText;
                    }
                }

                // set application name for sqlmembership provider. This needs to be set to create a site
                string oldAppName = SQLService.GetApplicationName();
                SQLService.SetApplicationName(AppName);

                // create site with user as site owner
                SPService.hstCreateSite(WebAppUrl, SiteUrl, SiteTitle, SiteDescription, nLCID, SiteTemplate,
                            siteownerLogin, userEmail, null, null, null, null, true);

                // create mapping for new site
                SQLService.CreateSiteMapping(AppName, SiteUrl);

                // restore application name for sqlmembership provider.
                SQLService.SetApplicationName(oldAppName);
            }
            finally
            {
                if (SQLService != null)
                    SQLService.Dispose();
                if (SPService != null)
                    SPService.Dispose();
            }

        }
        #endregion

        #region Web Methods - Host File Management
        /// <summary>
        /// Appends a host file entry
        /// </summary>
        /// <param name="domainName">domain name</param>
        /// <param name="IPAddress">IP address</param>
        [WebMethod]
        public void CreateDNS(string domainName, string IPAddress)
        {
            string hostfile = WebConfigurationManager.AppSettings.Get("HostsFile");
            string hostentry = IPAddress + "     " + domainName;
            using (StreamWriter writer = File.AppendText(hostfile))
            {
                writer.WriteLine(hostentry);
            }
        }
        /// <summary>
        /// Checks to see if the DNS entry exists in the host file
        /// </summary>
        /// <param name="domainName">domain name</param>
        /// <returns></returns>
        [WebMethod]
        public bool DNSExists(string domainName)
        {
            string hostfile = WebConfigurationManager.AppSettings.Get("HostsFile");
            using (StreamReader reader = File.OpenText(hostfile))
            {
                string hostentry = "";
                while ((hostentry = reader.ReadLine()) != null)
                {
                    if (hostentry.Contains(domainName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Unused Methods
        //These methods were in the original Hosting starter kit
        //but are unused in my version.  They may be useful in the future
        //but I didn't want to put them in and worry about them not working.

        ///// <summary>
        ///// Method to create a new sql user and add the user to and existing site
        ///// </summary>
        ///// <param name="SiteUrl">The URL of the site</param>
        ///// <param name="OwnerFirstName">User first name</param>
        ///// <param name="OwnerLastName">User last name</param>
        ///// <param name="OwnerLogin">User login for the site owner</param>
        ///// <param name="OwnerPassword">User password</param>
        ///// <param name="PwdQuestion">password question</param>
        ///// <param name="PwdAnswer">password answer</param>
        ///// <param name="OwnerEmail">user email address</param>
        ///// <param name="AppName">SQL membership provider's ApplicationName</param>
        ///// <param name="WebAppUrl">Host header parent URL to create this new site in</param>
        ///// <param name="ProviderName">Name of the sql provider used in the web.config file</param>
        //[WebMethod]
        //public void AddNewSQLUsertoSite(string SiteUrl,
        //                                    string OwnerFirstName,
        //                                    string OwnerLastName,
        //                                    string OwnerLogin,
        //                                    string OwnerPassword,
        //                                    string PwdQuestion,
        //                                    string PwdAnswer,
        //                                    string OwnerEmail,
        //                                    string AppName,
        //                                    string WebAppUrl,
        //                                    string ProviderName)
        //{
        //    SQLMembershipService SQLService = new SQLMembershipService();
        //    SharePointService SPService = new SharePointService();
        //    string userIdentity = ProviderName + ":" + OwnerLogin;
        //    string userName = OwnerFirstName + " " + OwnerLastName;
        //    string question = "Color";  // hard coded for Greg's Starterkit
        //    string answer = "Red";      // hard coded for Greg's Starterkit
        //    try
        //    {

        //        // create SQL user
        //        SQLService.CreateUser(AppName, OwnerLogin,
        //                                        OwnerPassword, OwnerEmail, question, answer, false);

        //        // Add the user to the WSS site collection as Reader
        //        SPService.hstAddUsertoSite(SiteUrl, userIdentity, userName, OwnerEmail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        ///// <summary>
        ///// Add and existing SQL user to an existing site
        ///// </summary>
        ///// <param name="SiteUrl">The URL of the site</param>
        ///// <param name="OwnerLogin">User login for the site owner</param>
        ///// <param name="AppName">SQL membership provider's ApplicationName</param>
        ///// <param name="WebAppUrl">Host header parent URL to create this new site in</param>
        ///// <param name="ProviderName">Name of the sql provider used in the web.config file</param>
        //[WebMethod]
        //public void AddSQLUsertoSite(string SiteUrl,
        //                                    string OwnerLogin,
        //                                    string AppName,
        //                                    string WebAppUrl,
        //                                    string ProviderName)
        //{
        //    SQLMembershipService SQLService = new SQLMembershipService();
        //    SharePointService SPService = new SharePointService();
        //    string siteownerLogin = ProviderName + ":" + OwnerLogin;
        //    string userEmail = String.Empty;

        //    try
        //    {
        //        // Get user information
        //        string userXml = SQLService.FindUser(AppName, OwnerLogin, false);
        //        if (userXml == string.Empty)
        //            throw new Exception("User: " + OwnerLogin + " does not exists");

        //        XmlDocument xmlDoc = new XmlDocument();
        //        // load the user xml into an XML document
        //        xmlDoc.LoadXml(userXml);
        //        XmlNode userNode = xmlDoc.FirstChild;

        //        // extract the email address
        //        for (int i = 0; i < userNode.ChildNodes.Count; i++)
        //        {
        //            if (userNode.ChildNodes[i].Name == "email")
        //            {
        //                userEmail = userNode.ChildNodes[i].InnerText;
        //            }
        //        }

        //        // set application name for sqlmembership provider. This needs to be set to create a site
        //        string oldAppName = SQLService.GetApplicationName();
        //        SQLService.SetApplicationName(AppName);

        //        // create site with user as site owner
        //        SPService.hstAddUsertoSite(SiteUrl, siteownerLogin, null, userEmail);

        //        // restore application name for sqlmembership provider.
        //        SQLService.SetApplicationName(oldAppName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        #endregion
    }
}
