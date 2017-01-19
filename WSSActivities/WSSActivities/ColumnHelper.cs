using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace MyLocalBroadband.Activities.WSS.Utilities
{
	class ColumnHelper
    {
        #region Public Constants
        public const string PublishedToInternalColumnName = "MLBPublishedTo";
        public const string PublishedFromInternalColumnName = "MLBPublishedFrom";
        #endregion

        #region Public Methods
        public static void EnsurePublishColumns(string sourceListURL, string destinationListURLS)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPList sourceList = ListHelper.GetSPListFromURL(sourceListURL);
                EnsurePublishColumns(sourceList, destinationListURLS);
            });
        }
        public static void EnsurePublishColumns(SPList sourceList, string destinationListURLS)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                EnsurePublishedToColumn(sourceList);
                string[] destinationURLs = destinationListURLS.Trim().Trim(';').Split(';');
                SPList destinationList = null;
                foreach (string destinationURL in destinationURLs)
                {
                    destinationList = ListHelper.GetSPListFromURL(destinationURL);
                    EnsurePublishedFromColumn(destinationList);
                }
            });
        }
        public static SPField LookupField(SPList list, string fieldName)
        {
            foreach (SPField field in list.Fields)
            {
                if (field.StaticName == fieldName)
                    return field;
            }
            return null;
        }
        public static bool ListContainsField(SPList list, string fieldName)
        {
            SPField field = LookupField(list, fieldName);
            if (field == null)
                return false;
            return true;
        }
        public static string GetFieldValue(SPListItem item, string internalColumnName)
        {
            string displayName = LookupField(item.ParentList, internalColumnName).Title;
            object value = item[displayName];
            if(value == null)
                return "";
            return item[displayName].ToString();
        }
        public static void SetFieldValue(SPListItem item, string internalColumnName, string newValue)
        {
            string displayName = LookupField(item.ParentList, internalColumnName).Title;
            item[displayName] = newValue;
        }
        public static void EnsurePublishedToColumn(SPList list)
        {
            SPField field = LookupField(list, PublishedToInternalColumnName);

            if (field == null)
            {
                CreateLinkedColumn(list, PublishedToInternalColumnName, "Published To", "This item has been published to these locations.");
                PublishedOriginalDeletedReceiver.RegisterEventReceiver(list);
                PublishedOriginalUpdatedReceiver.RegisterEventReceiver(list);
            }
            else if (field.Type != SPFieldType.Note)
            {
                throw new DataMisalignedException("Could not ensure source column.  Column with name Published To or " + PublishedToInternalColumnName + " exists but is not a note type");
            } 
        }
        public static void EnsurePublishedFromColumn(SPList list)
        {
            SPField field = LookupField(list, PublishedFromInternalColumnName);

            if (field == null)
            {
                CreateLinkedColumn(list, PublishedFromInternalColumnName, "Published From", "This item is publised from this location.");
                PublishedCopyDeleteReceiver.RegisterEventReceiver(list);
            }
            else if (field.Type != SPFieldType.Note)
            {
                throw new DataMisalignedException("Could not ensure destination column.  Column with name Published From or " + PublishedFromInternalColumnName + " exists but is not a note type");
            } 
        }

        #endregion

        #region Private Methods
        private static void CreateLinkedColumn(SPList list, string columnName,string displayName,string description)
        {
            SPField field = list.Fields[list.Fields.Add(columnName,SPFieldType.Note,false)];
            field.Title = displayName;
            field.Description = description;
            field.ShowInEditForm = false;
            field.ShowInNewForm = false;
            field.Update();
        }
        #endregion
    }
}
