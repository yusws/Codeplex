using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyLocalBroadband.WSSSecurityManagement.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TestDNS_btn_Click(object sender, EventArgs e)
        {
            HostingTools_WS.HostingTools hostingToolsService = null;
            try
            {
                DNSResults_rtf.Text = string.Empty;
                hostingToolsService = new HostingTools_WS.HostingTools();
                hostingToolsService.Url = CentralAdminURL_box.Text + "/_vti_adm/HostingToolsSvc.asmx";
                bool dnsExists = hostingToolsService.DNSExists(DNSURL_box.Text);
                if (dnsExists)
                    DNSResults_rtf.Text = "Yes, entry, " + DNSURL_box.Text + ", does exist.";
                else
                    DNSResults_rtf.Text = "No, entry, " + DNSURL_box.Text + ", does NOT exist.";
            }
            catch (Exception ex)
            {
                DNSResults_rtf.Text = "Error:\n" + ex.ToString();
            }
            finally
            {
                if (hostingToolsService != null)
                    hostingToolsService.Dispose();
            }
        }

        private void CreateDNS_btn_Click(object sender, EventArgs e)
        {
            HostingTools_WS.HostingTools hostingToolsService = null;
            try
            {
                DNSResults_rtf.Text = string.Empty;
                hostingToolsService = new HostingTools_WS.HostingTools();
                hostingToolsService.Url = CentralAdminURL_box.Text + "/_vti_adm/HostingToolsSvc.asmx";
                
                hostingToolsService.CreateDNS(DNSURL_box.Text, DNSIP_box.Text);
                DNSResults_rtf.Text = "Entry Created";
            }
            catch (Exception ex)
            {
                DNSResults_rtf.Text = "Error:\n" + ex.ToString();
            }
            finally
            {
                if (hostingToolsService != null)
                    hostingToolsService.Dispose();
            }
        }

        private void NewSiteNewUser_btn_Click(object sender, EventArgs e)
        {
            HostingTools_WS.HostingTools hostingToolsService = null;
            try
            {
                NewSiteResults_rtf.Text = string.Empty;
                hostingToolsService = new HostingTools_WS.HostingTools();
                hostingToolsService.Url = CentralAdminURL_box.Text + "/_vti_adm/HostingToolsSvc.asmx";

                string url = NewSiteSiteURL_box.Text.Trim();
                string title = NewSiteSiteTitle_box.Text.Trim();
                string description = NewSiteSiteDescription_box.Text.Trim();
                string template = NewSiteSiteTemplate_box.Text.Trim();
                string firstName = NewSiteOwnerFirstName_box.Text.Trim();
                string lastName = NewSiteOwnerLastName_box.Text.Trim();
                string login = NewSiteOwnerLogin_box.Text.Trim();
                string password = NewSiteOwnerPassword_box.Text.Trim();
                string question = NewSitePassswordQuestion_box.Text.Trim();
                string answer = NewSitePasswordAnswer_box.Text.Trim();
                string email = NewSiteOwnerEmail_box.Text.Trim();
                string appName = NewSiteMembershipApplicationName_box.Text.Trim();
                string webAppUrl = NewSiteWebApplicationURL_box.Text.Trim();
                string provider = NewSiteMembershipProviderName_box.Text.Trim();
                uint lcid = Convert.ToUInt32(NewSiteLCID_box.Text.Trim());

                hostingToolsService.CreateNewSiteNewSQLOwner(url, title, description, template, firstName, lastName, login, password, question, answer, email, appName, webAppUrl, provider, lcid);
                NewSiteResults_rtf.Text = "Site and User Created";
            }
            catch (Exception ex)
            {
                NewSiteResults_rtf.Text = "Error:\n" + ex.ToString();
            }
            finally
            {
                if (hostingToolsService != null)
                    hostingToolsService.Dispose();
            }
        }

        private void NewSiteExistingUser_btn_Click(object sender, EventArgs e)
        {
            HostingTools_WS.HostingTools hostingToolsService = null;
            try
            {
                NewSiteResults_rtf.Text = string.Empty;
                hostingToolsService = new HostingTools_WS.HostingTools();
                hostingToolsService.Url = CentralAdminURL_box.Text + "/_vti_adm/HostingToolsSvc.asmx";

                string url = NewSiteSiteURL_box.Text.Trim();
                string title = NewSiteSiteTitle_box.Text.Trim();
                string description = NewSiteSiteDescription_box.Text.Trim();
                string template = NewSiteSiteTemplate_box.Text.Trim();
                string login = NewSiteOwnerLogin_box.Text.Trim();
                string appName = NewSiteMembershipApplicationName_box.Text.Trim();
                string webAppUrl = NewSiteWebApplicationURL_box.Text.Trim();
                string provider = NewSiteMembershipProviderName_box.Text.Trim();
                uint lcid = Convert.ToUInt32(NewSiteLCID_box.Text.Trim());

                hostingToolsService.CreateNewSiteExistingSQLOwner(url, title, description, template, login, appName, webAppUrl, provider, lcid);
                NewSiteResults_rtf.Text = "Site and User Created";
            }
            catch (Exception ex)
            {
                NewSiteResults_rtf.Text = "Error:\n" + ex.ToString();
            }
            finally
            {
                if (hostingToolsService != null)
                    hostingToolsService.Dispose();
            }
        }
    }
}
