namespace MyLocalBroadband.WSSPictureUploader
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
            this.UserName_lbl = new System.Windows.Forms.Label();
            this.Password_lbl = new System.Windows.Forms.Label();
            this.UserName_box = new System.Windows.Forms.TextBox();
            this.Password_box = new System.Windows.Forms.TextBox();
            this.Site_lbl = new System.Windows.Forms.Label();
            this.Library_lbl = new System.Windows.Forms.Label();
            this.Site_box = new System.Windows.Forms.TextBox();
            this.Library_box = new System.Windows.Forms.TextBox();
            this.Folder_lbl = new System.Windows.Forms.Label();
            this.Folder_box = new System.Windows.Forms.TextBox();
            this.ChoosePictures_btn = new System.Windows.Forms.Button();
            this.Pictures_rtf = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Upload_btn = new System.Windows.Forms.Button();
            this.CreateFolders_chk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // UserName_lbl
            // 
            this.UserName_lbl.AutoSize = true;
            this.UserName_lbl.Location = new System.Drawing.Point(12, 9);
            this.UserName_lbl.Name = "UserName_lbl";
            this.UserName_lbl.Size = new System.Drawing.Size(60, 13);
            this.UserName_lbl.TabIndex = 0;
            this.UserName_lbl.Text = "User Name";
            // 
            // Password_lbl
            // 
            this.Password_lbl.AutoSize = true;
            this.Password_lbl.Location = new System.Drawing.Point(12, 39);
            this.Password_lbl.Name = "Password_lbl";
            this.Password_lbl.Size = new System.Drawing.Size(53, 13);
            this.Password_lbl.TabIndex = 1;
            this.Password_lbl.Text = "Password";
            // 
            // UserName_box
            // 
            this.UserName_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default, "UserDefault", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserName_box.Location = new System.Drawing.Point(96, 9);
            this.UserName_box.Name = "UserName_box";
            this.UserName_box.Size = new System.Drawing.Size(166, 20);
            this.UserName_box.TabIndex = 2;
            this.UserName_box.Text = global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default.UserDefault;
            // 
            // Password_box
            // 
            this.Password_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default, "PasswordDefault", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Password_box.Location = new System.Drawing.Point(96, 36);
            this.Password_box.Name = "Password_box";
            this.Password_box.PasswordChar = '*';
            this.Password_box.Size = new System.Drawing.Size(166, 20);
            this.Password_box.TabIndex = 3;
            this.Password_box.Text = global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default.PasswordDefault;
            // 
            // Site_lbl
            // 
            this.Site_lbl.AutoSize = true;
            this.Site_lbl.Location = new System.Drawing.Point(321, 12);
            this.Site_lbl.Name = "Site_lbl";
            this.Site_lbl.Size = new System.Drawing.Size(25, 13);
            this.Site_lbl.TabIndex = 4;
            this.Site_lbl.Text = "Site";
            // 
            // Library_lbl
            // 
            this.Library_lbl.AutoSize = true;
            this.Library_lbl.Location = new System.Drawing.Point(321, 39);
            this.Library_lbl.Name = "Library_lbl";
            this.Library_lbl.Size = new System.Drawing.Size(38, 13);
            this.Library_lbl.TabIndex = 5;
            this.Library_lbl.Text = "Library";
            // 
            // Site_box
            // 
            this.Site_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Site_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default, "SiteDefault", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Site_box.Location = new System.Drawing.Point(365, 12);
            this.Site_box.Name = "Site_box";
            this.Site_box.Size = new System.Drawing.Size(389, 20);
            this.Site_box.TabIndex = 6;
            this.Site_box.Text = global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default.SiteDefault;
            // 
            // Library_box
            // 
            this.Library_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default, "LibraryDefault", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Library_box.Location = new System.Drawing.Point(365, 36);
            this.Library_box.Name = "Library_box";
            this.Library_box.Size = new System.Drawing.Size(148, 20);
            this.Library_box.TabIndex = 7;
            this.Library_box.Text = global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default.LibraryDefault;
            // 
            // Folder_lbl
            // 
            this.Folder_lbl.AutoSize = true;
            this.Folder_lbl.Location = new System.Drawing.Point(519, 39);
            this.Folder_lbl.Name = "Folder_lbl";
            this.Folder_lbl.Size = new System.Drawing.Size(36, 13);
            this.Folder_lbl.TabIndex = 8;
            this.Folder_lbl.Text = "Folder";
            // 
            // Folder_box
            // 
            this.Folder_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Folder_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default, "FolderDefault", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Folder_box.Location = new System.Drawing.Point(563, 39);
            this.Folder_box.Name = "Folder_box";
            this.Folder_box.Size = new System.Drawing.Size(191, 20);
            this.Folder_box.TabIndex = 9;
            this.Folder_box.Text = global::MyLocalBroadband.WSSPictureUploader.Properties.Settings.Default.FolderDefault;
            // 
            // ChoosePictures_btn
            // 
            this.ChoosePictures_btn.Location = new System.Drawing.Point(15, 68);
            this.ChoosePictures_btn.Name = "ChoosePictures_btn";
            this.ChoosePictures_btn.Size = new System.Drawing.Size(110, 23);
            this.ChoosePictures_btn.TabIndex = 10;
            this.ChoosePictures_btn.Text = "Choose Pictures";
            this.ChoosePictures_btn.UseVisualStyleBackColor = true;
            this.ChoosePictures_btn.Click += new System.EventHandler(this.ChoosePictures_btn_Click);
            // 
            // Pictures_rtf
            // 
            this.Pictures_rtf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Pictures_rtf.Location = new System.Drawing.Point(15, 98);
            this.Pictures_rtf.Name = "Pictures_rtf";
            this.Pictures_rtf.Size = new System.Drawing.Size(753, 404);
            this.Pictures_rtf.TabIndex = 11;
            this.Pictures_rtf.Text = resources.GetString("Pictures_rtf.Text");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // Upload_btn
            // 
            this.Upload_btn.Location = new System.Drawing.Point(203, 68);
            this.Upload_btn.Name = "Upload_btn";
            this.Upload_btn.Size = new System.Drawing.Size(75, 23);
            this.Upload_btn.TabIndex = 12;
            this.Upload_btn.Text = "Upload";
            this.Upload_btn.UseVisualStyleBackColor = true;
            this.Upload_btn.Click += new System.EventHandler(this.Upload_btn_Click);
            // 
            // CreateFolders_chk
            // 
            this.CreateFolders_chk.AutoSize = true;
            this.CreateFolders_chk.Location = new System.Drawing.Point(620, 68);
            this.CreateFolders_chk.Name = "CreateFolders_chk";
            this.CreateFolders_chk.Size = new System.Drawing.Size(94, 17);
            this.CreateFolders_chk.TabIndex = 13;
            this.CreateFolders_chk.Text = "Create Folders";
            this.CreateFolders_chk.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 504);
            this.Controls.Add(this.CreateFolders_chk);
            this.Controls.Add(this.Upload_btn);
            this.Controls.Add(this.Pictures_rtf);
            this.Controls.Add(this.ChoosePictures_btn);
            this.Controls.Add(this.Folder_box);
            this.Controls.Add(this.Folder_lbl);
            this.Controls.Add(this.Library_box);
            this.Controls.Add(this.Site_box);
            this.Controls.Add(this.Library_lbl);
            this.Controls.Add(this.Site_lbl);
            this.Controls.Add(this.Password_box);
            this.Controls.Add(this.UserName_box);
            this.Controls.Add(this.Password_lbl);
            this.Controls.Add(this.UserName_lbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "My Local Broadband LLC: Picture Uploader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserName_lbl;
        private System.Windows.Forms.Label Password_lbl;
        private System.Windows.Forms.TextBox UserName_box;
        private System.Windows.Forms.TextBox Password_box;
        private System.Windows.Forms.Label Site_lbl;
        private System.Windows.Forms.Label Library_lbl;
        private System.Windows.Forms.TextBox Site_box;
        private System.Windows.Forms.TextBox Library_box;
        private System.Windows.Forms.Label Folder_lbl;
        private System.Windows.Forms.TextBox Folder_box;
        private System.Windows.Forms.Button ChoosePictures_btn;
        private System.Windows.Forms.RichTextBox Pictures_rtf;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Upload_btn;
        private System.Windows.Forms.CheckBox CreateFolders_chk;
    }
}

