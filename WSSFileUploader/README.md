# WSS File Uploader
The original project is located here: https://wssfileuploader.codeplex.com/


##Project Description
Console Application for uploading files to a SharePoint library. Can be called from the command line to upload files from a batch script. Works with WSS 3.0
WSS File Uploader 1.0

##About
This is a quick and dirty console application you can use to upload files to a SharePoint document library scripts.

I'm not proud of the code, but if you want to upload a file from a batch script or powershell script, this console app will get you there.

It's written to work against WSS 3.0 with forms based authentication. It doesn't support putting files in folders within the document library.

##Example 
WSSFileUpload user=testaccount password=password1 site=http://www.mylocalbroadband.com library=shared%20documents files=c:\\test.txt;c:\\test1.txt;c:\\test2.jpg
Note: Arguments may be in any order.

Argument defaults can also be set in the .config file.
If you set the default values in the config file, you may override any or all of the values by passing them in the command call.

