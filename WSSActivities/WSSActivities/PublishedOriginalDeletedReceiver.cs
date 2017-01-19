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
    class PublishedOriginalDeletedReceiver : SPItemEventReceiver
	{
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            SPListItem PublishedOriginal = null;
            try
            {
                PublishedOriginal = properties.ListItem;
                string destinationURLs = ColumnHelper.GetFieldValue(PublishedOriginal, ColumnHelper.PublishedToInternalColumnName);

                string[] destinationUrlArray = destinationURLs.Split(';');
                foreach (string destinationDisplayUrl in destinationUrlArray)
                {
                    SPListItem destinationItem = ListHelper.GetSPItemFromURL(destinationDisplayUrl);

                    if (destinationItem != null)
                    {
                        //if the destination list has been decoupled then do not delete the item there.
                        if (ColumnHelper.ListContainsField(destinationItem.ParentList, ColumnHelper.PublishedFromInternalColumnName))
                        {
                            destinationItem.Recycle();
                        }
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
            base.ItemDeleting(properties);
        }

        public static void RegisterEventReceiver(SPList list)
        {
            list.EventReceivers.Add(SPEventReceiverType.ItemDeleting, Assembly.GetExecutingAssembly().FullName, "MyLocalBroadband.Activities.WSS.PublishedOriginalDeletedReceiver");
            list.Update();
        }
        public static void RemoveEventReceiver(SPList list)
        {
            SPEventReceiverDefinition receiverToDelete = null;
            foreach (SPEventReceiverDefinition receiver in list.EventReceivers)
            {
                if (receiver.Class == "MyLocalBroadband.Activities.WSS.PublishedOriginalDeletedReceiver")
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
