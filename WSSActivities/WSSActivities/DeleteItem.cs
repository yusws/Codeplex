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
using MyLocalBroadband.Activities.WSS.Utilities;

namespace MyLocalBroadband.Activities.WSS
{
	public partial class DeleteItem: SequenceActivity
	{
		public DeleteItem()
		{
			InitializeComponent();
        }
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(DeleteItem));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(DeleteItem));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(DeleteItem));
        public static readonly DependencyProperty ItemIDProperty = DependencyProperty.Register("ItemID", typeof(string), typeof(DeleteItem));
        public static readonly DependencyProperty ListURLProperty = DependencyProperty.Register("ListURL", typeof(string), typeof(DeleteItem));
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
        public int ItemID
        {
            get { return Convert.ToInt32((string)GetValue(ItemIDProperty)); }
            set { SetValue(ItemIDProperty, value.ToString()); }
        }
        public string ListURL
        {
            get { return (string)GetValue(ListURLProperty); }
            set { SetValue(ListURLProperty, value); }
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
                //replace any workflow variables
                string listURL = Helper.ProcessStringField(ListURL, executionContext.Activity, __Context);

                using (SPSite listSite = new SPSite(listURL, __Context.Site.UserToken))
                {
                    using (SPWeb listWeb = listSite.OpenWeb())
                    {
                        //each list, even a non document library list has at least a root folder.
                        SPFolder listFolder = listWeb.GetFolder(listURL);

                        if (!listFolder.Exists)
                            throw new ApplicationException(string.Format("List at {0} does not exist!", ListURL));

                        SPList list = listWeb.Lists[listFolder.ParentListId];

                        list.GetItemById(ItemID).Recycle();
                        
                        string message = "Item with ID: " + ItemID + "; deleted from: " + ListURL;
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
	}
}
