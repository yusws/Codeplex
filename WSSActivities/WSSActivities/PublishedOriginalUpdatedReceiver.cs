using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Security;
using System.Web.Security;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Web.Util;
using System.Web.Configuration;
using System.Configuration;
using MyLocalBroadband.Activities.WSS.Utilities;

namespace MyLocalBroadband.Activities.WSS
{
    class PublishedOriginalUpdatedReceiver : SPItemEventReceiver
    {
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            SPListItem PublishedOriginal = null;
            bool updatePublishedToField = false;
            try
            {
                PublishedOriginal = properties.ListItem;
                StringBuilder destinationURLs = new StringBuilder(ColumnHelper.GetFieldValue(PublishedOriginal, ColumnHelper.PublishedToInternalColumnName));

                string[] destinationUrlArray = destinationURLs.ToString().Split(';');
                foreach (string destinationDisplayUrl in destinationUrlArray)
                {
                    SPListItem destinationItem = ListHelper.GetSPItemFromURL(destinationDisplayUrl);
                    if (destinationItem != null)
                    {
                        if (ColumnHelper.ListContainsField(destinationItem.ParentList, ColumnHelper.PublishedFromInternalColumnName))
                        {
                            ItemCopier.ListItemCopyOptions options = new ItemCopier.ListItemCopyOptions();
                            options.IncludeAttachments = true;
                            options.OperationType = ItemCopier.OperationType.Copy;
                            options.Overwrite = true;
                            options.DestinationFolder = ListHelper.GetSPFolderFromURL(destinationDisplayUrl);
                            options.LinkToOriginal = true;

                            using (ItemCopier myCopier = new ItemCopier(PublishedOriginal, destinationItem.ParentList, options))
                            {
                                myCopier.UpdateItem(destinationItem.ID);
                            }
                        }
                        else //published from column has been deleted, treat this as an unlinked list and do not syndicate the update
                        {
                            string old = destinationURLs.ToString();
                            destinationURLs = destinationURLs.Replace(destinationDisplayUrl, "");
                            destinationURLs = new StringBuilder(destinationURLs.Replace("; ;", "; ").ToString().TrimStart(';').Trim());
                            updatePublishedToField = true;
                        }
                    }
                    if (updatePublishedToField)
                    {
                        ColumnHelper.SetFieldValue(PublishedOriginal, ColumnHelper.PublishedToInternalColumnName, destinationURLs.ToString());
                        PublishedOriginal.SystemUpdate();
                    }
                }
            }
            catch
            {
                //if exception was caused because the published to field has been removed from this list,
                //remove the event handler
                if (!ColumnHelper.ListContainsField(properties.ListItem.ParentList, ColumnHelper.PublishedToInternalColumnName))
                {
                    RemoveEventReceiver(properties.ListItem.ParentList);
                }
            }
            base.ItemUpdated(properties);
        }

        public static void RegisterEventReceiver(SPList list)
        {
            list.EventReceivers.Add(SPEventReceiverType.ItemUpdated, Assembly.GetExecutingAssembly().FullName, "MyLocalBroadband.Activities.WSS.PublishedOriginalUpdatedReceiver");
            list.Update();
        }
        public static void RemoveEventReceiver(SPList list)
        {
            SPEventReceiverDefinition receiverToDelete = null;
            foreach (SPEventReceiverDefinition receiver in list.EventReceivers)
            {
                if (receiver.Class == "MyLocalBroadband.Activities.WSS.PublishedOriginalUpdatedReceiver")
                {
                    receiverToDelete = receiver;
                }
            }
            if (receiverToDelete != null)
                receiverToDelete.Delete();
            list.Update();
        }
	}
}
