using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using MyLocalBroadband.Logging;
using MyLocalBroadband.WebConfigManager;

namespace MyLocalBroadband.WSSSecurityProvider
{
    class SecProviderFeatureReceiver : SPFeatureReceiver
    {
        #region Member Data
        private const string _MODIFICATIONOWNER = "MyLocalBroadband.WSSSecurityProvider.SecProviderFeatureReceiver";
        private const string _CONNECTIONSTRINGNAME = "SecProviderConnectionString";
        private const string _CONNECTIONSTRING = @"<add name=""SecProviderConnectionString"" connectionString=""data source=127.0.0.1;Integrated Security=SSPI;Initial Catalog=SecProviderDB"" />";
        private const string _MEMBERSHIPPROVIDERNAME = "SecMembershipProvider";
        private const string _ROLEPROVIDERNAME = "SecRoleProvider";
        public const string _FBAAPPLICATIONNAME = "SecProviderFormsAuth";
        #endregion

        #region Public Overrides
        /// <summary>
        /// When this feature is activated it adds connection string, membership
        /// provider, and role provider entries into the web.config for the 
        /// web application.
        /// 
        /// It also sets the web application's people picker to only search within 
        /// the current site collection
        /// </summary>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    WebConfigManager.WebConfigManager.AddConfigModifications(webApp, _MODIFICATIONOWNER, GetWebConfigMods());
                    webApp.PeoplePickerSettings.OnlySearchWithinSiteCollection = true;
                    ULS.LogMessage("SecProviderFeatureReceiver.FeatureActivated", "Feature Activated on web app: " + webApp.Name, "Membership", TraceProvider.TraceSeverity.InformationEvent);
                }
                else
                {
                    ULS.LogMessage("SecProviderFeatureReceiver.FeatureActivated", "Not able to retrieve SPWebApplication", "Membership", TraceProvider.TraceSeverity.CriticalEvent);
                }
            }
            catch (Exception ex)
            {
                ULS.LogError("SecProviderFeatureReceiver.FeatureActivated", ex);
                throw ex;
            }
        }
        /// <summary>
        /// When this feature is deactivated, it removes the connection string,
        /// membership provider, and role provider information from the web.config
        /// for the web application.  The People picker setting for the web app
        /// is left as is because we don't know what the correct state would be.
        /// </summary>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    WebConfigManager.WebConfigManager.RemoveConfiguration(webApp, _MODIFICATIONOWNER);
                    ULS.LogMessage("SecProviderFeatureReceiver.FeatureDeactivating", "Feature Deactivated on web app: " + webApp.Name, "Membership", TraceProvider.TraceSeverity.InformationEvent);
                }
                else
                {
                    ULS.LogMessage("SecProviderFeatureReceiver.FeatureDeactivating", "Not able to retrieve SPWebApplication", "Membership", TraceProvider.TraceSeverity.CriticalEvent);
                }
            }
            catch (Exception ex)
            {
                ULS.LogError("SecProviderFeatureReceiver.FeatureDeactivating", ex);
                throw ex;
            }
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            // No Implementation
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            // No Implementation
        }
        #endregion

        #region Private Methods
        private List<WebConfigManager.WebConfigManager> GetWebConfigMods()
        {
            List<WebConfigManager.WebConfigManager> modifications = new List<WebConfigManager.WebConfigManager>();
            WebConfigManager.WebConfigManager mod = new WebConfigManager.WebConfigManager();
            mod.Name = "connectionStrings";
            mod.XPath = "configuration";
            mod.Value = "connectionStrings";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "add[@name='" + _CONNECTIONSTRINGNAME + "']";
            mod.XPath = "configuration/connectionStrings";
            mod.Value = _CONNECTIONSTRING;
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "membership";
            mod.XPath = "configuration/system.web";
            mod.Value = "membership";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "defaultProvider";
            mod.XPath = "configuration/system.web/membership";
            mod.Value = _MEMBERSHIPPROVIDERNAME;
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "providers";
            mod.XPath = "configuration/system.web/membership";
            mod.Value = "providers";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "add[@name='" + _MEMBERSHIPPROVIDERNAME + "']";
            mod.XPath = "configuration/system.web/membership/providers";
            mod.Value = "<add name=\"" + _MEMBERSHIPPROVIDERNAME + "\" connectionStringName=\"" + _CONNECTIONSTRINGNAME + "\" enablePasswordRetrieval=\"false\" enablePasswordReset=\"true\" requiresQuestionAndAnswer=\"false\" applicationName=\"" + _FBAAPPLICATIONNAME + "\" requiresUniqueEmail=\"false\" passwordFormat=\"Hashed\" maxInvalidPasswordAttempts=\"5\" minRequiredPasswordLength=\"1\" minRequiredNonalphanumericCharacters=\"0\" passwordAttemptWindow=\"10\" passwordStrengthRegularExpression=\"\" type=\"MyLocalBroadband.WSSSecurityProvider.SqlSiteMembershipProvider, MyLocalBroadband.WSSSecurityProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=af1a525c93de384c\"/>";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "roleManager";
            mod.XPath = "configuration/system.web";
            mod.Value = "roleManager";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "defaultProvider";
            mod.XPath = "configuration/system.web/roleManager";
            mod.Value = _ROLEPROVIDERNAME;
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "enabled";
            mod.XPath = "configuration/system.web/roleManager";
            mod.Value = "true";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "providers";
            mod.XPath = "configuration/system.web/roleManager";
            mod.Value = "providers";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "add[@name='" + _ROLEPROVIDERNAME + "']";
            mod.XPath = "configuration/system.web/roleManager/providers";
            mod.Value = "<add name=\"" + _ROLEPROVIDERNAME + "\" connectionStringName=\"" + _CONNECTIONSTRINGNAME + "\" applicationName=\"" + _FBAAPPLICATIONNAME + "\" type=\"MyLocalBroadband.WSSSecurityProvider.SqlSiteRoleProvider, MyLocalBroadband.WSSSecurityProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=af1a525c93de384c\"/>";
            modifications.Add(mod);

            return modifications;
        }

        private void SetFormsAuthentication(SPWebApplication webApp)
        {
            /* Originally this method was called at the end of the feature activated
             * handler.  However, I found two problems.  One) You don't
             * absolutely know which zone the user wants to enable FBA on.
             * Two) enabling FBA in this manner did not save the web.config change
             * to the <authentication mode="Forms"> tag
             */
            webApp.IisSettings[SPUrlZone.Internet].AuthenticationMode = System.Web.Configuration.AuthenticationMode.Forms;
            webApp.IisSettings[SPUrlZone.Internet].AllowAnonymous = true;
            webApp.IisSettings[SPUrlZone.Internet].MembershipProvider = _MEMBERSHIPPROVIDERNAME;
            webApp.IisSettings[SPUrlZone.Internet].RoleManager = _ROLEPROVIDERNAME;
            webApp.IisSettings[SPUrlZone.Internet].EnableClientIntegration = true;
            webApp.Update();
        }
        #endregion
    }
}
