using NerdImmunity2021.Feature.Cloudflare.Models;
using NerdImmunity2021.Feature.Cloudflare.Services;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Pipelines
{
    public class CloudflareItemSaving
    {
        public void OnItemSaving(object sender, EventArgs args)
        {
            try
            {
                var savedItem = Event.ExtractParameter(args, 0) as Item;
                if (savedItem == null)
                    return;
                //if the item being saved doesn't have presentation, it doesn't qualify for this rule
                if (savedItem.Fields[Sitecore.FieldIDs.LayoutField] == null || String.IsNullOrEmpty(savedItem.Fields[Sitecore.FieldIDs.LayoutField].Value))
                    return;
                //if the item doesn't inherit the Cloudflare Page Settings, it doesn't qualify for this rule
                CloudflarePageSettings savedCFsettings = new CloudflarePageSettings(savedItem);
                if (savedCFsettings == null)
                    return;
                var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;
                if (itemChanges == null || !itemChanges.IsFieldModified(savedCFsettings.fullyCachePageFieldID))
                    return;
                //the field for caching was update in this save so we need to check page rules in cloudflare and update item
                if (savedCFsettings.fullyCachePageField == null)
                    return;
                CloudflareService cfService = new CloudflareService();
                if (savedCFsettings.fullyCachePageField.Checked)
                {
                    string CFpageRuleId = cfService.AddPageRule(savedItem);
                    if (string.IsNullOrEmpty(CFpageRuleId))
                    {
                        //something went wrong, cancel the save and alert the user
                        cancelSaveWithPrompt(args);
                        return;
                    }
                    //update savedItem with page rule id
                    savedItem.Fields[savedCFsettings.cfPageRuleIdFieldID].Value = CFpageRuleId;
                }
                else
                {
                    bool success = cfService.RemovePageRule(savedItem);
                    if (!success)
                    {
                        //something went wrong, cancel the save and alert the user
                        cancelSaveWithPrompt(args);
                        return;
                    }
                    //clear the page rule ID from the item
                    savedItem.Fields[savedCFsettings.cfPageRuleIdFieldID].Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while saving item during check for Cloudflare cache clear.", ex, this);
            }
        }

        private void cancelSaveWithPrompt(EventArgs args)
        {
            SitecoreEventArgs evt = args as SitecoreEventArgs;
            evt.Result.Cancel = true;
            Sitecore.Context.ClientPage.ClientResponse.Alert("Connection to Cloudflare failed - please try again.");
        }
    }
}