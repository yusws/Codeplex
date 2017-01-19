using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Administration;

namespace MyLocalBroadband.WebConfigManager
{
    public class WebConfigManager
    {
        #region Properties
        public string Name { get; set; }
        public string XPath { get; set; }
        public string Value { get; set; }
        public SPWebConfigModification.SPWebConfigModificationType ModificationType { get; set; }
        #endregion

        #region Constructors
        public WebConfigManager()
        { }
        public WebConfigManager(string name, string xpath, string value, SPWebConfigModification.SPWebConfigModificationType modificationType)
        {
            Name = name;
            XPath = xpath;
            Value = value;
            ModificationType = modificationType;
        }
        #endregion

        #region Public Methods
        public SPWebConfigModification GetModification(string owner, uint sequence)
        {
            SPWebConfigModification modification = new SPWebConfigModification(Name, XPath);
            modification.Owner = owner;
            modification.Sequence = sequence;
            modification.Type = ModificationType;
            modification.Value = Value;
            return modification;
        }
        #endregion

        #region Static Methods
        public static void AddConfigModifications(SPWebApplication webApp, string owner, List<WebConfigManager> modifications)
        {
            foreach (WebConfigManager mod in modifications)
            {
                webApp.WebConfigModifications.Add(mod.GetModification(owner, 0));
            }
            webApp.WebService.ApplyWebConfigModifications();
            webApp.Update();
        }
        public static List<SPWebConfigModification> RemoveConfiguration(SPWebApplication webApp, string owner)
        {
            List<SPWebConfigModification> removedModifications = new List<SPWebConfigModification>();

            // Remove any modifications that were originally created by the owner.
            foreach (SPWebConfigModification mod in webApp.WebConfigModifications)
            {
                //note while mod types of EnsureSection won't actually be removed from the web.config
                //we still want to take the action of removing them here so that they are removed
                //from the sharepoint collection of config modifications
                if ((mod.Owner == owner))
                {
                    removedModifications.Add(mod);
                }
            }
            foreach (SPWebConfigModification rem in removedModifications)
            {
                webApp.WebConfigModifications.Remove(rem);
            }
            webApp.WebService.ApplyWebConfigModifications();
            webApp.Update();

            return removedModifications;
        }
        #endregion
    }
}
