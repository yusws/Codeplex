using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint.WorkflowActions;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Threading;
using System.IO;
using System.Security.Principal;
using MyLocalBroadband.Logging;

namespace MyLocalBroadband.Activities.WSS
{
	class WorkflowHistoryLogger
	{
        public static void LogError(ActivityExecutionContext executionContext,int userID, Exception ex)
        {
            // create error message
            string errorMessage = ex.Message + " " + ex.StackTrace;

            // add any inner exceptions
            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                errorMessage += "Inner Error: " + innerException.Message + " " + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            // log error
            LogMessage(executionContext, SPWorkflowHistoryEventType.WorkflowError, "Failure", errorMessage,userID, true);
        }

        public static void LogMessage(ActivityExecutionContext executionContext, SPWorkflowHistoryEventType eventType, string outcome,int userID, string message)
        {
            LogMessage(executionContext, eventType, outcome, message,userID, true);
        }
        
        public static void LogMessage(ActivityExecutionContext executionContext,SPWorkflowHistoryEventType eventType,string outcome,string message,int userID, bool ccULS)
        {
            try
            {
                //write to Workflow History List
                ISharePointService spService = (ISharePointService)executionContext.GetService(typeof(ISharePointService));
                spService.LogToHistoryList(executionContext.ContextGuid, eventType, userID, TimeSpan.MinValue, outcome, message, message);

                //Write to ULS trace log
                if (ccULS)
                {
                    string source = executionContext.Activity.Name;
                    TraceProvider.TraceSeverity traceSeverity = TraceProvider.TraceSeverity.InformationEvent;

                    if (eventType == SPWorkflowHistoryEventType.WorkflowError)
                        traceSeverity = TraceProvider.TraceSeverity.Exception;

                    ULS.LogMessage(source, message, "Site Management", traceSeverity);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SiteManagementActivity", "History Logging Failed:" + ex.ToString());
            }
        }
    }
}
