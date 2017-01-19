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
	public partial class SetSiteTitle: SequenceActivity
	{
		public SetSiteTitle()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetSiteTitle));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetSiteTitle));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetSiteTitle));
        public static readonly DependencyProperty SiteURLProperty = DependencyProperty.Register("SiteURL", typeof(string), typeof(SetSiteTitle));
        public static readonly DependencyProperty NewTitleProperty = DependencyProperty.Register("NewTitle", typeof(string), typeof(SetSiteTitle));
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
        public string NewTitle
        {
            get { return (string)GetValue(NewTitleProperty); }
            set { SetValue(NewTitleProperty, value); }
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
                using (SPSite site = new SPSite(SiteURL, __Context.Web.CurrentUser.UserToken))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (string.IsNullOrEmpty(NewTitle))
                        {
                            NewTitle = "";
                        }

                        string oldTitle = web.Title;
                        web.Title = NewTitle;
                        web.Update();

                        string message = "Site: " + SiteURL + "; renamed from " + oldTitle + " to " + NewTitle;
                        WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                    }
                }
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
        #endregion
	}
}
