using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace MyLocalBroadband.WSSPictureUploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void ChoosePictures_btn_Click(object sender, EventArgs e)
        {
            // Display the openFile dialog.
            DialogResult result = openFileDialog1.ShowDialog();

            // OK button was pressed.
            if (result == DialogResult.OK)
            {
                Pictures_rtf.Lines = openFileDialog1.FileNames;
            }

        }

        private void Upload_btn_Click(object sender, EventArgs e)
        {
            //WINDOWS AUTH todo: remove this call for windows auth sites.
            System.Net.CookieContainer cookieJar = Authenticate(Site_box.Text.Trim(), UserName_box.Text.Trim(), Password_box.Text.Trim());
            
            Imaging_WS.Imaging imagingService = null;
            try
            {
                imagingService = new Imaging_WS.Imaging();
                imagingService.Url = Site_box.Text.Trim() + "/_vti_bin/Imaging.asmx";
                //WINDOWS AUTH TODO: remove cookie and replace with setting credential
                imagingService.CookieContainer = cookieJar;

                string library = Library_box.Text.Trim();
                string folder = Folder_box.Text.Trim();
                if (CreateFolders_chk.Checked)
                {
                    //create folder
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml("<Request><files><file filename=\"new folder\" newbasename=\"" + folder + "\"/></files></Request>");
                    XmlElement request = xmlDoc.CreateElement("Req");
                    request.AppendChild(xmlDoc["Request"]);

                    imagingService.CreateNewFolder(library, "");
                    //<results xmlns="http://schemas.microsoft.com/sharepoint/soap/ois/"><result name="new folder" renamed="true" newbasename="test3" /></results>
                    string ignore = imagingService.Rename(library, "", request).OuterXml;
                }
                
                //upload files
                StringBuilder success = new StringBuilder("The following uploads were successful:\n");
                StringBuilder failed = new StringBuilder("These following uploads failed:\n");
                bool containedErrors = false;
                foreach (string line in Pictures_rtf.Lines)
                {
                    try
                    {
                        imagingService.Upload(library, folder, File.ReadAllBytes(line), line.Substring(line.LastIndexOf("\\") + 1), true);
                        success.Append("x " + line + "\n");
                    }
                    catch
                    {
                        containedErrors = true;
                        failed.Append(line + "\n");
                    }
                }
                if (containedErrors)
                {
                    MessageBox.Show(this, "The upload experienced errors.\n  You may retry the failed uploads by first deleting everything\nbut the failed list in the file selection window.\n", "Contained Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Pictures_rtf.Text = failed.ToString() + "\n\n\n" + success.ToString();
                }
                else
                {
                    Pictures_rtf.Text = success.ToString();
                }
                this.Refresh();
            }
            finally
            {
                if (imagingService != null)
                    imagingService.Dispose();
            }

        }

        //create a cookie for forms based authenticated sites.
        private CookieContainer Authenticate(string siteURL, string user, string password)
        {
            CookieContainer cookieJar = new CookieContainer();
            Authentication_WS.Authentication service = null;
            try
            {
                service = new Authentication_WS.Authentication();
                service.Url = siteURL + "/_vti_bin/Authentication.asmx";
                service.CookieContainer = cookieJar;
                Authentication_WS.LoginResult result = service.Login(user, password);
            }
            finally
            {
                if (service != null)
                    service.Dispose();
            }
            return cookieJar;
        }
    }
}