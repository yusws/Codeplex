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
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace MyLocalBroadband.Activities.WSS
{
	public partial class CreateSiteGroup: SequenceActivity
	{
		public CreateSiteGroup()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(CreateSiteGroup));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(CreateSiteGroup));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(CreateSiteGroup));
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(CreateSiteGroup));
        public static readonly DependencyProperty GroupDescriptionProperty = DependencyProperty.Register("GroupDescription", typeof(string), typeof(CreateSiteGroup));
        public static readonly DependencyProperty GroupOwnerProperty = DependencyProperty.Register("GroupOwner", typeof(string), typeof(CreateSiteGroup));
        public static readonly DependencyProperty GroupMembersProperty = DependencyProperty.Register("GroupMembers", typeof(string), typeof(CreateSiteGroup));
        public static readonly DependencyProperty WebURLProperty = DependencyProperty.Register("WebURL", typeof(string), typeof(CreateSiteGroup));
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
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }
        public string GroupDescription
        {
            get { return (string)GetValue(GroupDescriptionProperty); }
            set { SetValue(GroupDescriptionProperty, value); }
        }
        public string GroupOwner
        {
            get { return (string)GetValue(GroupOwnerProperty); }
            set { SetValue(GroupOwnerProperty, value); }
        }
        public string GroupMembers
        {
            get { return (string)GetValue(GroupMembersProperty); }
            set { SetValue(GroupMembersProperty, value); }
        }
        public string WebURL
        {
            get { return (string)GetValue(WebURLProperty); }
            set { SetValue(WebURLProperty, value); }
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
                using (SPSite site = new SPSite(WebURL, __Context.Web.CurrentUser.UserToken))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (SiteGroupExists(web, GroupName))
                        {
                            throw new Exception("Site Group, " + GroupName + ", Already Exists in Site " + WebURL);
                        }

                        web.SiteGroups.Add(GroupName, web.CurrentUser, web.CurrentUser, GroupDescription);
                        SPGroup group = GetSiteGroup(web, GroupName);

                        if (group == null)
                        {
                            throw new Exception("Failed to Create site group, " + GroupName + ", in Site " + WebURL);
                        }

                        SetGroupOwner(group);

                        group.Users.Remove(web.CurrentUser.LoginName);
                        AddGroupMembers(group);

                        string message = "Created Group " + GroupName + " in site " + WebURL;
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

        #region Private methods
        private bool SiteGroupExists(SPWeb web, string groupName) 
        {
            foreach (SPGroup group in web.SiteGroups)
                if (group.Name.ToLower() == groupName.ToLower())
                    return true;
            return false;
        }
        private SPGroup GetSiteGroup(SPWeb web, string groupName)
        {
            foreach (SPGroup group in web.SiteGroups)
                if (group.Name.ToLower() == groupName.ToLower())
                    return group;
            return null;
        }
        private void SetGroupOwner(SPGroup group)
        {
            using (SPWeb web = group.ParentWeb)
            {
                SPGroup ownerGroup = GetSiteGroup(web, GroupOwner);

                if (ownerGroup != null)
                {
                    group.Owner = ownerGroup;
                }
                else
                {
                    web.SiteUsers.Add(GroupOwner, "", "", "");
                    group.Owner = web.SiteUsers[GroupOwner];
                }
                group.Update();
            }
        }
        private void AddGroupMembers(SPGroup group)
        {
            if (!string.IsNullOrEmpty(GroupMembers))
            {
                string[] logins = GroupMembers.Split(';');
                foreach (string login in logins)
                {
                    group.Users.Add(login, "", "", "");
                }
                group.Update();
            }
        }
        #endregion
    }
}
