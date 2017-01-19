This is a class library I use to handle logging in the SharePoint uls logs.  I used to put these classes (or similar)
into every project I created, but that got old in a hurry.


The TraceProvider class was ripped directly from the WSS SDK trace provider example.
The ULS class was modeled after a class within the community kit (www.codeplex.com/cks).

When you're developing, you can just add a reference to the .dll in your project.  On your SharePoint server you can deploy the .dll using the solution file or just drop the .dll in the GAC.

This solution was created using the Smart Template addin for Visual Studio 2008 (www.codeplex.com/smarttemplates)


There are 3 public static methods available in the ULS class here.  So far they've covered all my needs, but let me know if you find a regular use for another method.

static void LogMessage(string source, string message, string category, TraceProvider.TraceSeverity severity)
static void LogError(string source, string errorMessage, string errorCategory)
static void LogError(string source, Exception ex)