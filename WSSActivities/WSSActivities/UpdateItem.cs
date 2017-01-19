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
	public partial class UpdateItem: SequenceActivity
	{
		public UpdateItem()
		{
			InitializeComponent();
        }
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(UpdateItem));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(UpdateItem));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(UpdateItem));
        public static readonly DependencyProperty SourceListIDProperty = DependencyProperty.Register("SourceListID", typeof(string), typeof(UpdateItem));
        public static readonly DependencyProperty SourceListItemIDProperty = DependencyProperty.Register("SourceListItemID", typeof(int), typeof(UpdateItem));
        public static readonly DependencyProperty DestinationListURLProperty = DependencyProperty.Register("DestinationListURL", typeof(string), typeof(UpdateItem));
        public static readonly DependencyProperty DestinationItemIDProperty = DependencyProperty.Register("DestinationItemID", typeof(Double), typeof(UpdateItem));
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
        public string SourceListID
        {
            get { return (string)GetValue(SourceListIDProperty); }
            set { SetValue(SourceListIDProperty, value); }
        }
        public int SourceListItemID
        {
            get { return (int)GetValue(SourceListItemIDProperty); }
            set { SetValue(SourceListItemIDProperty, value); }
        }
        public string DestinationListURL
        {
            get { return (string)GetValue(DestinationListURLProperty); }
            set { SetValue(DestinationListURLProperty, value); }
        }
        public int DestinationItemID
        {
            get { return Convert.ToInt32((Double)GetValue(DestinationItemIDProperty)); }
            set { SetValue(DestinationItemIDProperty, value.ToString()); }
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
                        string destinationUrlProcessed = Helper.ProcessStringField(DestinationListURL, executionContext.Activity, __Context);

                        using (SPSite destSite = new SPSite(destinationUrlProcessed, __Context.Site.UserToken))
                        {
                            using (SPWeb destWeb = destSite.OpenWeb())
                            {
                                SPList destinationList = null;

                                //each list, even a non document library list has at least a root folder.
                                SPFolder destFolder = destWeb.GetFolder(destinationUrlProcessed);

                                if (!destFolder.Exists)
                                    throw new ApplicationException(string.Format("List at {0} does not exist!", DestinationListURL));

                                destinationList = destWeb.Lists[destFolder.ParentListId];

                                SPList sourceList = sourceWeb.Lists[new Guid(SourceListID)];

                                SPListItem sourceItem = sourceList.Items.GetItemById(SourceListItemID);

                                ItemCopier.ListItemCopyOptions options = new ItemCopier.ListItemCopyOptions();

                                options.IncludeAttachments = true;

                                options.OperationType = ItemCopier.OperationType.Copy;

                                options.Overwrite = true;

                                options.DestinationFolder = destFolder;

                                using (ItemCopier myCopier = new ItemCopier(sourceItem, destinationList, options))
                                {
                                    myCopier.UpdateItem(DestinationItemID);

                                    string message = "Item Updated item at" + DestinationListURL + "; ID: " + DestinationItemID + "; with data from list:" + SourceListID + "; item:" + SourceListItemID;
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
