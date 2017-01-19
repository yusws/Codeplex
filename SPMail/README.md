# SPMail
The original version of this project is available at https://spmail.codeplex.com/

##About
A console application that will pull mail from a POP3 mail server and dump it into a local directory. You can configure SharePoint's incoming mail settings to pick up mail from this directory. Errors and Information are logged to SharePoint's ULS.
SPMail 1.0

##Setup
1. Create a folder where you want your email to get processed, such as C:\SPMail
2. In Central Admin; operations>incoming email settings, set your implementation to retrieve mail from a drop folder and enter the folder path you used in step 1. 
3. Put the spmail utility on your machine and set up its configuration file. Any POP3 enabled email server will work, but this example uses branded Gmail (part of Google apps)

    &lt;configuration&gt;

    &lt;appSettings&gt;

    &lt;!-- Pop Client Settings--&gt;

    &lt;add key="popHost" value="pop.gmail.com"/&gt;

    &lt;add key="popPort" value="995"/&gt;

    &lt;add key="useSSL" value="true"/&gt;

    &lt;add key="popUser" value="youraccount@yourdomain.com"/&gt;

    &lt;add key="popPassword" value="password"/&gt;

    &lt;add key="traceToConsole" value="true"/&gt;

    &lt;add key="deleteAfterRetrieve" value="true"/&gt;
    
    &lt;!-- Drop Box Settings--&gt;

    &lt;add key="dropFolderPath" value="c:\spIncomingMail&gt;

    &lt;/appSettings&gt;

    &lt;/configuration&gt;
4. Set up a timer job to run spmail.exe on a regular basis, like every 5 minutes.

##Setup Notes 
You’ll need to get your mail into your pop account. You can do that with groups, aliases, or a catch all account. 
SharePoint ignores everything after the @ when processing incoming mail, so you may be able to use that to your advantage if you’re in a hosted environment. i.e. I can forward listname@mydomain.com to listname@hosteddomain.com and everything should work fine. 
I’ve successfully used this to pull email from GMAIL, however this application should work for any POP3 server. 
A Note on Gmail: Gmail ignores the deleteAfterRetrieve commands. You’ll need to configure this in your POP setup within your Gmail account.

References
The Net project used in this solution was originally from www.codeproject.com/KB/IP/NetPopMimeClient.aspx
The MyLocalBroadband.Logging.dll is a ULS logging solution that I’ve written and it is available at SPLogger.codeplex.com
