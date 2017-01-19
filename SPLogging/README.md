# ULS Logger
The original project is located here: https://splogger.codeplex.com/


##Project Description
A class library to assist your SharePoint projects logging to the ULS. This is wrapped in a .wsp solution and is an installable feature.

My Local Broadband ULS Logger
This is a class library installable as a solution that assists with logging messages to the ULS.
ULS Logger 1.0

This project makes use of a TraceProvider class that was ripped directly from the WSS SDK trace provider example.
The ULS class was modeled after a class within the community kit (cks.codeplex.com).

When you're developing, you can just add a reference to the .dll in your project. On your SharePoint server you can deploy the .dll using the solution file or just drop the .dll in the GAC.

This solution was created using the Smart Template addin for Visual Studio 2008 (smarttemplates.codeplex.com)


There are 3 public static methods available in the ULS class here. So far they've covered all my needs, but let me know if you find a regular use for another method.

static void LogMessage(string source, string message, string category, TraceProvider.TraceSeverity severity)
static void LogError(string source, string errorMessage, string errorCategory)
static void LogError(string source, Exception ex)
