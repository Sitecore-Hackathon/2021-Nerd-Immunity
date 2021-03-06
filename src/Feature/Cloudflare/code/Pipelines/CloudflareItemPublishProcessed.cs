using NerdImmunity2021.Feature.Cloudflare.Models;
using NerdImmunity2021.Feature.Cloudflare.Services;
using Sitecore.Diagnostics;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Pipelines
{
    public class CloudflareItemPublishProcessed 
    {
        public void ItemProcessed(object sender, EventArgs args)
        {
            try
            {
                ItemProcessedEventArgs itemProcessedEventArgs = args as ItemProcessedEventArgs;
                PublishItemContext context = itemProcessedEventArgs != null ? itemProcessedEventArgs.Context : null;
                //TODO: validate publish is against public database
                Sitecore.Data.Items.Item publishedItem = context.PublishOptions.TargetDatabase.GetItem(context.ItemId, context.PublishOptions.Language);
                if (publishedItem == null)
                    return;
                //if the item being saved doesn't have presentation, it doesn't qualify for this rule
                if (publishedItem.Fields[Sitecore.FieldIDs.LayoutField] == null || String.IsNullOrEmpty(publishedItem.Fields[Sitecore.FieldIDs.LayoutField].Value))
                    return;
                CloudflarePageSettings publishedPage = new CloudflarePageSettings(publishedItem);
                if (publishedPage.fullyCachePageField.Checked)
                {
                    //clear cache
                    CloudflareService cfService = new CloudflareService();
                    bool success = cfService.ClearCache(publishedPage);
                    Log.Audit("Cloudflare cache purge for " + publishedItem.ID + " (" + publishedItem.Name + ") completed: " + success.ToString(), this);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while saving item during check for Cloudflare cache clear.", ex, this);
            }
        }
    }
}