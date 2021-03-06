using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflare.Pipelines
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
                var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;
                if (itemChanges == null || !itemChanges.IsFieldModified(new Sitecore.Data.ID("{5116173A-016D-47C7-B250-D935CE4ACAF5}")))
                    return;
                //the field for caching was update in this save so we need to check page rules in cloudflare and update item
                Sitecore.Data.Fields.CheckboxField cacheCheckboxField = savedItem.Fields["{5116173A-016D-47C7-B250-D935CE4ACAF5}"];
                if (cacheCheckboxField == null)
                    return;
                if (cacheCheckboxField.Checked)
                    return; 
                // string CFpageRuleId = CFUtility.AddPageRule(savedItem);
                // if successful, set the page rule ID on the item
                // savedItem.Update
                // if not successful - alert the user
                else
                    return; 
                // bool success = RemovePageRule(savedItem);
                // if successful, clear the page rule ID from the item
                // savedItem.Update
                // if not successful, alert the user
            }
            catch (Exception ex)
            {
                Log.Error("Error while saving item during check for Cloudflare cache clear.", ex, this);
            }
        }
    }
}