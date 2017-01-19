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
	public partial class SetSiteProperty: SequenceActivity
	{
		public SetSiteProperty()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetSiteProperty));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetSiteProperty));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetSiteProperty));
        public static readonly DependencyProperty SiteURLProperty = DependencyProperty.Register("SiteURL", typeof(string), typeof(SetSiteProperty));
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(SetSiteProperty));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(SetSiteProperty));
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
        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
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
                        string message;
                        if(web.Properties.ContainsKey(Key))
                        {
                            web.Properties[Key] = Value;
                            message = "Site, " + SiteURL + ", property " + Key + " updated with value " + Value;
                        }
                        else
                        {
                            web.Properties.Add(Key,Value);
                            message = "Site, " + SiteURL + ", property " + Key + " added new with value " + Value;
                        }
                        web.Properties.Update();
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
