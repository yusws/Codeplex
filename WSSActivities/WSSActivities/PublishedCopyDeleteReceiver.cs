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
    class PublishedCopyDeleteReceiver : SPItemEventReceiver
	{
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            SPListItem PublishedCopy = null;
            try
            {
                PublishedCopy = properties.ListItem;
                string sourceURL = ColumnHelper.GetFieldValue(PublishedCopy, ColumnHelper.PublishedFromInternalColumnName);

                SPListItem sourceItem = ListHelper.GetSPItemFromURL(sourceURL);
                if (sourceItem != null)
                {
                    StringBuilder sourceLinks = new StringBuilder(ColumnHelper.GetFieldValue(sourceItem, ColumnHelper.PublishedToInternalColumnName));
                    string LinkTextInSource = ListHelper.GetDisplayUrl(PublishedCopy) + " ;";
                    sourceLinks = sourceLinks.Replace(LinkTextInSource, "");

                    ColumnHelper.SetFieldValue(sourceItem, ColumnHelper.PublishedToInternalColumnName, sourceLinks.ToString());
                    sourceItem.SystemUpdate();
                }
            }
            catch
            {
                //if exception was caused because the published from field has been removed from this list,
                //remove the event handler
                if (!(ColumnHelper.ListContainsField(properties.ListItem.ParentList, ColumnHelper.PublishedFromInternalColumnName)))
                {
                    RemoveEventReceiver(properties.ListItem.ParentList);
                }
            }
            base.ItemDeleting(properties);
        }
        public static void RegisterEventReceiver(SPList list)
        {
            list.EventReceivers.Add(SPEventReceiverType.ItemDeleting, Assembly.GetExecutingAssembly().FullName, "MyLocalBroadband.Activities.WSS.PublishedCopyDeleteReceiver");
            list.Update();
        }
        public static void RemoveEventReceiver(SPList list)
        {
            SPEventReceiverDefinition receiverToDelete = null;
            foreach (SPEventReceiverDefinition receiver in list.EventReceivers)
            {
                if (receiver.Class == "MyLocalBroadband.Activities.WSS.PublishedCopyDeleteReceiver")
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
