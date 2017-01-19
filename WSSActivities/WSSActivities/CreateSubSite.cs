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
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace MyLocalBroadband.Activities.WSS
{
	public partial class CreateSubSite: Activity
	{
		public CreateSubSite()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(CreateSubSite));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(CreateSubSite));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(CreateSubSite));
        public static readonly DependencyProperty SiteURLProperty = DependencyProperty.Register("SiteURL", typeof(string), typeof(CreateSubSite));
        public static readonly DependencyProperty SiteTitleProperty = DependencyProperty.Register("SiteTitle", typeof(string), typeof(CreateSubSite));
        public static readonly DependencyProperty SiteDescriptionProperty = DependencyProperty.Register("SiteDescription", typeof(string), typeof(CreateSubSite));
        public static readonly DependencyProperty TemplateIDProperty = DependencyProperty.Register("TemplateID", typeof(string), typeof(CreateSubSite));
        public static readonly DependencyProperty LCIDProperty = DependencyProperty.Register("LCID", typeof(UInt32), typeof(CreateSubSite));
        public static readonly DependencyProperty UseUniquePermissionsProperty = DependencyProperty.Register("UseUniquePermissions", typeof(bool), typeof(CreateSubSite));
        public static readonly DependencyProperty ConvertIfExistsProperty = DependencyProperty.Register("ConvertIfExists", typeof(bool), typeof(CreateSubSite));
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(string), typeof(CreateSubSite));
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
        public bool UseUniquePermissions
        {
            get { return (bool)GetValue(UseUniquePermissionsProperty); }
            set { SetValue(UseUniquePermissionsProperty, value); }
        }
        public bool ConvertIfExists
        {
            get { return (bool)GetValue(ConvertIfExistsProperty); }
            set { SetValue(ConvertIfExistsProperty, value); }
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
               // SPSecurity.RunWithElevatedPrivileges(delegate()
               //{                
                   string parentSiteURL = GetParentSiteURL();
                   using (SPSite parentSite = new SPSite(parentSiteURL,__Context.Web.CurrentUser.UserToken))
                   {
                       using (SPWeb parentWeb = parentSite.OpenWeb())
                       {
                           SPUser user2 = parentWeb.CurrentUser;

                           string webRelativeURL = SiteURL.Substring(parentWeb.Url.Length + 1);
                           if (String.IsNullOrEmpty(SiteDescription))
                               SiteDescription = "";

                           SPWeb subsite = parentWeb.Webs.Add(webRelativeURL, SiteTitle, SiteDescription, LCID, TemplateID, UseUniquePermissions, ConvertIfExists);
                           subsite.Navigation.UseShared = true;
                           
                           Result = subsite.Url;

                           string message = "Provisioning " + SiteTitle + " at " + SiteURL;
                           WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete",UserID, message);
                       }
                   }
               //});
            }
            catch (Exception ex)
            {
                WorkflowHistoryLogger.LogError(executionContext,UserID, ex);
                return ActivityExecutionStatus.Faulting;
            }
            return ActivityExecutionStatus.Closed;
        }
        #endregion

        #region Private methods
        string GetParentSiteURL()
        {
            string[] urlParts = SiteURL.Split('/');
            return string.Join("/", urlParts, 0, urlParts.Length - 1);
        }
        #endregion
    }
}
