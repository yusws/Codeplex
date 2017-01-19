using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Text;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using MyLocalBroadband.Activities.WSS.Utilities;

namespace MyLocalBroadband.Activities.WSS
{
	public partial class ActivateFeature: SequenceActivity
	{
		public ActivateFeature()
		{
			InitializeComponent();
        }
        #region DependencyProperty Declarations
        public static readonly DependencyProperty __ContextProperty = DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(ActivateFeature));
        public static readonly DependencyProperty __ListIdProperty = DependencyProperty.Register("__ListId", typeof(string), typeof(ActivateFeature));
        public static readonly DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(ActivateFeature));
        public static readonly DependencyProperty FeatureScopeProperty = DependencyProperty.Register("FeatureScope", typeof(string), typeof(ActivateFeature));
        public static readonly DependencyProperty FeatureTitleProperty = DependencyProperty.Register("FeatureTitle", typeof(string), typeof(ActivateFeature));
        public static readonly DependencyProperty WebURLProperty = DependencyProperty.Register("WebURL", typeof(string), typeof(ActivateFeature));
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
        public string FeatureScope
        {
            get { return (string)GetValue(FeatureScopeProperty); }
            set { SetValue(FeatureScopeProperty, value); }
        }
        public string FeatureTitle
        {
            get { return (string)GetValue(FeatureTitleProperty); }
            set { SetValue(FeatureTitleProperty, value); }
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
                Guid featureDefinitionID = GetFeatureID();
                if (featureDefinitionID == Guid.Empty)
                {
                    throw new ApplicationException("Feature titled " + FeatureTitle + " not defined in scope " + FeatureScope.ToString());
                }

                using (SPSite targetSite = new SPSite(WebURL, __Context.Site.UserToken))
                {
                    if (GetTargetScope() == SPFeatureScope.Site)
                    {
                        targetSite.Features.Add(featureDefinitionID, true);
 
                    }
                    else if (GetTargetScope() == SPFeatureScope.Web)
                    {
                        using (SPWeb targetWeb = targetSite.OpenWeb())
                        {
                            targetWeb.Features.Add(featureDefinitionID, true);
                        }
                    }

                    string message = GetTargetScope().ToString() + " feature, " + FeatureTitle + "(" + featureDefinitionID.ToString() + "), activated at " + WebURL;
                    WorkflowHistoryLogger.LogMessage(executionContext, SPWorkflowHistoryEventType.None, "Complete", UserID, message);
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

        #region Private Methods
        private Guid GetFeatureID()
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo(1033);
            SPFeatureDefinitionCollection featureDefinitions = SPFarm.Local.FeatureDefinitions;
           
            foreach (SPFeatureDefinition featureDefinition in featureDefinitions)
            {
                if (featureDefinition.GetTitle(cultureInfo).ToLower() == FeatureTitle.ToLower())
                {
                    if (featureDefinition.Scope == GetTargetScope())
                    {
                        return featureDefinition.Id;
                    }
                }
            }
            return Guid.Empty;
        }
        private SPFeatureScope GetTargetScope()
        {
            return (SPFeatureScope)Enum.Parse(typeof(SPFeatureScope), FeatureScope);
        }
        #endregion
	}
}
