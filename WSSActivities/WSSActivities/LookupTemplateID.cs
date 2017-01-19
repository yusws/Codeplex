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
	public partial class LookupTemplateID: SequenceActivity
	{
		public LookupTemplateID()
		{
			InitializeComponent();
        }

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(LookupTemplateID));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(LookupTemplateID));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(LookupTemplateID));
        public static readonly DependencyProperty TemplateNameProperty = DependencyProperty.Register("TemplateName", typeof(string), typeof(LookupTemplateID));
        public static readonly DependencyProperty LCIDProperty = DependencyProperty.Register("LCID", typeof(UInt32), typeof(LookupTemplateID));
        public static readonly DependencyProperty TemplateIDProperty = DependencyProperty.Register("TemplateID", typeof(string), typeof(LookupTemplateID));
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
        public string TemplateName
        {
            get { return (string)GetValue(TemplateNameProperty); }
            set { SetValue(TemplateNameProperty, value); }
        }
        public UInt32 LCID
        {
            get { return (UInt32)GetValue(LCIDProperty); }
            set { SetValue(LCIDProperty, value); }
        }
        public string TemplateID
        {
            get { return (string)GetValue(TemplateIDProperty); }
            set { SetValue(TemplateIDProperty, value); }
        }
        private int UserID
        {
            get { return __Context.Web.CurrentUser.ID; }
        }
        #endregion

        #region Execute Method
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            TemplateID = string.Empty;
            try
            {
                using (SPSite currentSite = new SPSite(__Context.Web.Site.ID, __Context.Web.CurrentUser.UserToken))
                {
                    TemplateID = LookupTemplateIDInGallery(currentSite);
                    if (string.IsNullOrEmpty(TemplateID))
                    {
                        TemplateID = LookupTemplateIDInDefinitions(currentSite);

                        if (string.IsNullOrEmpty(TemplateID))
                        {
                            string message = "Failed to locate templateID for template named " + TemplateName + " LCID " + LCID + " in site " + currentSite.Url;
                            WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.WorkflowError, "Failure", UserID, message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WorkflowHistoryLogger.LogError(executionContext, UserID, ex);
                return ActivityExecutionStatus.Faulting;
            }

            if (string.IsNullOrEmpty(TemplateID))
            {          
                return ActivityExecutionStatus.Faulting;
            }
            return ActivityExecutionStatus.Closed;
        }
        #endregion

        #region Private methods
        private string LookupTemplateIDInGallery(SPSite site)
        {
            SPWebTemplateCollection templates = site.GetCustomWebTemplates(LCID);
            foreach (SPWebTemplate template in templates)
            {
                if ((template.Name.ToLower() == TemplateName.ToLower())||(template.Title.ToLower() == TemplateName.ToLower()))
                {
                    return template.Name;
                }
            }
            return string.Empty;
        }
        private string LookupTemplateIDInDefinitions(SPSite site)
        {
            SPWebTemplateCollection templates = site.GetWebTemplates(LCID);
            foreach (SPWebTemplate template in templates)
            {
                if ((template.Name.ToLower() == TemplateName.ToLower()) || (template.Title.ToLower() == TemplateName.ToLower()))
                {
                    return template.Name;
                }
            }
            return string.Empty;
        }
        #endregion
	}
}
