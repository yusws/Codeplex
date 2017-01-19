# WSS Security Provider
The original project is located here: https://wsssecurityprovider.codeplex.com/


##WSS Security Provider
WSS Security Provider - Forms Based Authentication security Provider. Allows seperate security stores for multiple host header sites running within a single web ap. 
Powershell Warmup script, Powershell Backup script. SQL Unlock FBA User script.


##External References
the WSS Security Provider makes use of these other codeplex projects: 

- ULS Logger .dll that is used to log messages to the SharePoint ULS. 
- Web Config Manager .dll that is used to write entries to the web.config when the features are activated. 
- Smart Templates Visual Studio Template used to package solution file (.wsp)


##Powershell Scripts 
###Site Collection Warmup
Special thanks goes out to Darrin Bishop on this. His blog entry was the starting point of my first attempt at PowerShell scripting.

This script is similar to the prolific vb script for warming up a server. However, this one is smart enough to find every application on the current server and then every site collection in each of those web apps. It then hits the default page in each site collection.

###Site Collection Backup
Special thanks goes out to Darrin Bishop on this. His blog entry was the starting point of my first attempt at PowerShell scripting.


This script can be used to backup every site collection within a web app. You set it up with a web app url and a backup directory. It will locate all the site collections in the web app and call stsadm to back each of them up. 

Backups are created in a subdirectory named in the format yyyyMMdd. You can also give a number of days worth of backups to keep. It will then look for a folder by the name yyyyMMdd created x days ago and deletes it.


##SQL Scripts 


###ASPNetProvider Unlock User Accounts

I added a feature to my asp.net membership provider that automatically unlocks user accounts after a given amount of time. 

I just created a stored procedure that unlocks the accounts and a job that runs the procedure every 10 minutes. 


