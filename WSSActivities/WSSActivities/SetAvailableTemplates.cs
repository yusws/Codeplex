using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.ObjectModel;
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
	public partial class SetAvailableTemplates: SequenceActivity
	{
		public SetAvailableTemplates()
		{
			InitializeComponent();
		}

        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(SetAvailableTemplates));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(SetAvailableTemplates));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(SetAvailableTemplates));
        public static readonly DependencyProperty WebURLProperty = DependencyProperty.Register("WebURL", typeof(string), typeof(SetAvailableTemplates));
        public static readonly DependencyProperty TemplateIDsProperty = DependencyProperty.Register("TemplateIDs", typeof(string), typeof(SetAvailableTemplates));
        public static readonly DependencyProperty LCIDProperty = DependencyProperty.Register("LCID", typeof(UInt32), typeof(SetAvailableTemplates));
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
        public string TemplateIDs
        {
            get { return (string)GetValue(TemplateIDsProperty); }
            set { SetValue(TemplateIDsProperty, value); }
        }
        public UInt32 LCID
        {
            get { return (UInt32)GetValue(LCIDProperty); }
            set { SetValue(LCIDProperty, value); }
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
                        string[] templateIDs = TemplateIDs.Split(';');
                        Collection<SPWebTemplate> templateList = new Collection<SPWebTemplate>();
                        foreach (string templateid in templateIDs)
                        {
                            SPWebTemplate template = LookupTemplate(site, templateid);
                            if (template != null)
                                templateList.Add(template);
                            else
                            {
                                throw new Exception("Could not locate template, " + templateid + ", in site, " + WebURL + ".");
                            }
                        }
                        web.SetAvailableWebTemplates(templateList, LCID);
                        web.Update();

                        string message = "Web: " + WebURL + " templates restricted to: " + TemplateIDs;
                        WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
                    }
                }
            }
            catch (Exception ex)
            {
                WorkflowHistoryLogger.LogError(executionContext,UserID, ex);
                return ActivityExecutionStatus.Faulting;
            }
            return ActivityExecutionStatus.Closed;
        }
        #endregion

        #region Private methods
        private SPWebTemplate LookupTemplate(SPSite site, string templateName)
        {
            SPWebTemplate template = LookupTemplateIDInGallery(site, templateName);
            if (template != null)
                return template;
            else
                return LookupTemplateIDInDefinitions(site, templateName);
        }
        private SPWebTemplate LookupTemplateIDInGallery(SPSite site,string templateName)
        {
            SPWebTemplateCollection templates = site.GetCustomWebTemplates(LCID);
            foreach (SPWebTemplate template in templates)
            {
                if ((template.Name.ToLower() == templateName.ToLower()) || (template.Title.ToLower() == templateName.ToLower()))
                {
                    return template;
                }
            }
            return null;
        }
        private SPWebTemplate LookupTemplateIDInDefinitions(SPSite site,string templateName)
        {
            SPWebTemplateCollection templates = site.GetWebTemplates(LCID);
            foreach (SPWebTemplate template in templates)
            {
                if ((template.Name.ToLower() == templateName.ToLower()) || (template.Title.ToLower() == templateName.ToLower()))
                {
                    return template;
                }
            }
            return null;
        }
        #endregion
	}
}
