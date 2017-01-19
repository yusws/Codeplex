using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MyLocalBroadband.Logging
{
    public static class ULS
    {
        public static void LogMessage(string source, string message, string category, TraceProvider.TraceSeverity severity)
        {
            // log error to ULS log
            TraceProvider.WriteTrace(0, severity, Guid.NewGuid(), Assembly.GetExecutingAssembly().FullName, source, category, message);
        }
        public static void LogError(string source, string errorMessage, string errorCategory)
        {
            // log error to ULS log
            TraceProvider.WriteTrace(0, TraceProvider.TraceSeverity.CriticalEvent, Guid.NewGuid(), Assembly.GetExecutingAssembly().FullName, source, errorCategory, errorMessage);
        }

        public static void LogError(string source, Exception ex)
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
            LogError(source, errorMessage, ex.GetType().ToString());
        }
    }
}
