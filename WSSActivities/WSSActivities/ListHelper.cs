using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Web;

namespace MyLocalBroadband.Activities.WSS
{
	class ListHelper
    {
        #region Public Methods
        public static SPList GetSPListFromURL(string url)
        {
            using (SPSite site = new SPSite(url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    return web.GetList(url);
                }
            }
        }
        public static SPList GetSPListFromURL(string url, SPUserToken userToken)
        {
            using (SPSite site = new SPSite(url,userToken))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    return web.GetList(url);
                }
            }
        }

        public static SPListItem GetSPItemFromURL(string url)
        {
            SPListItem item = null;
            try
            {
                SPList list = GetSPListFromURL(url);
                string query = url.Substring(url.IndexOf('?'));
                NameValueCollection parameters = HttpUtility.ParseQueryString(query);
                int id = Convert.ToInt32(parameters["id"]);
                item = list.GetItemById(id);
            }
            catch 
            {
                item = null;
            }

            return item;
        }
        public static SPFolder GetSPFolderFromURL(string url)
        {
            Uri uri = new Uri(url);
            using (SPSite site = new SPSite(url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    //each list, even a non document library list has at least a root folder.
                    SPFolder folder = web.GetFolder(uri.AbsolutePath);

                    if (folder.Exists)
                    {
                        //adjust for forms directory in document libraries
                        if (web.Lists[folder.ParentListId].BaseType == SPBaseType.DocumentLibrary)
                        {
                            if (url.ToLower().IndexOf(web.Lists[folder.ParentListId].Forms[PAGETYPE.PAGE_DISPLAYFORM].Url.ToLower()) >= 0)
                            {
                                return folder.ParentFolder;
                            }
                        }
                        return folder;
                    }
                    else
                    {
                        SPFile file = web.GetFile(uri.AbsolutePath);
                        if (file.Exists)
                        {
                            folder = file.ParentFolder;
                            //adjust for forms directory in document libraries
                            if (web.Lists[folder.ParentListId].BaseType == SPBaseType.DocumentLibrary)
                            {
                                if (url.ToLower().IndexOf(web.Lists[folder.ParentListId].Forms[PAGETYPE.PAGE_DISPLAYFORM].Url.ToLower()) >= 0)
                                {
                                    return file.ParentFolder.ParentFolder;
                                }
                            }
                            return file.ParentFolder;
                        }
                    }
                }
            }
            throw new ApplicationException(string.Format("Folder at {0} does not exist!", url));
        }


        public static string GetDisplayUrl(SPListItem item)
        {
            using (SPWeb web = item.Web)
            {
                return (web.Url + "/" + item.ParentList.Forms[PAGETYPE.PAGE_DISPLAYFORM].Url + "?id=" + item.ID).ToLower();
            }
        }
        #endregion
    }
}
