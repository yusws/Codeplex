using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using MyLocalBroadband.WebConfigManager;
using MyLocalBroadband.Logging;

namespace MyLocalBroadband.WSSSecurityProvider.WebServices
{
    class SecProviderWSFeatureReceiver : SPFeatureReceiver
    {
        #region Member Data
        private const string _MODIFICATIONOWNER = "SecurityProvider.SecProviderWSFeatureReceiver";
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
        /// web application.  It also sets a path to the hosts file where
        /// local DNS entries are made. 
        /// </summary>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    WebConfigManager.WebConfigManager.AddConfigModifications(webApp, _MODIFICATIONOWNER, GetWebConfigMods());
                    ULS.LogMessage("SecurityProviderWSFeatureReceiver.FeatureActivated", "Feature Activated on web app: " + webApp.Name, "Membership", TraceProvider.TraceSeverity.InformationEvent);
                }
                else
                {
                    ULS.LogMessage("SecurityProviderWSFeatureReceiver.FeatureActivated", "Not able to retrieve SPWebApplication", "Membership", TraceProvider.TraceSeverity.CriticalEvent);
                }
            }
            catch (Exception ex)
            {
                ULS.LogError("SecurityProviderWSFeatureReceiver.FeatureActivated", ex);
                throw ex;
            }
        }

        /// <summary>
        /// When this feature is deactivated, it removes the connection string,
        /// membership provider, and role provider information from the web.config
        /// for the web application.  The path to the Hosts file is also removed
        /// from the web.config.
        /// </summary>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    WebConfigManager.WebConfigManager.RemoveConfiguration(webApp, _MODIFICATIONOWNER);
                    ULS.LogMessage("SecurityProviderWSFeatureReceiver.FeatureDeactivating", "Feature Deactivated on web app: " + webApp.Name, "Membership", TraceProvider.TraceSeverity.InformationEvent);
                }
                else
                {
                    ULS.LogMessage("SecurityProviderWSFeatureReceiver.FeatureDeactivating", "Not able to retrieve SPWebApplication", "Membership", TraceProvider.TraceSeverity.CriticalEvent);
                }
            }
            catch (Exception ex)
            {
                ULS.LogError("SecurityProviderWSFeatureReceiver.FeatureDeactivating", ex);
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
            mod.Value = "<add name=\"" + _MEMBERSHIPPROVIDERNAME + "\" connectionStringName=\"" + _CONNECTIONSTRINGNAME + "\" enablePasswordRetrieval=\"false\" enablePasswordReset=\"true\" requiresQuestionAndAnswer=\"false\" applicationName=\"" + _FBAAPPLICATIONNAME + "\" requiresUniqueEmail=\"false\" passwordFormat=\"Hashed\" maxInvalidPasswordAttempts=\"5\" minRequiredPasswordLength=\"1\" minRequiredNonalphanumericCharacters=\"0\" passwordAttemptWindow=\"10\" passwordStrengthRegularExpression=\"\" type=\"System.Web.Security.SqlMembershipProvider\"/>";
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
            mod.Name = "providers";
            mod.XPath = "configuration/system.web/roleManager";
            mod.Value = "providers";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "add[@name='" + _ROLEPROVIDERNAME + "']";
            mod.XPath = "configuration/system.web/roleManager/providers";
            mod.Value = "<add name=\"" + _ROLEPROVIDERNAME + "\" connectionStringName=\"" + _CONNECTIONSTRINGNAME + "\" applicationName=\"" + _FBAAPPLICATIONNAME + "\" type=\"System.Web.Security.SqlRoleProvider\"/>";
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "appSettings";
            mod.XPath = "configuration";
            mod.Value = "appSettings";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager.WebConfigManager();
            mod.Name = "add[@key='HostsFile']";
            mod.XPath = "configuration/appSettings";
            mod.Value = @"<add key=""HostsFile"" value=""C:\Windows\system32\drivers\etc\hosts""/>";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            modifications.Add(mod);

            return modifications;
        }
        #endregion
    }
}
