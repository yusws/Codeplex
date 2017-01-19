using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using MyLocalBroadband.Activities.WSS.Utilities;
namespace MyLocalBroadband.Activities.WSS
{
	public partial class SetMasterPage: SequenceActivity
	{
		public SetMasterPage()
		{
			InitializeComponent();
        }
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetMasterPage));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetMasterPage));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetMasterPage));
        public static readonly DependencyProperty WebURLProperty = DependencyProperty.Register("WebURL", typeof(string), typeof(SetMasterPage));
        public static readonly DependencyProperty MasterUrlProperty = DependencyProperty.Register("MasterUrl", typeof(string), typeof(SetMasterPage));
        public static readonly DependencyProperty CustomMasterUrlProperty = DependencyProperty.Register("CustomMasterUrl", typeof(string), typeof(SetMasterPage));
        public static readonly DependencyProperty AlternateCssUrlProperty = DependencyProperty.Register("AlternateCssUrl", typeof(string), typeof(SetMasterPage));
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
        public string WebURL
        {
            get { return (string)GetValue(WebURLProperty); }
            set { SetValue(WebURLProperty, value); }
        }
        public string MasterUrl
        {
            get { return (string)GetValue(MasterUrlProperty); }
            set { SetValue(MasterUrlProperty, value); }
        }
        public string CustomMasterUrl
        {
            get { return (string)GetValue(CustomMasterUrlProperty); }
            set { SetValue(CustomMasterUrlProperty, value); }
        }
        public string AlternateCssUrl
        {
            get { return (string)GetValue(AlternateCssUrlProperty); }
            set { SetValue(AlternateCssUrlProperty, value); }
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
                using (SPSite targetSite = new SPSite(WebURL, __Context.Site.UserToken))
                {
                    using (SPWeb targetWeb = targetSite.OpenWeb())
                    {
                        targetWeb.MasterUrl = MasterUrl;
                        targetWeb.CustomMasterUrl = CustomMasterUrl;

                        if (string.IsNullOrEmpty(AlternateCssUrl))
                        {
                            AlternateCssUrl = "";
                        }
                        targetWeb.AlternateCssUrl = AlternateCssUrl;
                        targetWeb.Update();
                    }

                    string message = "Web Masterpage settings updated.  MasterURL:" + MasterUrl + " CustomMasterURL:" + CustomMasterUrl + " AlternateCSSUrl:" + AlternateCssUrl;
                    WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
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

        #region Private Method
        #endregion
	}
}
