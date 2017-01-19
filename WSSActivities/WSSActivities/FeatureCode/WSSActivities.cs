using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using MyLocalBroadband.WebConfigManager;

namespace MyLocalBroadband.Activities.WSS
{
    class WSSActivities : SPFeatureReceiver
    {
        #region Member Data
        private const string _MODIFICATIONOWNER = "MyLocalBroadband.Activities.WSS.WSSActivities";
        private const string _ASSEMBLY = @"MyLocalBroadband.Activities.WSS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2621e188d62293e";
        #endregion

        #region Public Overrides
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                WebConfigManager.WebConfigManager.AddConfigModifications(webApp, _MODIFICATIONOWNER, GetWebConfigMods());
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            if (webApp != null)
            {
                WebConfigManager.WebConfigManager.RemoveConfiguration(webApp, _MODIFICATIONOWNER);
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
            mod.Name = "add[@Assembly='" + _ASSEMBLY + "']";
            mod.XPath = "configuration/System.Workflow.ComponentModel.WorkflowCompiler/authorizedTypes";
            mod.Value = @"<authorizedType Assembly=""" + _ASSEMBLY + @""" Namespace=""MyLocalBroadband.Activities.WSS"" TypeName=""*"" Authorized=""True"" />";
            mod.ModificationType = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            modifications.Add(mod);
            return modifications;
        }
        #endregion
    }
}
