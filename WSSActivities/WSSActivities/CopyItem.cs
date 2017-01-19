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
	public partial class CopyItem: SequenceActivity
	{
		public CopyItem()
		{
			InitializeComponent();
        }
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(CopyItem));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(CopyItem));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(CopyItem));
        public static readonly DependencyProperty ListIDProperty = DependencyProperty.Register("ListID", typeof(string), typeof(CopyItem));
        public static readonly DependencyProperty ListItemIDProperty = DependencyProperty.Register("ListItemID", typeof(int), typeof(CopyItem));
        public static readonly DependencyProperty DestinationListUrlProperty = DependencyProperty.Register("DestinationListUrl", typeof(string), typeof(CopyItem));
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(int), typeof(CopyItem));
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
        public string ListID
        {
            get { return (string)GetValue(ListIDProperty); }
            set { SetValue(ListIDProperty, value); }
        }
        public int ListItemID
        {
            get { return (int)GetValue(ListItemIDProperty); }
            set { SetValue(ListItemIDProperty, value); }
        }
        public string DestinationListUrl
        {
            get { return (string)GetValue(DestinationListUrlProperty); }
            set { SetValue(DestinationListUrlProperty, value); }
        }
        public int Result
        {
            get { return (int)GetValue(ResultProperty); }
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
                using (SPSite sourceSite = new SPSite(__Context.Web.Site.ID, __Context.Site.UserToken))
                {
                    using (SPWeb sourceWeb = sourceSite.OpenWeb(this.__Context.Web.ID))
                    {
                        //replace any workflow variables
                        string destinationUrlProcessed = Helper.ProcessStringField(DestinationListUrl, executionContext.Activity, __Context);

                        using (SPSite destSite = new SPSite(destinationUrlProcessed, __Context.Site.UserToken))
                        {
                            using (SPWeb destWeb = destSite.OpenWeb())
                            {
                                SPList destinationList = null;

                                //each list, even a non document library list has at least a root folder.
                                SPFolder destFolder = destWeb.GetFolder(destinationUrlProcessed);

                                if (!destFolder.Exists)
                                    throw new ApplicationException(string.Format("List at {0} does not exist!", DestinationListUrl));

                                destinationList = destWeb.Lists[destFolder.ParentListId];

                                SPList sourceList = sourceWeb.Lists[new Guid(ListID)];
                                SPListItem sourceItem = sourceList.Items.GetItemById(ListItemID);
                                ItemCopier.ListItemCopyOptions options = new ItemCopier.ListItemCopyOptions();

                                options.IncludeAttachments = true;
                                options.OperationType = ItemCopier.OperationType.Copy;
                                options.Overwrite = true;
                                options.DestinationFolder = destFolder;

                                using (ItemCopier myCopier = new ItemCopier(sourceItem, destinationList, options))
                                {
                                    Result = myCopier.Copy();

                                    string message = "Item Copied from List: " + ListID + "; item: " + ListItemID + "; to url:" + DestinationListUrl + "; new id: " + Result;
                                    WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                                }
                            }
                        }
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
