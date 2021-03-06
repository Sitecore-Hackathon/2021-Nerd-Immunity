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
                //if (itemChanges == null || !itemChanges.IsFieldModified("{5116173A-016D-47C7-B250-D935CE4ACAF5}"))
                    //return;
                //the field for caching was update in this save so we need to check page rules in cloudflare
                //var cacheSettings = savedItem.Fields["{5116173A-016D-47C7-B250-D935CE4ACAF5}"].Value;
                //if (cacheSettings)
                    //AddPageRule(savedItem);
                //else
                    //RemovePageRule(savedItem);
            }
            catch (Exception ex)
            {
                Log.Error("Error while saving item during check for Cloudflare cache clear.", ex, this);
            }
        }
    }
}