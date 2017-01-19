using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WSSFileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            string user;
            string password;
            string siteURL;
            string sourceURL;
            string library;
            string fileList;
            string[] destinationURLs;
            string[] dest = new string[1];

            if (args.Length == 1 && (args[0].ToLower() == "help" || args[0].ToLower() == "?"))
            {
                ShowHelp();
            }
            else
            {
                try
                {
                    GetParameters(args, out user, out password, out siteURL, out sourceURL, out library, out fileList);
                    System.Net.CookieContainer cookieJar = Authenticate(siteURL, user, password);
                    string[] files = fileList.Split(';');
                    foreach (string file in files)
                    {
                        try
                        {
                            dest[0] = siteURL + "/" + library + "/" + file.Substring(file.LastIndexOf("\\") + 1);
                            destinationURLs = dest;
                            Console.WriteLine(dest[0]);
                            Copy_ws.CopyResult[] results = UploadFile(siteURL, cookieJar, file, destinationURLs, file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        static void GetParameters(string[] args, out string user, out string password, out string siteURL, out string sourceURL, out string library, out string fileList)
        {
            //set defaults from app.config
            user = Properties.Settings.Default.UserNameDefault;
            password = Properties.Settings.Default.PasswordDefault;
            siteURL = Properties.Settings.Default.SiteURLDefault;
            fileList = Properties.Settings.Default.FilesDefault;
            sourceURL = Properties.Settings.Default.SiteURLDefault;
            library = Properties.Settings.Default.LibraryDefault;

            //override defaults with command line arguments
            foreach (string arg in args)
            {
                string[] pair = arg.Split('=');
                switch (pair[0].ToLower())
                {
                    case "user":
                        user = pair[1];
                        break;

                    case "password":
                        password = pair[1];
                        break;

                    case "site":
                        siteURL = pair[1];
                        break;

                    case "library":
                        library = pair[1];
                        break;

                    case "sourceurl":
                        sourceURL = pair[1];
                        break;

                    case "files":
                        fileList = pair[1];
                        break;
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("\n\n\nWSSFileUpload user password site library files\n\n");
            Console.WriteLine("ex.\nWSSFileUpload user=testaccount password=password1 site=http://www.mylocalbroadband.com library=shared%20documents files=c:\\test.txt;c:\\test1.txt;c:\\test2.jpg");
            Console.WriteLine("Note: Arguments may be in any order.");
            Console.WriteLine("Argument defaults can also be set in the .config file.\n");
            Console.WriteLine("If you set the default values in the config file, you may override any or all of the values by passing them in the command call.\n");
        }
        static System.Net.CookieContainer Authenticate(string siteURL, string user, string password)
        {
            CookieContainer cookieJar = new CookieContainer();
            Authentication_ws.Authentication service = null;
            try
            {
                service = new Authentication_ws.Authentication();
                service.Url = siteURL + "/_vti_bin/Authentication.asmx";
                service.CookieContainer = cookieJar;
                Authentication_ws.LoginResult result = service.Login(user, password);
            }
            finally
            {
                if (service != null)
                    service.Dispose();
            }
            return cookieJar;
        }
        static Copy_ws.CopyResult[] UploadFile(string siteURL, CookieContainer cookieJar, string sourceURL, string[] destinationURLs, string file)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Copy_ws.FieldInformation[] fieldInfo = new WSSFileUpload.Copy_ws.FieldInformation[0];
            Copy_ws.CopyResult[] results;

            Copy_ws.Copy service = null;
            try
            {
                service = new WSSFileUpload.Copy_ws.Copy();
                service.CookieContainer = cookieJar;
                service.Url = siteURL + "/_vti_bin/Copy.asmx";
                service.CopyIntoItems(sourceURL, destinationURLs, fieldInfo, bytes, out results);
            }
            finally
            {
                if (service != null)
                    service.Dispose();
            }
            return results;
        }
    }
}
