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
	public partial class PublishItem: SequenceActivity
    {
		public PublishItem()
		{
			InitializeComponent();
		}
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(PublishItem));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(PublishItem));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(PublishItem));
        public static readonly DependencyProperty ListIDProperty = DependencyProperty.Register("ListID", typeof(string), typeof(PublishItem));
        public static readonly DependencyProperty ListItemIDProperty = DependencyProperty.Register("ListItemID", typeof(int), typeof(PublishItem));
        public static readonly DependencyProperty DestinationListUrlProperty = DependencyProperty.Register("DestinationListUrl", typeof(string), typeof(PublishItem));
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
                        SPList sourceList = sourceWeb.Lists[new Guid(ListID)];

                        //replace any workflow variables
                        string destinationUrls = Helper.ProcessStringField(DestinationListUrl, executionContext.Activity, __Context);

                        ColumnHelper.EnsurePublishColumns(sourceList, destinationUrls);

                        SPListItem sourceItem = sourceList.Items.GetItemById(ListItemID);
                        StringBuilder resultLinks = new StringBuilder();
                        string[] destinationListUrls = destinationUrls.Trim().Trim(';').Split(';');
                        foreach (string destinationUrl in destinationListUrls)
                        {
                            SPFolder destFolder = ListHelper.GetSPFolderFromURL(destinationUrl);

                            if (!destFolder.Exists)
                                throw new ApplicationException(string.Format("List at {0} does not exist!", destinationUrl));

                            SPList destinationList = ListHelper.GetSPListFromURL(destinationUrl, __Context.Site.UserToken);

                            ItemCopier.ListItemCopyOptions options = new ItemCopier.ListItemCopyOptions();
                            options.IncludeAttachments = true;
                            options.OperationType = ItemCopier.OperationType.Copy;
                            options.Overwrite = true;
                            options.DestinationFolder = destFolder;
                            options.LinkToOriginal = true;

                            using (ItemCopier myCopier = new ItemCopier(sourceItem, destinationList, options))
                            {
                                int itemID = GetExistingID(destinationList, destinationUrl);
                                if (itemID > 0)
                                {
                                    myCopier.UpdateItem(itemID);
                                    string message = "Published item updated from List: " + ListID + "; item: " + ListItemID + "; to url:" + DestinationListUrl + "; new id: " + itemID;
                                    WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                                }
                                else
                                {
                                    itemID = myCopier.Copy();
                                    using (SPWeb destWeb = destinationList.ParentWeb)
                                    {
                                        string newURL = destWeb.Url + "/" + destinationList.Forms[PAGETYPE.PAGE_DISPLAYFORM].Url + "?id=" + itemID + " ; ";
                                        resultLinks.Append(newURL.ToLower());

                                        string message = "New Item Published from List: " + ListID + "; item: " + ListItemID + "; to url:" + newURL;
                                        WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                                    }
                                }
                            }
                        }
                        string currentLinks = (string)sourceItem[ColumnHelper.PublishedToInternalColumnName];
                        sourceItem[ColumnHelper.PublishedToInternalColumnName] = resultLinks.ToString() + currentLinks;
                        sourceItem.SystemUpdate();
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

        #region Private Methods
        //private string RemoveBrokenLinks(string delimitedURLS)
        //{
        //    if(string.IsNullOrEmpty(delimitedURLS))
        //        return "";

        //    StringBuilder resultLinks = new StringBuilder();
        //    string[] urls = delimitedURLS.Trim().Trim(';').Split(';');
        //    foreach (string url in urls)
        //    {
        //        try
        //        {
        //            SPList list = ListHelper.GetSPListFromURL(url.Trim());
        //            if (GetExistingID(list, url) > 0)
        //            {
        //                resultLinks.Append(url + " ; ");
        //            }
        //        }
        //        catch { }
        //    }
        //    return resultLinks.ToString();
        //}
        private int GetExistingID(SPList list, string url)
        {
            try
            {
                string query = url.Substring(url.IndexOf('?'));
                NameValueCollection parameters = HttpUtility.ParseQueryString(query);
                int id = Convert.ToInt32(parameters["id"]);
                return list.GetItemById(id).ID;
            }
            catch{}

            return -1;
        }
        #endregion
    }
}
