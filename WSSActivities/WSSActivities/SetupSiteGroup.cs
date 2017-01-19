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
	public partial class SetupSiteGroup: SequenceActivity
	{
		public SetupSiteGroup()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetupSiteGroup));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetupSiteGroup));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetupSiteGroup));
        public static readonly DependencyProperty WebURLProperty = DependencyProperty.Register("WebURL", typeof(string), typeof(SetupSiteGroup));
        public static readonly DependencyProperty SiteGroupNameProperty = DependencyProperty.Register("SiteGroupName", typeof(string), typeof(SetupSiteGroup));
        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register("Role", typeof(string), typeof(SetupSiteGroup));
        public static readonly DependencyProperty ClearQuickLaunchGroupsProperty = DependencyProperty.Register("ClearQuickLaunchGroups", typeof(bool), typeof(SetupSiteGroup));
        public static readonly DependencyProperty ClearInheritedPermissionsProperty = DependencyProperty.Register("ClearInheritedPermissions", typeof(bool), typeof(SetupSiteGroup));
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
        public string WebURL
        {
            get { return (string)GetValue(WebURLProperty); }
            set { SetValue(WebURLProperty, value); }
        }
        public string SiteGroupName
        {
            get { return (string)GetValue(SiteGroupNameProperty); }
            set { SetValue(SiteGroupNameProperty, value); }
        }
        public string Role
        {
            get { return (string)GetValue(RoleProperty); }
            set { SetValue(RoleProperty, value); }
        }
        public bool ClearQuickLaunchGroups
        {
            get { return (bool)GetValue(ClearQuickLaunchGroupsProperty); }
            set { SetValue(ClearQuickLaunchGroupsProperty, value); }
        }
        public bool ClearInheritedPermissions
        {
            get { return (bool)GetValue(ClearInheritedPermissionsProperty); }
            set { SetValue(ClearInheritedPermissionsProperty, value); }
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
                        SPGroup siteGroup = GetSiteGroup(web);
                        if (siteGroup == null)
                        {
                            throw new Exception("Site Group, " + SiteGroupName + ", does not exists in Site " + WebURL);
                        }

                        SetSiteGroupAssociation(web, siteGroup);
                        SetupQuickLaunch(web, siteGroup);
                        SetGroupPermissions(web, siteGroup);

                        string message = "Site Group " + SiteGroupName + " associated to site " + WebURL;
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
        private void SetSiteGroupAssociation(SPWeb web,SPGroup group)
        {
            string roleSiteProperty = "";
            switch (Role)
            {
                case "owner":
                    roleSiteProperty = "vti_associateownergroup";
                    break;
                case "member":
                    roleSiteProperty = "vti_associatemembergroup";
                    break;
                default:
                    roleSiteProperty = "vti_associatevisitorgroup";
                    break;
            }
            web.Properties[roleSiteProperty] = group.ID.ToString();
            web.Properties.Update();
        }
        private SPGroup GetSiteGroup(SPWeb web)
        {
            string groupName = SiteGroupName.ToLower();
            foreach (SPGroup group in web.SiteGroups)
                if (group.Name.ToLower() == groupName)
                    return group;
            return null;
        }
        private SPGroup GetWebGroup(SPWeb web)
        {
            string groupName = SiteGroupName.ToLower();
            foreach (SPGroup group in web.Groups)
                if (group.Name.ToLower() == groupName)
                    return group;
            return null;
        }
        private void SetupQuickLaunch(SPWeb web,SPGroup group)
        {
            string groupList = "";
            if (!ClearQuickLaunchGroups)
            {
                groupList = web.Properties["vti_associategroups"];
            }

            if (string.IsNullOrEmpty(groupList))
            {
                groupList = group.ID.ToString();
            }
            else
            {
                groupList += ";" + group.ID.ToString();
            }
            web.Properties["vti_associategroups"] = groupList;
            web.Properties.Update();
        }
        private void SetGroupPermissions(SPWeb web, SPGroup group)
        {
            SetBaseLinePermissions(web,group);

            string roleName = "";
            switch (Role)
            {
                case "owner":
                    roleName = "Full Control";
                    break;
                case "member":
                    roleName = "Contribute";
                    break;
                default:
                    roleName = "Read";
                    break;
            }
            SPRoleAssignment assignment = new SPRoleAssignment(group);
            SPRoleDefinition role = web.RoleDefinitions[roleName]; 
            assignment.RoleDefinitionBindings.Add(role);
            web.RoleAssignments.Add(assignment);

        }
        private void SetBaseLinePermissions(SPWeb web, SPGroup group)
        {
            if (ClearInheritedPermissions)
            {
                web.ResetRoleInheritance();
            }
            if (!web.HasUniqueRoleAssignments)
            {
                web.BreakRoleInheritance(!ClearInheritedPermissions);
            }
            SPGroup existingGroup = GetWebGroup(web);
            if(existingGroup != null)
                web.Groups.Remove(existingGroup.Name);
        }
        #endregion
	}
}
