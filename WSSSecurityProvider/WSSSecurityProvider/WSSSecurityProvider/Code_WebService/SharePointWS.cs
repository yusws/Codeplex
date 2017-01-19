using System;
using System.Web.Services;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace MyLocalBroadband.WSSSecurityProvider.WebServices
{

    [WebService(Namespace = "http://www.mylocalbroadband.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SharePointService : System.Web.Services.WebService
    {
        #region Constructors
        public SharePointService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        #endregion

        #region Web Methods - Site Creation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webAppURL"></param>
        /// <param name="strURL"></param>
        /// <param name="strTitle"></param>
        /// <param name="strDesc"></param>
        /// <param name="nLCID"></param>
        /// <param name="strTemplate"></param>
        /// <param name="strOwnerLogin1"></param>
        /// <param name="strOwnerName1"></param>
        /// <param name="strOwnerEmail1"></param>
        /// <param name="strOwnerLogin2"></param>
        /// <param name="strOwnerName2"></param>
        /// <param name="strOwnerEmail2"></param>
        /// <param name="hhMode"></param>
        [WebMethod]
        public void hstCreateSite(string webAppURL, string strURL, string strTitle, string strDesc, uint nLCID, string strTemplate,
                                    string strOwnerLogin1, string strOwnerName1, string strOwnerEmail1,
                                    string strOwnerLogin2, string strOwnerName2, string strOwnerEmail2,
                                    bool hhMode)
        {
            System.Uri srvrUri = new System.Uri(webAppURL);
            SPWebApplication webApp = SPWebApplication.Lookup(srvrUri);

            SPSiteCollection siteColl = webApp.Sites;
            SPSite oSite = null;
            try
            {
                // If the secondary owner information is not provided, make sure it's null
                if (String.IsNullOrEmpty(strOwnerLogin2)) strOwnerLogin2 = null;
                if (String.IsNullOrEmpty(strOwnerName2)) strOwnerName2 = null;
                if (String.IsNullOrEmpty(strOwnerEmail2)) strOwnerEmail2 = null;
                if (String.IsNullOrEmpty(strDesc)) strDesc = null;
                if (String.IsNullOrEmpty(strTitle)) strTitle = null;
                if (String.IsNullOrEmpty(strTemplate)) strTemplate = null;

                // add site to the site collection
                oSite = siteColl.Add(strURL, strTitle, strDesc, nLCID, strTemplate,
                            strOwnerLogin1, strOwnerName1, strOwnerEmail1,
                            strOwnerLogin2, strOwnerName2, strOwnerEmail2, true);

            }
            finally
            {
                // dispose object
                if (oSite != null)
                    oSite.Dispose();
            }
        }
        #endregion

        #region Unused Methods
        //These methods were in the original Hosting starter kit
        //but are unused in my version.  They may be useful in the future
        //but I didn't want to put them in and worry about them not working.


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="webAppURL"></param>
        ///// <param name="strSiteURL"></param>
        ///// <param name="bDeleteUsers"></param>
        //[WebMethod]
        //public void hstDeleteSite(string webAppURL, string strSiteURL, bool bDeleteUsers)
        //{
        //    System.Uri srvrUri = new System.Uri(webAppURL);
        //    SPWebApplication webApp = SPWebApplication.Lookup(srvrUri);

        //    try
        //    {
        //        SPSiteCollection siteColl = webApp.Sites;
        //        string[] siteURLS = siteColl.Names;
        //        // iterate through the site collection to make sure we're deleting a site that exists.
        //        foreach (string url in siteURLS)
        //        {
        //            if (url.ToLower() == strSiteURL.ToLower())
        //            {
        //                // delete site from the site collection
        //                siteColl.Delete(url, bDeleteUsers);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="siteName"></param>
        ///// <param name="ContentDBSvr"></param>
        ///// <param name="ContentDBName"></param>
        ///// <param name="ContentDBLogin"></param>
        ///// <param name="ContentDBPwd"></param>
        ///// <param name="AppPoolID"></param>
        ///// <param name="AppPoolUserName"></param>
        ///// <param name="AppPoolPwd"></param>
        ///// <param name="strSvrComment"></param>
        ///// <param name="Port"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public string hstCreateWebApp(string siteName, string ContentDBSvr, string ContentDBName,
        //                            string ContentDBLogin, string ContentDBPwd,
        //                            string AppPoolID, string AppPoolUserName, string AppPoolPwd,
        //                            string strSvrComment, int Port)
        //{

        //    try
        //    {
        //        SPFarm farm = SPFarm.Local;
        //        // specify Web Application properties
        //        SPWebApplicationBuilder webAppBldr = new SPWebApplicationBuilder(farm);
        //        webAppBldr.ServerComment = strSvrComment;
        //        webAppBldr.Port = Port;
        //        webAppBldr.DefaultZoneUri = new Uri(siteName);
        //        webAppBldr.UseNTLMExclusively = true;

        //        // Specify AppPool to use
        //        webAppBldr.ApplicationPoolId = AppPoolID;
        //        webAppBldr.ApplicationPoolUsername = AppPoolUserName;
        //        System.Security.SecureString secString = new System.Security.SecureString();
        //        foreach (char c in AppPoolPwd.ToCharArray())
        //        {
        //            secString.AppendChar(c);
        //        }
        //        webAppBldr.ApplicationPoolPassword = secString;

        //        // specify Database name
        //        if (ContentDBName.Length > 0)
        //        {
        //            webAppBldr.CreateNewDatabase = true;
        //            webAppBldr.DatabaseServer = ContentDBSvr;
        //            webAppBldr.DatabaseName = ContentDBName;
        //            // todo: determine which identity can be used 
        //            //webAppBldr.DatabasePassword = ContentDBPwd;
        //            //webAppBldr.DatabaseUsername = ContentDBLogin;
        //        }

        //        // elevate permissions to allow user to create a Web App.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            // create WSS Web App
        //            SPWebApplication webApp = webAppBldr.Create();

        //            // create IIS site
        //            webApp.Provision();
        //        });
        //        return "WebApp is created and provisioned";

        //    }
        //    catch (Exception ex)
        //    {
        //        return (ex.Message + " -- " + ex.StackTrace);
        //    }
        //}

        ///// <summary>
        ///// sets a property in the site propertybag 
        ///// </summary>
        ///// <param name="strSiteURL">The URL of the site</param>
        ///// <param name="strPropertyName">property name</param>
        ///// <param name="strPropertyValue">property value</param>
        //[WebMethod]
        //public void hstSetSiteProperty(string strSiteURL, string strPropertyName, string strPropertyValue)
        //{


        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oWeb = oSite.OpenWeb();

        //    try
        //    {
        //        //// elevate permissions to allow user to create a new site property.
        //        //oWeb = oSite.OpenWeb();
        //        oWeb.AllowUnsafeUpdates = true;
        //        // add new property name and value
        //        oWeb.Properties.Add(strPropertyName, strPropertyValue);
        //        oWeb.Properties.Update();
        //        oWeb.AllowUnsafeUpdates = false;

        //        //});
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //        // dispose objects
        //        if (oWeb != null)
        //            oWeb.Dispose();

        //        if (oSite != null)
        //            oSite.Dispose();
        //    }
        //}
        ///// <summary>
        ///// gets the value of the property specified
        ///// </summary>
        ///// <param name="strSiteURL">The URL of the site</param>
        ///// <param name="strPropertyName">Property name</param>
        ///// <returns></returns>
        //[WebMethod]
        //public string hstGetSiteProperty(string strSiteURL, string strPropertyName)
        //{
        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oWeb = oSite.OpenWeb();
        //    // check for property existence
        //    foreach (System.Collections.DictionaryEntry props in oWeb.Properties)
        //    {
        //        // if property exist, get the value
        //        if (props.Key.ToString() == strPropertyName.ToLower())
        //        {
        //            string strValue = props.Value.ToString();
        //            return (strValue);
        //        }
        //    }
        //    // dispose objects
        //    if (oWeb != null)
        //        oWeb.Dispose();

        //    if (oSite != null)
        //        oSite.Dispose();
        //    throw new Exception("Site Property: " + strPropertyName + " does not exists");

        //}
        ///// <summary>
        ///// adds a user to the site, with reader permissions
        ///// </summary>
        ///// <param name="strSiteURL">The URL of the site</param>
        ///// <param name="strUserLogin">user login</param>
        ///// <param name="strUserName">user name</param>
        ///// <param name="strUserEmail">user email address</param>
        //[WebMethod]
        //public void hstAddUsertoSite(string strSiteURL, string strUserLogin, string strUserName, string strUserEmail)
        //{
        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oWeb = oSite.OpenWeb();

        //    // get site's user collection
        //    SPUserCollection userColl = oWeb.AllUsers;

        //    // if user exists, throw exception 
        //    foreach (SPUser user in userColl)
        //    {
        //        if (user.LoginName.ToLower() == strUserLogin.ToLower())
        //        {
        //            throw new Exception("User " + strUserLogin + " already exists");
        //        }
        //    }
        //    try
        //    {
        //        // elevate permissions to allow new user to be added to the site.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            oWeb.AllUsers.Add(strUserLogin, strUserEmail, strUserName, null);
        //        });
        //        // get the user object to assign a role
        //        SPUser spUser = oWeb.AllUsers[strUserLogin];

        //        // get the reader role
        //        SPRoleDefinitionCollection roleDefsColl = oWeb.RoleDefinitions;
        //        SPRoleDefinition readRoleDef = null;
        //        foreach (SPRoleDefinition roleDef in roleDefsColl)
        //        {
        //            if (roleDef.Type == SPRoleType.Reader)
        //            {
        //                readRoleDef = roleDef;
        //                break;
        //            }
        //        }

        //        // create a new role assignment for the user
        //        SPRoleAssignment ra = new SPRoleAssignment(spUser);

        //        // elevate permissions to allow new user to be added to the role.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            // add the user to the role
        //            ra.RoleDefinitionBindings.Add(readRoleDef);

        //            // add the role assignment to the site
        //            oWeb.RoleAssignments.Add(ra);

        //            // apply the update.
        //            oWeb.Update();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //dispose objects
        //        if (oWeb != null)
        //            oWeb.Dispose();

        //        if (oSite != null)
        //            oSite.Dispose();
        //    }
        //}
        ///// <summary>
        ///// sets the people picker property for the web app 
        ///// </summary>
        ///// <param name="webAppUrl">web app url</param>
        //[WebMethod]
        //public void hstSetPeoplePickerProperty(string webAppUrl)
        //{
        //    try
        //    {
        //        // elevate permissions to allow setting of property.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            System.Uri srvrUri = new System.Uri(webAppUrl);
        //            SPWebApplication webApp = SPWebApplication.Lookup(srvrUri);

        //            // if property is false, set it to true
        //            if (!webApp.PeoplePickerSettings.OnlySearchWithinSiteCollection)
        //            {
        //                webApp.PeoplePickerSettings.OnlySearchWithinSiteCollection = true;
        //                webApp.Update();
        //            }
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        ///// <summary>
        ///// Get all current sub sites templates for the site
        ///// </summary>
        ///// <param name="strSiteURL">url of the sitecollection i.e. "http://www.sitea.com"</param>
        ///// <returns></returns>
        //[WebMethod]
        //public string hstGetSubSiteTemplates(string strSiteURL)
        //{
        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oWeb = oSite.OpenWeb();
        //    SPWebCollection allSites = oSite.AllWebs;

        //    string siteTemplates = string.Empty;

        //    foreach (SPWeb subSite in allSites)
        //    {
        //        if (siteTemplates != string.Empty) siteTemplates += ",";
        //        string templateid = subSite.WebTemplate + "#" + subSite.Configuration.ToString();

        //        // if web template is STS, get the TemplateID from the propertybag value.
        //        // we had to do this because sites from .stp files do not store the templateID
        //        string propertyVal = string.Empty;
        //        if (subSite.WebTemplate == "STS")
        //        {
        //            string strProperty = "STP_ID";

        //            foreach (System.Collections.DictionaryEntry props in subSite.Properties)
        //            {
        //                // if property exist, get the value
        //                if (props.Key.ToString() == strProperty.ToLower())
        //                {
        //                    templateid = props.Value.ToString();
        //                    break;
        //                }
        //            }
        //        }

        //        string url = subSite.Navigation.Web.Url;
        //        siteTemplates += templateid + "?" + url;
        //    }
        //    return (siteTemplates);

        //}
        ///// <summary>
        ///// Method to create a Sub site for a site collection
        ///// </summary>
        ///// <param name="strSiteURL">url of the sitecollection i.e. "http://www.sitea.com"</param>
        ///// <param name="subsitePath">the path to the subsite i.e. inventory</param>
        ///// <param name="strTitle">sub site title</param>
        ///// <param name="strDesc">sub site description</param>
        ///// <param name="strTemplate">a valid templateID</param>
        ///// <param name="nLCID">the LCID for the language i.e. 1033 for english</param>
        //[WebMethod]
        //public void hstCreateSubSite(string strSiteURL, string subSitePath, string strTitle,
        //                            string strDesc, string strTemplate, uint nLCID)
        //{

        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oSubSiteWeb = oSite.OpenWeb();

        //    SPWeb oWeb = null;

        //    if (String.IsNullOrEmpty(strDesc)) strDesc = null;
        //    if (String.IsNullOrEmpty(strTitle)) strTitle = null;

        //    try
        //    {
        //        // elevate permissions to allow user to create a new site.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            // the subsite will inherit permissions and will not convert the site if it exists
        //            oWeb = oSubSiteWeb.Webs.Add(subSitePath, strTitle, strDesc, nLCID, strTemplate, false, false);

        //            SPNavigationNodeCollection nodes = oSubSiteWeb.Navigation.TopNavigationBar;
        //            SPNavigationNode navNode = new SPNavigationNode(strTitle, subSitePath);
        //            nodes.AddAsLast(navNode);

        //            oWeb.Navigation.UseShared = true;

        //            // create entry in property bag to store template and url in the subsite.
        //            oWeb.AllowUnsafeUpdates = true;
        //            // add the Templateid to the property bag. This needs to be done becuase
        //            // sites that are created from site templates (.stp) do not retain the templateid.
        //            oWeb.Properties.Add("STP_ID", strTemplate);
        //            oWeb.Properties.Update();
        //            oWeb.AllowUnsafeUpdates = false;

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //dispose objects
        //        if (oWeb != null)
        //            oWeb.Dispose();

        //        if (oSite != null)
        //            oSite.Dispose();
        //    }
        //}
        ///// <summary>
        ///// Method to delete a site collection's sub site.
        ///// </summary>
        ///// <param name="strSiteURL">url of the sitecollection i.e. "http://www.sitea.com"</param>
        ///// <param name="subSitePath">the path to the subsite i.e. inventory</param>
        //[WebMethod]
        //public void hstDeleteSubSite(string strSiteURL, string subSitePath)
        //{

        //    SPSite oSite = new SPSite(strSiteURL);
        //    SPWeb oSubSiteWeb = oSite.OpenWeb();

        //    try
        //    {
        //        // elevate permissions to allow user to create a new site.
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            // the subsite will inherit permissions and will not convert the site if it exists
        //            oSubSiteWeb.Webs.Delete(subSitePath);

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //dispose objects
        //        if (oSite != null)
        //            oSite.Dispose();
        //    }
        //}
        #endregion
    } 
}
