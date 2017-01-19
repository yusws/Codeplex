using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace MyLocalBroadband.WebConfigManager
{
    class ExampleUsageFeatureReceiver : SPFeatureReceiver
    {
        #region Member Data
        private const string _MODIFICATIONOWNER = "SecurityProvider.SecProviderFeatureReceiver";
        private const string _CONNECTIONSTRINGNAME = "SecProviderConnectionString";
        private const string _CONNECTIONSTRING = @"<add name=""SecProviderConnectionString"" connectionString=""data source=127.0.0.1;Integrated Security=SSPI;Initial Catalog=SecProviderDB"" />";
        #endregion

        #region Public Overrides
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                WebConfigManager.AddConfigModifications(webApp, _MODIFICATIONOWNER, GetWebConfigMods());
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                WebConfigManager.RemoveConfiguration(webApp, _MODIFICATIONOWNER);
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
        private List<WebConfigManager> GetWebConfigMods()
        {
            List<WebConfigManager> modifications = new List<WebConfigManager>();
            WebConfigManager mod = new WebConfigManager();
            mod.Name = "connectionStrings";
            mod.XPath = "configuration";
            mod.Value = "connectionStrings";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureSection;
            modifications.Add(mod);

            mod = new WebConfigManager();
            mod.Name = "add[@name='" + _CONNECTIONSTRINGNAME + "']";
            mod.XPath = "configuration/connectionStrings";
            mod.Value = _CONNECTIONSTRING;
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            modifications.Add(mod);
            return modifications;
        }
        #endregion
    }
}
