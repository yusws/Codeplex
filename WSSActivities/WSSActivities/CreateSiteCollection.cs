using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace MyLocalBroadband.Activities.WSS
{
	public partial class CreateSiteCollection: SequenceActivity
	{
		public CreateSiteCollection()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(CreateSiteCollection));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(CreateSiteCollection));
        public static readonly DependencyProperty WebAppURLProperty = DependencyProperty.Register("WebAppURL", typeof(string), typeof(CreateSiteCollection));        
        public static readonly DependencyProperty SiteURLProperty = DependencyProperty.Register("SiteURL", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty SiteTitleProperty = DependencyProperty.Register("SiteTitle", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty SiteDescriptionProperty = DependencyProperty.Register("SiteDescription", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty TemplateIDProperty = DependencyProperty.Register("TemplateID", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty LCIDProperty = DependencyProperty.Register("LCID", typeof(UInt32), typeof(CreateSiteCollection));
        public static readonly DependencyProperty PrimaryOwnerLoginProperty = DependencyProperty.Register("PrimaryOwnerLogin", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty PrimaryOwnerDisplayNameProperty = DependencyProperty.Register("PrimaryOwnerDisplayName", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty PrimaryOwnerEmailProperty = DependencyProperty.Register("PrimaryOwnerEmail", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty SecondaryOwnerLoginProperty = DependencyProperty.Register("SecondaryOwnerLogin", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty SecondaryOwnerDisplayNameProperty = DependencyProperty.Register("SecondaryOwnerDisplayName", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty SecondaryOwnerEmailProperty = DependencyProperty.Register("SecondaryOwnerEmail", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty IsolateInNewContentDBProperty = DependencyProperty.Register("IsolateInNewContentDB", typeof(bool), typeof(CreateSiteCollection));
        public static readonly DependencyProperty DBServerProperty = DependencyProperty.Register("DBServer", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty NewContentDBNameProperty = DependencyProperty.Register("NewContentDBName", typeof(string), typeof(CreateSiteCollection));
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(string), typeof(CreateSiteCollection));
        #endregion

        #region Properties
        public WorkflowContext __Context
        {
            get { return (WorkflowContext)GetValue(__ContextProperty); }
            set { SetValue(__ContextProperty, value); }
        }
        public int __ListItem
        {
            get { return (int)GetValue(__ListItemProperty); }
            set { SetValue(__ListItemProperty, value); }
        }
        public string __ListId
        {
            get { return (string)GetValue(__ListIdProperty); }
            set { SetValue(__ListIdProperty, value); }
        }
        public string WebAppURL
        {
            get { return (string)GetValue(WebAppURLProperty); }
            set { SetValue(WebAppURLProperty, value); }
        }
        public string SiteURL
        {
            get { return (string)GetValue(SiteURLProperty); }
            set { SetValue(SiteURLProperty, value); }
        }
        public string SiteTitle
        {
            get { return (string)GetValue(SiteTitleProperty); }
            set { SetValue(SiteTitleProperty, value); }
        }
        public string SiteDescription
        {
            get { return (string)GetValue(SiteDescriptionProperty); }
            set { SetValue(SiteDescriptionProperty, value); }
        }
        public string TemplateID
        {
            get { return (string)GetValue(TemplateIDProperty); }
            set { SetValue(TemplateIDProperty, value); }
        }

        public UInt32 LCID
        {
            get { return (UInt32)GetValue(LCIDProperty); }
            set { SetValue(LCIDProperty, value); }
        }
        public string PrimaryOwnerLogin
        {
            get { return (string)GetValue(PrimaryOwnerLoginProperty); }
            set { SetValue(PrimaryOwnerLoginProperty, value); }
        }
        public string PrimaryOwnerDisplayName
        {
            get { return (string)GetValue(PrimaryOwnerDisplayNameProperty); }
            set { SetValue(PrimaryOwnerDisplayNameProperty, value); }
        }
        public string PrimaryOwnerEmail
        {
            get { return (string)GetValue(PrimaryOwnerEmailProperty); }
            set { SetValue(PrimaryOwnerEmailProperty, value); }
        }
        public string SecondaryOwnerLogin
        {
            get { return (string)GetValue(SecondaryOwnerLoginProperty); }
            set { SetValue(SecondaryOwnerLoginProperty, value); }
        }
        public string SecondaryOwnerDisplayName
        {
            get { return (string)GetValue(SecondaryOwnerDisplayNameProperty); }
            set { SetValue(SecondaryOwnerDisplayNameProperty, value); }
        }
        public string SecondaryOwnerEmail
        {
            get { return (string)GetValue(SecondaryOwnerEmailProperty); }
            set { SetValue(SecondaryOwnerEmailProperty, value); }
        }
        public bool IsolateInNewContentDB
        {
            get { return (bool)GetValue(IsolateInNewContentDBProperty); }
            set { SetValue(IsolateInNewContentDBProperty, value); }
        }
        public string DBServer
        {
            get { return (string)GetValue(DBServerProperty); }
            set { SetValue(DBServerProperty, value); }
        }
        public string NewContentDBName
        {
            get { return (string)GetValue(NewContentDBNameProperty); }
            set { SetValue(NewContentDBNameProperty, value); }
        }
        public string Result
        {
            get { return (string)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }
        private int UserID
        {
            get { return __Context.Web.CurrentUser.ID; }
        }
        #endregion

        #region Execute Method
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                string secondaryLogin = GetEmptyAsNull(SecondaryOwnerLogin);
                string secondaryDisplayName = GetEmptyAsNull(SecondaryOwnerDisplayName);
                string secondaryEmail = GetEmptyAsNull(SecondaryOwnerEmail);
                string templateID = GetEmptyAsNull(TemplateID);
                string description = GetNullAsEmpty(SiteDescription);
                string title = GetNullAsEmpty(SiteTitle);


                SPSiteCollection sites = GetSitesCollection();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (IsolateInNewContentDB)
                    {
                        using (SPSite site = sites.Add(SiteURL, title, description, LCID, templateID, PrimaryOwnerLogin, PrimaryOwnerDisplayName, PrimaryOwnerEmail, secondaryLogin, secondaryDisplayName, secondaryEmail,DBServer,NewContentDBName,null,null))
                        {
                            CapContentDB(site);
                            Result = site.Url;
                        }
                    }
                    else
                    {
                        using (SPSite site = sites.Add(SiteURL, title, description, LCID, templateID, PrimaryOwnerLogin, PrimaryOwnerDisplayName, PrimaryOwnerEmail, secondaryLogin, secondaryDisplayName, secondaryEmail))
                        {
                            Result = site.Url;
                        }
                    }

                    string message = "Provisioning Site Collection " + SiteTitle + " at " + SiteURL;
                    WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                    
                });
            }
            catch (Exception ex)
            {
                WorkflowHistoryLogger.LogError(executionContext, UserID, ex);
                return ActivityExecutionStatus.Faulting;
            }
            return ActivityExecutionStatus.Closed;
        }
        #endregion

        #region Private methods
        private SPSiteCollection GetSitesCollection()
        {
            SPWebApplication webApp = null;
            SPSiteCollection sites = null;

            if (string.IsNullOrEmpty(WebAppURL))
            {
                using (SPSite currentSite = new SPSite(__Context.Web.Site.ID, __Context.Web.CurrentUser.UserToken))
                {
                    webApp = currentSite.WebApplication;
                }
            }
            else
            {
                System.Uri srvrUri = new System.Uri(WebAppURL);
                webApp = SPWebApplication.Lookup(srvrUri);
            }
            sites = webApp.Sites;
            return sites;
        }
        private void CapContentDB(SPSite site)
        {
            SPContentDatabaseCollection contentDBs = site.WebApplication.ContentDatabases;
            foreach (SPContentDatabase db in contentDBs)
            {
                if (db.Name == NewContentDBName)
                {
                    db.WarningSiteCount = 0;
                    db.MaximumSiteCount = 1;
                    db.Update();
                }
            }
        }
        private string GetNullAsEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            return s;
        }
        private string GetEmptyAsNull(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            return s;
        }
        #endregion
	}
}
