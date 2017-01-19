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
    /// <summary>
    /// Handles copying of one list item from one list to another, even if lists are located on different sites. 
    /// </summary>
    public class ItemCopier : IDisposable
    {
        public enum OperationType
        {
            /// <summary>
            /// in Document Library - copy the file to destination document library
            /// in List - copies the item to destination list
            /// </summary>
            Copy = 1,
            /// <summary>
            /// in Document Library - move the file to destination document library
            /// in List - copy the item to destination list, delete the source item
            /// </summary
            Move,
        }

        public struct ListItemCopyOptions
        {
            /// <summary>
            /// move or copy file or list item
            /// </summary>
            public OperationType OperationType;
            /// <summary>
            /// does not apply to document libraries
            /// </summary>
            public bool IncludeAttachments;
            /// <summary>
            ///overwrite when copying or moving a file. Does not apply to list items.
            /// </summary>
            public bool Overwrite;
            /// <summary>
            /// copy or move a file to the specific subfolder. Does not apply to list items.
            /// </summary>
            public SPFolder DestinationFolder;

            public bool LinkToOriginal;

        }

        protected bool _finished = false;

        protected ListItemCopyOptions _options = default(ListItemCopyOptions);

        private SPListItem _sourceItem;

        private SPList _destList;

        protected SPList SourceList
        {
            get { return _sourceItem.ParentList; }
        }

        protected SPList DestinationList
        {
            get { return _destList; }
        }

        protected SPListItem SourceItem
        {
            get { return _sourceItem; }
        }



        public ItemCopier(SPListItem sourceItem, SPList destination, ListItemCopyOptions options)
        {
            // validate
            if (sourceItem == null || destination == null || sourceItem.ParentList == null)
                throw new ApplicationException("Source and Destination items and lists cannot be null!");

            if (sourceItem.ParentList.BaseType != destination.BaseType)
                throw new ApplicationException("Cannot copy items between different list types!");

            _options = options;
            _sourceItem = sourceItem;
            _destList = destination;


        }

        public virtual int Copy()
        {
            int result = 0;

            if (_finished)
                throw new InvalidOperationException("Copy or Update() was already called on this object. Only one call to Copy or Update() is allowed per object instance!");
            if (_options.LinkToOriginal)
            {
                ColumnHelper.EnsurePublishedToColumn(SourceList);
                ColumnHelper.EnsurePublishedFromColumn(DestinationList);
            }

            if (DestinationList.BaseType != SPBaseType.DocumentLibrary)
                result = CopyListItem();
            else
                result = CopyFile();

            _finished = true;

            return result;
        }

        public virtual void UpdateItem(int destinationItemID)
        {
            if (_finished)
                throw new InvalidOperationException("Copy or Update() was already called on this object. Only one call to Copy or Update() is allowed per object instance!");
            
            if (_options.LinkToOriginal)
            {
                ColumnHelper.EnsurePublishedToColumn(SourceList);
                ColumnHelper.EnsurePublishedFromColumn(DestinationList);
            }
            if (DestinationList.BaseType != SPBaseType.DocumentLibrary)
                UpdateListItem(destinationItemID);
            else
                CopyFile();

            _finished = true;
        }

        /// <summary>
        /// handles copying of files from one document library to another
        /// </summary>
        protected virtual int CopyFile()
        {
            SPFolder destFolder = null;

            if (_options.DestinationFolder != null && _options.DestinationFolder.Exists)
                destFolder = _options.DestinationFolder;
            else
                destFolder = DestinationList.RootFolder;

            string destUrl = DestinationList.ParentWeb.Url + "/" + destFolder.Url + "/" + SourceItem.File.Name;

            //we cannot use SPFile.MoveTo or CopyTo when copying or moving files between sites.
            if (SourceItem.ParentList.ParentWeb.ID == DestinationList.ParentWeb.ID)
            {
                if (_options.OperationType == OperationType.Copy)
                {
                    this.SourceItem.File.CopyTo(destUrl, _options.Overwrite);
                }
                else
                {
                    this.SourceItem.File.MoveTo(destUrl, _options.Overwrite);
                }
            }
            else
            {
                SPFile targetFile = DestinationList.ParentWeb.GetFile(destUrl);

                if (targetFile.Exists && !_options.Overwrite)
                    throw new InvalidOperationException(string.Format("File at {0} already exists and overwrite option not specified!", destUrl));

                CopyFileWithHistoryCrossSite(this.SourceItem.File, destFolder, destUrl);

                if (_options.OperationType == OperationType.Move)

                    this.SourceItem.File.Delete();
            }

            //copy file attributes
            SPFile myFile = DestinationList.ParentWeb.GetFile(destUrl);

            //copy any  field values that match in both lists
            CopyFieldValues(this.SourceItem, myFile.Item,_options.LinkToOriginal);

            myFile.Item.SystemUpdate();

            return myFile.Item.ID;
        }
        private static void CopyFieldValues(SPListItem source, SPListItem destination)
        {
            CopyFieldValues(source, destination, false);
        }
        /// <summary>
        /// copies list properties excluding readonly fields and fields that are not present in destination list
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private static void CopyFieldValues(SPListItem source, SPListItem destination, bool linkToOriginal)
        {
            foreach (SPField sourceField in source.Fields) // loop thru source item fields
            {
                if (FieldShouldBeCopied(sourceField)) //can we copy this field?
                {
                    if (destination.Fields.ContainsField(sourceField.Title)) // does a field with same title exists in dest list
                    {
                        SPField destField = destination.Fields[sourceField.Title];

                        if (FieldShouldBeCopied(destField) && FieldShouldBeCopiedTo(sourceField, destField)) // do the field types match?
                        {
                            //user lookup ids are not valid when copying items cross site, so we need to create new lookup ids in destination site by calling SPWeb.EnsureUser()
                            if (sourceField.Type == SPFieldType.User && source[sourceField.Title] != null && source.ParentList.ParentWeb.ID != destination.ParentList.ParentWeb.ID)
                            {
                                try
                                {
                                    SPFieldUser fieldUser = sourceField as SPFieldUser;

                                    if (fieldUser.AllowMultipleValues == false)
                                    {
                                        SPFieldUserValue userValue = new SPFieldUserValue(source.ParentList.ParentWeb, source[sourceField.Title].ToString());

                                        destination[sourceField.Title] = destination.ParentList.ParentWeb.EnsureUser(userValue.User.LoginName);
                                    }
                                    else
                                    {
                                        SPFieldUserValueCollection useValCol = new SPFieldUserValueCollection(source.ParentList.ParentWeb, source[sourceField.Title].ToString());
                                        SPFieldUserValueCollection destValCol = new SPFieldUserValueCollection();

                                        foreach (SPFieldUserValue usr in useValCol)
                                        {
                                            using (SPWeb parentWeb = destination.ParentList.ParentWeb)
                                            {
                                                SPUser destUser = parentWeb.EnsureUser(usr.User.LoginName);
                                                destValCol.Add(new SPFieldUserValue(parentWeb, destUser.ID, string.Empty));
                                            }
                                        }
                                        destination[sourceField.Title] = destValCol;
                                    }
                                }
                                catch { }
                            }
                            else
                                destination[sourceField.Title] = source[sourceField.Title];
                        }
                    }
                }
            }
            if (linkToOriginal)
            {
                destination[ColumnHelper.PublishedFromInternalColumnName] = ListHelper.GetDisplayUrl(source);
            }
        }


        /// <summary>
        /// handles copying of list items
        /// </summary>
        protected virtual int CopyListItem()
        {
            SPListItem destItem = DestinationList.Items.Add();

            CopyFieldValues(SourceItem, destItem,_options.LinkToOriginal);

            // attachment routine
            if (SourceList.EnableAttachments && DestinationList.EnableAttachments)
            {
                if (_options.IncludeAttachments)
                {
                    AttachmentInfo[] attachments = AttachmentHelper.GetListItemAttachments(SourceItem);

                    foreach (AttachmentInfo ai in attachments)
                    {
                        destItem.Attachments.Add(ai.FileName, AttachmentHelper.ReadFully(ai.Stream, Convert.ToInt32(ai.Stream.Length)));
                    }
                }
            }

            destItem.SystemUpdate();

            if (_options.OperationType == OperationType.Move)
                SourceItem.Delete();

            return destItem.ID;
        }/// <summary>
        /// handles copying of list items
        /// </summary>
        protected virtual void UpdateListItem(int destinationItemID)
        {
            SPListItem destItem = DestinationList.GetItemById(destinationItemID);

            CopyFieldValues(SourceItem, destItem,_options.LinkToOriginal);

            // attachment routine
            if (SourceList.EnableAttachments && DestinationList.EnableAttachments)
            {
                if (_options.IncludeAttachments)
                {
                    AttachmentHelper.DeleteAllAttachments(destItem);

                    AttachmentInfo[] attachments = AttachmentHelper.GetListItemAttachments(SourceItem);

                    foreach (AttachmentInfo ai in attachments)
                    {
                        destItem.Attachments.Add(ai.FileName, AttachmentHelper.ReadFully(ai.Stream, Convert.ToInt32(ai.Stream.Length)));
                    }
                }
            }

            destItem.SystemUpdate();
        }

        /// <summary>
        /// copies a file from one site to another including file history. This method should not be used to copy files 
        /// within the same site, SPFile.CopyTo() should be used instead.
        /// Special thanks to dink, who wrote an article about copying files with version history at http://www.sharepointblogs.com/dez/archive/2007/11/30/moving-copying-documents-between-libraries-with-metadata-including-version-history.aspx
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destination"></param>
        /// <param name="destinationUrl"></param>
        static void CopyFileWithHistoryCrossSite(SPFile sourceFile, SPFolder destination, string destinationUrl)
        {
            //prevens security validation errors
            //sourceFile.ParentFolder.ParentWeb.Site.WebApplication.FormDigestSettings.Enabled = false;

            //custom sorted list used to reorder versions
            SortedList intListofFileVers = new SortedList();

            ICollection items = intListofFileVers.Keys;
            try
            {

                if (sourceFile.Versions.Count != 0)
                {
                    foreach (SPFileVersion ver in sourceFile.Versions)
                    {
                        string tempKey = "";

                        //get version ids
                        tempKey = Regex.Replace(ver.Url, "_vti_history/", "");
                        tempKey = Regex.Replace(tempKey, "/" + sourceFile.ParentFolder.Url, "");
                        tempKey = Regex.Replace(tempKey, "/" + sourceFile.Name, "");

                        intListofFileVers.Add(int.Parse(tempKey), "");


                    }

                    //since items in sorted list are now actually sorted correctly
                    //we start with this list in order to process the versions
                    //to copy them in the correct order
                    foreach (object key in items)
                    {
                        //as we iterate the keys in the sorted list (the version numbers)
                        //we then run a comparison on the actual versions to find which one matches
                        //the key so we can process each one in order
                        foreach (SPFileVersion newVer in sourceFile.Versions)
                        {
                            string temp = "";

                            //parses version number from previous versions URL again 
                            //in order to compare it to key stored in SortedList.
                            temp = Regex.Replace(newVer.Url, "_vti_history/", "");
                            temp = Regex.Replace(temp, "/" + sourceFile.ParentFolder.Url, "");
                            temp = Regex.Replace(temp, "/" + sourceFile.Name, "");

                            //checks to see if version matches key 
                            if (temp == key.ToString())
                            {
                                //opens file for processing and calls method to determine major/minor status
                                byte[] verFile = newVer.OpenBinary();

                                int num = (int.Parse(temp));

                                int baseNum = 512;

                                decimal d = num / baseNum;

                                int i = (int)Math.Floor(d) * 512;


                                SPFile copFileVers = destination.Files.Add(destinationUrl, verFile, true);


                            }
                        }
                    }
                }
            }
            catch  //if error copying
            {

            }
            //Last step which copies current version
            byte[] binFile;

            binFile = sourceFile.OpenBinary();

            destination.Files.Add(destinationUrl, binFile, true);

            //  sourceFile.ParentFolder.ParentWeb.Site.WebApplication.FormDigestSettings.Enabled = true;

        }


        /// <summary>
        /// determines if field should be copied
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool FieldShouldBeCopied(SPField field)
        {
            if (field.ReadOnlyField)
            {
                return false;
            }
            else
            {
                if (((field.Type != SPFieldType.Invalid) && (field.Type != SPFieldType.Attachments) && (field.Type != SPFieldType.File)) && (field.Type != SPFieldType.Computed))
                {
                    if (field.InternalName == ColumnHelper.PublishedToInternalColumnName || field.InternalName == ColumnHelper.PublishedFromInternalColumnName)
                        return false;

                    return (!(field.InternalName == "_HasCopyDestinations") && ((field.InternalName != "_CopySource") && !(field.InternalName == "ContentTypeId")));
                }
            }
            return false;
        }


        /// <summary>
        /// makes sure that the field can be copied
        /// </summary>
        /// <param name="fromField"></param>
        /// <param name="toField"></param>
        /// <returns></returns>
        public static bool FieldShouldBeCopiedTo(SPField fromField, SPField toField)
        {
            if (fromField.Type == toField.Type)
            {
                if (fromField.Type != SPFieldType.Lookup)
                {
                    if (fromField.Type == SPFieldType.User)
                    {
                        SPFieldUser user = fromField as SPFieldUser;
                        SPFieldUser user2 = toField as SPFieldUser;

                        if (user.AllowMultipleValues && !user2.AllowMultipleValues)
                            return false;

                        if ((user2.SelectionMode == SPFieldUserSelectionMode.PeopleOnly) && (user.SelectionMode == SPFieldUserSelectionMode.PeopleAndGroups))
                        {

                            return false;
                        }
                    }
                    return true;
                }
                SPFieldLookup lookup = fromField as SPFieldLookup;
                SPFieldLookup lookup2 = toField as SPFieldLookup;

                if (!(lookup.LookupWebId == lookup2.LookupWebId))
                {

                    return false;
                }
                return (lookup.LookupList == lookup2.LookupList);
            }
            return false;
        }


        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}