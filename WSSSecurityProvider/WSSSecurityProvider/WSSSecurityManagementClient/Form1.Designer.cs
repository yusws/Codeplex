namespace MyLocalBroadband.WSSSecurityManagement.Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CentralAdminURL_lbl = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.DNS_tab = new System.Windows.Forms.TabPage();
            this.DNSIP_lbl = new System.Windows.Forms.Label();
            this.DNSResults_rtf = new System.Windows.Forms.RichTextBox();
            this.CreateDNS_btn = new System.Windows.Forms.Button();
            this.TestDNS_btn = new System.Windows.Forms.Button();
            this.DNSURL_lbl = new System.Windows.Forms.Label();
            this.NewSite_tab = new System.Windows.Forms.TabPage();
            this.NewSiteLCID_lbl = new System.Windows.Forms.Label();
            this.NewSiteWebApplicationURL_lbl = new System.Windows.Forms.Label();
            this.NewSiteMembershipApplicationName_lbl = new System.Windows.Forms.Label();
            this.NewSiteMembershipProviderName_lbl = new System.Windows.Forms.Label();
            this.NewSiteOwnerEmail_lbl = new System.Windows.Forms.Label();
            this.NewSitePasswordAnswer_lbl = new System.Windows.Forms.Label();
            this.NewSitePassswordQuestion_lbl = new System.Windows.Forms.Label();
            this.NewSiteOwnerPassword_lbl = new System.Windows.Forms.Label();
            this.NewSiteOwnerLogin_lbl = new System.Windows.Forms.Label();
            this.NewSiteOwnerLastName_lbl = new System.Windows.Forms.Label();
            this.NewSiteOwnerFirstName_lbl = new System.Windows.Forms.Label();
            this.NewSiteSiteTemplate_lbl = new System.Windows.Forms.Label();
            this.NewSiteSiteDescription_lbl = new System.Windows.Forms.Label();
            this.NewSiteSiteTitle_lbl = new System.Windows.Forms.Label();
            this.NewSiteSiteURL_lbl = new System.Windows.Forms.Label();
            this.ExistingUser_grp = new System.Windows.Forms.GroupBox();
            this.NewUser_grp = new System.Windows.Forms.GroupBox();
            this.DNSIP_box = new System.Windows.Forms.TextBox();
            this.DNSURL_box = new System.Windows.Forms.TextBox();
            this.NewSiteOwnerLogin_box = new System.Windows.Forms.TextBox();
            this.NewSitePasswordAnswer_box = new System.Windows.Forms.TextBox();
            this.NewSiteOwnerFirstName_box = new System.Windows.Forms.TextBox();
            this.NewSitePassswordQuestion_box = new System.Windows.Forms.TextBox();
            this.NewSiteOwnerPassword_box = new System.Windows.Forms.TextBox();
            this.NewSiteOwnerEmail_box = new System.Windows.Forms.TextBox();
            this.NewSiteOwnerLastName_box = new System.Windows.Forms.TextBox();
            this.NewSiteMembershipProviderName_box = new System.Windows.Forms.TextBox();
            this.NewSiteWebApplicationURL_box = new System.Windows.Forms.TextBox();
            this.NewSiteMembershipApplicationName_box = new System.Windows.Forms.TextBox();
            this.NewSiteLCID_box = new System.Windows.Forms.TextBox();
            this.NewSiteSiteTemplate_box = new System.Windows.Forms.TextBox();
            this.NewSiteSiteDescription_box = new System.Windows.Forms.TextBox();
            this.NewSiteSiteTitle_box = new System.Windows.Forms.TextBox();
            this.NewSiteSiteURL_box = new System.Windows.Forms.TextBox();
            this.CentralAdminURL_box = new System.Windows.Forms.TextBox();
            this.NewSiteNewUser_btn = new System.Windows.Forms.Button();
            this.NewSiteExistingUser_btn = new System.Windows.Forms.Button();
            this.NewSiteResults_rtf = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.DNS_tab.SuspendLayout();
            this.NewSite_tab.SuspendLayout();
            this.ExistingUser_grp.SuspendLayout();
            this.NewUser_grp.SuspendLayout();
            this.SuspendLayout();
            // 
            // CentralAdminURL_lbl
            // 
            this.CentralAdminURL_lbl.AutoSize = true;
            this.CentralAdminURL_lbl.Location = new System.Drawing.Point(12, 22);
            this.CentralAdminURL_lbl.Name = "CentralAdminURL_lbl";
            this.CentralAdminURL_lbl.Size = new System.Drawing.Size(97, 13);
            this.CentralAdminURL_lbl.TabIndex = 0;
            this.CentralAdminURL_lbl.Text = "Central Admin URL";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.DNS_tab);
            this.tabControl1.Controls.Add(this.NewSite_tab);
            this.tabControl1.Location = new System.Drawing.Point(2, 46);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(897, 389);
            this.tabControl1.TabIndex = 2;
            // 
            // DNS_tab
            // 
            this.DNS_tab.Controls.Add(this.DNSIP_box);
            this.DNS_tab.Controls.Add(this.DNSIP_lbl);
            this.DNS_tab.Controls.Add(this.DNSResults_rtf);
            this.DNS_tab.Controls.Add(this.CreateDNS_btn);
            this.DNS_tab.Controls.Add(this.TestDNS_btn);
            this.DNS_tab.Controls.Add(this.DNSURL_box);
            this.DNS_tab.Controls.Add(this.DNSURL_lbl);
            this.DNS_tab.Location = new System.Drawing.Point(4, 22);
            this.DNS_tab.Name = "DNS_tab";
            this.DNS_tab.Padding = new System.Windows.Forms.Padding(3);
            this.DNS_tab.Size = new System.Drawing.Size(889, 363);
            this.DNS_tab.TabIndex = 0;
            this.DNS_tab.Text = "DNS";
            this.DNS_tab.UseVisualStyleBackColor = true;
            // 
            // DNSIP_lbl
            // 
            this.DNSIP_lbl.AutoSize = true;
            this.DNSIP_lbl.Location = new System.Drawing.Point(32, 44);
            this.DNSIP_lbl.Name = "DNSIP_lbl";
            this.DNSIP_lbl.Size = new System.Drawing.Size(17, 13);
            this.DNSIP_lbl.TabIndex = 5;
            this.DNSIP_lbl.Text = "IP";
            // 
            // DNSResults_rtf
            // 
            this.DNSResults_rtf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DNSResults_rtf.Location = new System.Drawing.Point(6, 115);
            this.DNSResults_rtf.Name = "DNSResults_rtf";
            this.DNSResults_rtf.Size = new System.Drawing.Size(877, 242);
            this.DNSResults_rtf.TabIndex = 4;
            this.DNSResults_rtf.Text = "results";
            // 
            // CreateDNS_btn
            // 
            this.CreateDNS_btn.Location = new System.Drawing.Point(293, 73);
            this.CreateDNS_btn.Name = "CreateDNS_btn";
            this.CreateDNS_btn.Size = new System.Drawing.Size(75, 23);
            this.CreateDNS_btn.TabIndex = 3;
            this.CreateDNS_btn.Text = "Create DNS";
            this.CreateDNS_btn.UseVisualStyleBackColor = true;
            this.CreateDNS_btn.Click += new System.EventHandler(this.CreateDNS_btn_Click);
            // 
            // TestDNS_btn
            // 
            this.TestDNS_btn.Location = new System.Drawing.Point(107, 73);
            this.TestDNS_btn.Name = "TestDNS_btn";
            this.TestDNS_btn.Size = new System.Drawing.Size(75, 23);
            this.TestDNS_btn.TabIndex = 2;
            this.TestDNS_btn.Text = "Test DNS";
            this.TestDNS_btn.UseVisualStyleBackColor = true;
            this.TestDNS_btn.Click += new System.EventHandler(this.TestDNS_btn_Click);
            // 
            // DNSURL_lbl
            // 
            this.DNSURL_lbl.AutoSize = true;
            this.DNSURL_lbl.Location = new System.Drawing.Point(20, 18);
            this.DNSURL_lbl.Name = "DNSURL_lbl";
            this.DNSURL_lbl.Size = new System.Drawing.Size(29, 13);
            this.DNSURL_lbl.TabIndex = 0;
            this.DNSURL_lbl.Text = "URL";
            // 
            // NewSite_tab
            // 
            this.NewSite_tab.Controls.Add(this.NewSiteResults_rtf);
            this.NewSite_tab.Controls.Add(this.NewSiteExistingUser_btn);
            this.NewSite_tab.Controls.Add(this.NewSiteNewUser_btn);
            this.NewSite_tab.Controls.Add(this.NewUser_grp);
            this.NewSite_tab.Controls.Add(this.NewSiteMembershipProviderName_box);
            this.NewSite_tab.Controls.Add(this.NewSiteWebApplicationURL_box);
            this.NewSite_tab.Controls.Add(this.NewSiteMembershipApplicationName_box);
            this.NewSite_tab.Controls.Add(this.NewSiteLCID_box);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteTemplate_box);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteDescription_box);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteTitle_box);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteURL_box);
            this.NewSite_tab.Controls.Add(this.NewSiteLCID_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteWebApplicationURL_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteMembershipApplicationName_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteMembershipProviderName_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteTemplate_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteDescription_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteTitle_lbl);
            this.NewSite_tab.Controls.Add(this.NewSiteSiteURL_lbl);
            this.NewSite_tab.Location = new System.Drawing.Point(4, 22);
            this.NewSite_tab.Name = "NewSite_tab";
            this.NewSite_tab.Padding = new System.Windows.Forms.Padding(3);
            this.NewSite_tab.Size = new System.Drawing.Size(889, 363);
            this.NewSite_tab.TabIndex = 1;
            this.NewSite_tab.Text = "New Site";
            this.NewSite_tab.UseVisualStyleBackColor = true;
            // 
            // NewSiteLCID_lbl
            // 
            this.NewSiteLCID_lbl.AutoSize = true;
            this.NewSiteLCID_lbl.Location = new System.Drawing.Point(122, 125);
            this.NewSiteLCID_lbl.Name = "NewSiteLCID_lbl";
            this.NewSiteLCID_lbl.Size = new System.Drawing.Size(31, 13);
            this.NewSiteLCID_lbl.TabIndex = 14;
            this.NewSiteLCID_lbl.Text = "LCID";
            // 
            // NewSiteWebApplicationURL_lbl
            // 
            this.NewSiteWebApplicationURL_lbl.AutoSize = true;
            this.NewSiteWebApplicationURL_lbl.Location = new System.Drawing.Point(43, 183);
            this.NewSiteWebApplicationURL_lbl.Name = "NewSiteWebApplicationURL_lbl";
            this.NewSiteWebApplicationURL_lbl.Size = new System.Drawing.Size(110, 13);
            this.NewSiteWebApplicationURL_lbl.TabIndex = 13;
            this.NewSiteWebApplicationURL_lbl.Text = "Web Application URL";
            // 
            // NewSiteMembershipApplicationName_lbl
            // 
            this.NewSiteMembershipApplicationName_lbl.AutoSize = true;
            this.NewSiteMembershipApplicationName_lbl.Location = new System.Drawing.Point(3, 154);
            this.NewSiteMembershipApplicationName_lbl.Name = "NewSiteMembershipApplicationName_lbl";
            this.NewSiteMembershipApplicationName_lbl.Size = new System.Drawing.Size(150, 13);
            this.NewSiteMembershipApplicationName_lbl.TabIndex = 12;
            this.NewSiteMembershipApplicationName_lbl.Text = "Membership Application Name";
            // 
            // NewSiteMembershipProviderName_lbl
            // 
            this.NewSiteMembershipProviderName_lbl.AutoSize = true;
            this.NewSiteMembershipProviderName_lbl.Location = new System.Drawing.Point(16, 212);
            this.NewSiteMembershipProviderName_lbl.Name = "NewSiteMembershipProviderName_lbl";
            this.NewSiteMembershipProviderName_lbl.Size = new System.Drawing.Size(137, 13);
            this.NewSiteMembershipProviderName_lbl.TabIndex = 11;
            this.NewSiteMembershipProviderName_lbl.Text = "Memberhsip Provider Name";
            // 
            // NewSiteOwnerEmail_lbl
            // 
            this.NewSiteOwnerEmail_lbl.AutoSize = true;
            this.NewSiteOwnerEmail_lbl.Location = new System.Drawing.Point(75, 129);
            this.NewSiteOwnerEmail_lbl.Name = "NewSiteOwnerEmail_lbl";
            this.NewSiteOwnerEmail_lbl.Size = new System.Drawing.Size(66, 13);
            this.NewSiteOwnerEmail_lbl.TabIndex = 10;
            this.NewSiteOwnerEmail_lbl.Text = "Owner Email";
            // 
            // NewSitePasswordAnswer_lbl
            // 
            this.NewSitePasswordAnswer_lbl.AutoSize = true;
            this.NewSitePasswordAnswer_lbl.Location = new System.Drawing.Point(50, 216);
            this.NewSitePasswordAnswer_lbl.Name = "NewSitePasswordAnswer_lbl";
            this.NewSitePasswordAnswer_lbl.Size = new System.Drawing.Size(91, 13);
            this.NewSitePasswordAnswer_lbl.TabIndex = 9;
            this.NewSitePasswordAnswer_lbl.Text = "Password Answer";
            // 
            // NewSitePassswordQuestion_lbl
            // 
            this.NewSitePassswordQuestion_lbl.AutoSize = true;
            this.NewSitePassswordQuestion_lbl.Location = new System.Drawing.Point(43, 187);
            this.NewSitePassswordQuestion_lbl.Name = "NewSitePassswordQuestion_lbl";
            this.NewSitePassswordQuestion_lbl.Size = new System.Drawing.Size(98, 13);
            this.NewSitePassswordQuestion_lbl.TabIndex = 8;
            this.NewSitePassswordQuestion_lbl.Text = "Password Question";
            // 
            // NewSiteOwnerPassword_lbl
            // 
            this.NewSiteOwnerPassword_lbl.AutoSize = true;
            this.NewSiteOwnerPassword_lbl.Location = new System.Drawing.Point(54, 158);
            this.NewSiteOwnerPassword_lbl.Name = "NewSiteOwnerPassword_lbl";
            this.NewSiteOwnerPassword_lbl.Size = new System.Drawing.Size(87, 13);
            this.NewSiteOwnerPassword_lbl.TabIndex = 7;
            this.NewSiteOwnerPassword_lbl.Text = "Owner Password";
            // 
            // NewSiteOwnerLogin_lbl
            // 
            this.NewSiteOwnerLogin_lbl.AutoSize = true;
            this.NewSiteOwnerLogin_lbl.Location = new System.Drawing.Point(6, 24);
            this.NewSiteOwnerLogin_lbl.Name = "NewSiteOwnerLogin_lbl";
            this.NewSiteOwnerLogin_lbl.Size = new System.Drawing.Size(67, 13);
            this.NewSiteOwnerLogin_lbl.TabIndex = 6;
            this.NewSiteOwnerLogin_lbl.Text = "Owner Login";
            // 
            // NewSiteOwnerLastName_lbl
            // 
            this.NewSiteOwnerLastName_lbl.AutoSize = true;
            this.NewSiteOwnerLastName_lbl.Location = new System.Drawing.Point(49, 100);
            this.NewSiteOwnerLastName_lbl.Name = "NewSiteOwnerLastName_lbl";
            this.NewSiteOwnerLastName_lbl.Size = new System.Drawing.Size(92, 13);
            this.NewSiteOwnerLastName_lbl.TabIndex = 5;
            this.NewSiteOwnerLastName_lbl.Text = "Owner Last Name";
            // 
            // NewSiteOwnerFirstName_lbl
            // 
            this.NewSiteOwnerFirstName_lbl.AutoSize = true;
            this.NewSiteOwnerFirstName_lbl.Location = new System.Drawing.Point(50, 71);
            this.NewSiteOwnerFirstName_lbl.Name = "NewSiteOwnerFirstName_lbl";
            this.NewSiteOwnerFirstName_lbl.Size = new System.Drawing.Size(91, 13);
            this.NewSiteOwnerFirstName_lbl.TabIndex = 4;
            this.NewSiteOwnerFirstName_lbl.Text = "Owner First Name";
            // 
            // NewSiteSiteTemplate_lbl
            // 
            this.NewSiteSiteTemplate_lbl.AutoSize = true;
            this.NewSiteSiteTemplate_lbl.Location = new System.Drawing.Point(81, 96);
            this.NewSiteSiteTemplate_lbl.Name = "NewSiteSiteTemplate_lbl";
            this.NewSiteSiteTemplate_lbl.Size = new System.Drawing.Size(72, 13);
            this.NewSiteSiteTemplate_lbl.TabIndex = 3;
            this.NewSiteSiteTemplate_lbl.Text = "Site Template";
            // 
            // NewSiteSiteDescription_lbl
            // 
            this.NewSiteSiteDescription_lbl.AutoSize = true;
            this.NewSiteSiteDescription_lbl.Location = new System.Drawing.Point(72, 67);
            this.NewSiteSiteDescription_lbl.Name = "NewSiteSiteDescription_lbl";
            this.NewSiteSiteDescription_lbl.Size = new System.Drawing.Size(81, 13);
            this.NewSiteSiteDescription_lbl.TabIndex = 2;
            this.NewSiteSiteDescription_lbl.Text = "Site Description";
            // 
            // NewSiteSiteTitle_lbl
            // 
            this.NewSiteSiteTitle_lbl.AutoSize = true;
            this.NewSiteSiteTitle_lbl.Location = new System.Drawing.Point(105, 38);
            this.NewSiteSiteTitle_lbl.Name = "NewSiteSiteTitle_lbl";
            this.NewSiteSiteTitle_lbl.Size = new System.Drawing.Size(48, 13);
            this.NewSiteSiteTitle_lbl.TabIndex = 1;
            this.NewSiteSiteTitle_lbl.Text = "Site Title";
            // 
            // NewSiteSiteURL_lbl
            // 
            this.NewSiteSiteURL_lbl.AutoSize = true;
            this.NewSiteSiteURL_lbl.Location = new System.Drawing.Point(103, 9);
            this.NewSiteSiteURL_lbl.Name = "NewSiteSiteURL_lbl";
            this.NewSiteSiteURL_lbl.Size = new System.Drawing.Size(50, 13);
            this.NewSiteSiteURL_lbl.TabIndex = 0;
            this.NewSiteSiteURL_lbl.Text = "Site URL";
            // 
            // ExistingUser_grp
            // 
            this.ExistingUser_grp.Controls.Add(this.NewSiteOwnerLogin_box);
            this.ExistingUser_grp.Controls.Add(this.NewSiteOwnerLogin_lbl);
            this.ExistingUser_grp.Location = new System.Drawing.Point(79, 9);
            this.ExistingUser_grp.Name = "ExistingUser_grp";
            this.ExistingUser_grp.Size = new System.Drawing.Size(367, 53);
            this.ExistingUser_grp.TabIndex = 30;
            this.ExistingUser_grp.TabStop = false;
            this.ExistingUser_grp.Text = "Existing User";
            // 
            // NewUser_grp
            // 
            this.NewUser_grp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewUser_grp.Controls.Add(this.ExistingUser_grp);
            this.NewUser_grp.Controls.Add(this.NewSitePasswordAnswer_box);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerFirstName_box);
            this.NewUser_grp.Controls.Add(this.NewSitePassswordQuestion_box);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerFirstName_lbl);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerPassword_box);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerLastName_lbl);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerEmail_box);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerPassword_lbl);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerLastName_box);
            this.NewUser_grp.Controls.Add(this.NewSitePassswordQuestion_lbl);
            this.NewUser_grp.Controls.Add(this.NewSitePasswordAnswer_lbl);
            this.NewUser_grp.Controls.Add(this.NewSiteOwnerEmail_lbl);
            this.NewUser_grp.Location = new System.Drawing.Point(452, 6);
            this.NewUser_grp.Name = "NewUser_grp";
            this.NewUser_grp.Size = new System.Drawing.Size(457, 246);
            this.NewUser_grp.TabIndex = 31;
            this.NewUser_grp.TabStop = false;
            this.NewUser_grp.Text = "New User";
            // 
            // DNSIP_box
            // 
            this.DNSIP_box.Location = new System.Drawing.Point(55, 41);
            this.DNSIP_box.Name = "DNSIP_box";
            this.DNSIP_box.Size = new System.Drawing.Size(417, 20);
            this.DNSIP_box.TabIndex = 6;
            this.DNSIP_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.DNSIP;
            // 
            // DNSURL_box
            // 
            this.DNSURL_box.Location = new System.Drawing.Point(55, 15);
            this.DNSURL_box.Name = "DNSURL_box";
            this.DNSURL_box.Size = new System.Drawing.Size(417, 20);
            this.DNSURL_box.TabIndex = 1;
            this.DNSURL_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.DNSURL;
            // 
            // NewSiteOwnerLogin_box
            // 
            this.NewSiteOwnerLogin_box.Location = new System.Drawing.Point(75, 21);
            this.NewSiteOwnerLogin_box.Name = "NewSiteOwnerLogin_box";
            this.NewSiteOwnerLogin_box.Size = new System.Drawing.Size(282, 20);
            this.NewSiteOwnerLogin_box.TabIndex = 23;
            this.NewSiteOwnerLogin_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_OwnerLogin;
            // 
            // NewSitePasswordAnswer_box
            // 
            this.NewSitePasswordAnswer_box.Location = new System.Drawing.Point(154, 213);
            this.NewSitePasswordAnswer_box.Name = "NewSitePasswordAnswer_box";
            this.NewSitePasswordAnswer_box.Size = new System.Drawing.Size(282, 20);
            this.NewSitePasswordAnswer_box.TabIndex = 29;
            this.NewSitePasswordAnswer_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_PasswordAnswer;
            // 
            // NewSiteOwnerFirstName_box
            // 
            this.NewSiteOwnerFirstName_box.Location = new System.Drawing.Point(154, 68);
            this.NewSiteOwnerFirstName_box.Name = "NewSiteOwnerFirstName_box";
            this.NewSiteOwnerFirstName_box.Size = new System.Drawing.Size(282, 20);
            this.NewSiteOwnerFirstName_box.TabIndex = 24;
            this.NewSiteOwnerFirstName_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_OwnerFirstName;
            // 
            // NewSitePassswordQuestion_box
            // 
            this.NewSitePassswordQuestion_box.Location = new System.Drawing.Point(154, 184);
            this.NewSitePassswordQuestion_box.Name = "NewSitePassswordQuestion_box";
            this.NewSitePassswordQuestion_box.Size = new System.Drawing.Size(282, 20);
            this.NewSitePassswordQuestion_box.TabIndex = 28;
            this.NewSitePassswordQuestion_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_PasswordQuestion;
            // 
            // NewSiteOwnerPassword_box
            // 
            this.NewSiteOwnerPassword_box.Location = new System.Drawing.Point(154, 155);
            this.NewSiteOwnerPassword_box.Name = "NewSiteOwnerPassword_box";
            this.NewSiteOwnerPassword_box.Size = new System.Drawing.Size(282, 20);
            this.NewSiteOwnerPassword_box.TabIndex = 27;
            this.NewSiteOwnerPassword_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_OwnerPassword;
            this.NewSiteOwnerPassword_box.UseSystemPasswordChar = true;
            // 
            // NewSiteOwnerEmail_box
            // 
            this.NewSiteOwnerEmail_box.Location = new System.Drawing.Point(154, 126);
            this.NewSiteOwnerEmail_box.Name = "NewSiteOwnerEmail_box";
            this.NewSiteOwnerEmail_box.Size = new System.Drawing.Size(282, 20);
            this.NewSiteOwnerEmail_box.TabIndex = 26;
            this.NewSiteOwnerEmail_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_OwnerEmail;
            // 
            // NewSiteOwnerLastName_box
            // 
            this.NewSiteOwnerLastName_box.Location = new System.Drawing.Point(154, 97);
            this.NewSiteOwnerLastName_box.Name = "NewSiteOwnerLastName_box";
            this.NewSiteOwnerLastName_box.Size = new System.Drawing.Size(282, 20);
            this.NewSiteOwnerLastName_box.TabIndex = 25;
            this.NewSiteOwnerLastName_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_OwnerLastName;
            // 
            // NewSiteMembershipProviderName_box
            // 
            this.NewSiteMembershipProviderName_box.Location = new System.Drawing.Point(166, 209);
            this.NewSiteMembershipProviderName_box.Name = "NewSiteMembershipProviderName_box";
            this.NewSiteMembershipProviderName_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteMembershipProviderName_box.TabIndex = 22;
            this.NewSiteMembershipProviderName_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_MembershipProviderName;
            // 
            // NewSiteWebApplicationURL_box
            // 
            this.NewSiteWebApplicationURL_box.Location = new System.Drawing.Point(166, 180);
            this.NewSiteWebApplicationURL_box.Name = "NewSiteWebApplicationURL_box";
            this.NewSiteWebApplicationURL_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteWebApplicationURL_box.TabIndex = 21;
            this.NewSiteWebApplicationURL_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_WebApplicationURL;
            // 
            // NewSiteMembershipApplicationName_box
            // 
            this.NewSiteMembershipApplicationName_box.Location = new System.Drawing.Point(166, 151);
            this.NewSiteMembershipApplicationName_box.Name = "NewSiteMembershipApplicationName_box";
            this.NewSiteMembershipApplicationName_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteMembershipApplicationName_box.TabIndex = 20;
            this.NewSiteMembershipApplicationName_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_WebApplicationName;
            // 
            // NewSiteLCID_box
            // 
            this.NewSiteLCID_box.Location = new System.Drawing.Point(166, 122);
            this.NewSiteLCID_box.Name = "NewSiteLCID_box";
            this.NewSiteLCID_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteLCID_box.TabIndex = 19;
            this.NewSiteLCID_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_LCID;
            // 
            // NewSiteSiteTemplate_box
            // 
            this.NewSiteSiteTemplate_box.Location = new System.Drawing.Point(166, 93);
            this.NewSiteSiteTemplate_box.Name = "NewSiteSiteTemplate_box";
            this.NewSiteSiteTemplate_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteSiteTemplate_box.TabIndex = 18;
            this.NewSiteSiteTemplate_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_SiteTemplate;
            // 
            // NewSiteSiteDescription_box
            // 
            this.NewSiteSiteDescription_box.Location = new System.Drawing.Point(166, 64);
            this.NewSiteSiteDescription_box.Name = "NewSiteSiteDescription_box";
            this.NewSiteSiteDescription_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteSiteDescription_box.TabIndex = 17;
            this.NewSiteSiteDescription_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_SiteDescription;
            // 
            // NewSiteSiteTitle_box
            // 
            this.NewSiteSiteTitle_box.Location = new System.Drawing.Point(166, 35);
            this.NewSiteSiteTitle_box.Name = "NewSiteSiteTitle_box";
            this.NewSiteSiteTitle_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteSiteTitle_box.TabIndex = 16;
            this.NewSiteSiteTitle_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_SiteTitle;
            // 
            // NewSiteSiteURL_box
            // 
            this.NewSiteSiteURL_box.Location = new System.Drawing.Point(166, 6);
            this.NewSiteSiteURL_box.Name = "NewSiteSiteURL_box";
            this.NewSiteSiteURL_box.Size = new System.Drawing.Size(280, 20);
            this.NewSiteSiteURL_box.TabIndex = 15;
            this.NewSiteSiteURL_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.NewSite_SiteURL;
            // 
            // CentralAdminURL_box
            // 
            this.CentralAdminURL_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CentralAdminURL_box.Location = new System.Drawing.Point(113, 19);
            this.CentralAdminURL_box.Name = "CentralAdminURL_box";
            this.CentralAdminURL_box.Size = new System.Drawing.Size(737, 20);
            this.CentralAdminURL_box.TabIndex = 1;
            this.CentralAdminURL_box.Text = global::MyLocalBroadband.WSSSecurityManagement.Client.Properties.Settings.Default.CentralAdminURL;
            // 
            // NewSiteNewUser_btn
            // 
            this.NewSiteNewUser_btn.Location = new System.Drawing.Point(46, 244);
            this.NewSiteNewUser_btn.Name = "NewSiteNewUser_btn";
            this.NewSiteNewUser_btn.Size = new System.Drawing.Size(113, 23);
            this.NewSiteNewUser_btn.TabIndex = 32;
            this.NewSiteNewUser_btn.Text = "New Site/New User";
            this.NewSiteNewUser_btn.UseVisualStyleBackColor = true;
            this.NewSiteNewUser_btn.Click += new System.EventHandler(this.NewSiteNewUser_btn_Click);
            // 
            // NewSiteExistingUser_btn
            // 
            this.NewSiteExistingUser_btn.Location = new System.Drawing.Point(272, 244);
            this.NewSiteExistingUser_btn.Name = "NewSiteExistingUser_btn";
            this.NewSiteExistingUser_btn.Size = new System.Drawing.Size(128, 23);
            this.NewSiteExistingUser_btn.TabIndex = 33;
            this.NewSiteExistingUser_btn.Text = "New Site/Existing User";
            this.NewSiteExistingUser_btn.UseVisualStyleBackColor = true;
            this.NewSiteExistingUser_btn.Click += new System.EventHandler(this.NewSiteExistingUser_btn_Click);
            // 
            // NewSiteResults_rtf
            // 
            this.NewSiteResults_rtf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NewSiteResults_rtf.Location = new System.Drawing.Point(6, 282);
            this.NewSiteResults_rtf.Name = "NewSiteResults_rtf";
            this.NewSiteResults_rtf.Size = new System.Drawing.Size(877, 75);
            this.NewSiteResults_rtf.TabIndex = 34;
            this.NewSiteResults_rtf.Text = "results";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 439);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.CentralAdminURL_box);
            this.Controls.Add(this.CentralAdminURL_lbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "My Local Broadband - WSS Security Provider Manager";
            this.tabControl1.ResumeLayout(false);
            this.DNS_tab.ResumeLayout(false);
            this.DNS_tab.PerformLayout();
            this.NewSite_tab.ResumeLayout(false);
            this.NewSite_tab.PerformLayout();
            this.ExistingUser_grp.ResumeLayout(false);
            this.ExistingUser_grp.PerformLayout();
            this.NewUser_grp.ResumeLayout(false);
            this.NewUser_grp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CentralAdminURL_lbl;
        private System.Windows.Forms.TextBox CentralAdminURL_box;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DNS_tab;
        private System.Windows.Forms.TextBox DNSURL_box;
        private System.Windows.Forms.Label DNSURL_lbl;
        private System.Windows.Forms.TabPage NewSite_tab;
        private System.Windows.Forms.Button CreateDNS_btn;
        private System.Windows.Forms.Button TestDNS_btn;
        private System.Windows.Forms.RichTextBox DNSResults_rtf;
        private System.Windows.Forms.TextBox DNSIP_box;
        private System.Windows.Forms.Label DNSIP_lbl;
        private System.Windows.Forms.Label NewSiteLCID_lbl;
        private System.Windows.Forms.Label NewSiteWebApplicationURL_lbl;
        private System.Windows.Forms.Label NewSiteMembershipApplicationName_lbl;
        private System.Windows.Forms.Label NewSiteMembershipProviderName_lbl;
        private System.Windows.Forms.Label NewSiteOwnerEmail_lbl;
        private System.Windows.Forms.Label NewSitePasswordAnswer_lbl;
        private System.Windows.Forms.Label NewSitePassswordQuestion_lbl;
        private System.Windows.Forms.Label NewSiteOwnerPassword_lbl;
        private System.Windows.Forms.Label NewSiteOwnerLogin_lbl;
        private System.Windows.Forms.Label NewSiteOwnerLastName_lbl;
        private System.Windows.Forms.Label NewSiteOwnerFirstName_lbl;
        private System.Windows.Forms.Label NewSiteSiteTemplate_lbl;
        private System.Windows.Forms.Label NewSiteSiteDescription_lbl;
        private System.Windows.Forms.Label NewSiteSiteTitle_lbl;
        private System.Windows.Forms.Label NewSiteSiteURL_lbl;
        private System.Windows.Forms.TextBox NewSiteOwnerFirstName_box;
        private System.Windows.Forms.TextBox NewSiteOwnerLogin_box;
        private System.Windows.Forms.TextBox NewSiteMembershipProviderName_box;
        private System.Windows.Forms.TextBox NewSiteWebApplicationURL_box;
        private System.Windows.Forms.TextBox NewSiteMembershipApplicationName_box;
        private System.Windows.Forms.TextBox NewSiteLCID_box;
        private System.Windows.Forms.TextBox NewSiteSiteTemplate_box;
        private System.Windows.Forms.TextBox NewSiteSiteDescription_box;
        private System.Windows.Forms.TextBox NewSiteSiteTitle_box;
        private System.Windows.Forms.TextBox NewSiteSiteURL_box;
        private System.Windows.Forms.TextBox NewSitePasswordAnswer_box;
        private System.Windows.Forms.TextBox NewSitePassswordQuestion_box;
        private System.Windows.Forms.TextBox NewSiteOwnerPassword_box;
        private System.Windows.Forms.TextBox NewSiteOwnerEmail_box;
        private System.Windows.Forms.TextBox NewSiteOwnerLastName_box;
        private System.Windows.Forms.GroupBox ExistingUser_grp;
        private System.Windows.Forms.GroupBox NewUser_grp;
        private System.Windows.Forms.Button NewSiteExistingUser_btn;
        private System.Windows.Forms.Button NewSiteNewUser_btn;
        private System.Windows.Forms.RichTextBox NewSiteResults_rtf;
    }
}

