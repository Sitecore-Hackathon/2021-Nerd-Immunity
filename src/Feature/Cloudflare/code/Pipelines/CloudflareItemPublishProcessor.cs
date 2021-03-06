using NerdImmunity2021.Feature.Cloudflare.Models;
using NerdImmunity2021.Feature.Cloudflare.Services;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Publishing.Pipelines.Publish;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Pipelines
{
    public class CloudflareItemPublishProcessor : PublishProcessor
    {
        public override void Process(PublishContext context)
        {
            try
            {
                Assert.ArgumentNotNull(context, "context");
                if (context.Aborted)
                    return;
                //Fetches List of Processed(Created/Updated/Deleted) Items
                var processedItems = context.ProcessedPublishingCandidates.Keys
                        .Select(i => context.PublishOptions.TargetDatabase.GetItem(i.ItemId)).Where(j => j != null);
                List<string> PagesToPurge = new List<string>();
                foreach (Item publishedItem in processedItems)
                {
                    //if the item being published doesn't have presentation, it doesn't qualify for this rule
                    if (publishedItem.Fields[Sitecore.FieldIDs.LayoutField] == null || String.IsNullOrEmpty(publishedItem.Fields[Sitecore.FieldIDs.LayoutField].Value))
                        continue;
                    CloudflarePageSettings publishedPage = new CloudflarePageSettings(publishedItem);
                    if (publishedPage.fullyCachePageField.Checked)
                    {

                        var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

                        SiteInfo currentSiteinfo = null;
                        var matchLength = 0;
                        foreach (var siteInfo in siteInfoList)
                        {
                            if (publishedItem.Paths.FullPath.StartsWith(siteInfo.RootPath, StringComparison.OrdinalIgnoreCase) && siteInfo.RootPath.Length > matchLength)
                            {
                                matchLength = siteInfo.RootPath.Length;
                                currentSiteinfo = siteInfo;
                            }
                        }
                        if (currentSiteinfo != null)
                        {
                            string PageSiteStartPath = currentSiteinfo.RootPath + currentSiteinfo.StartItem;
                            string RelativeUrl = Sitecore.Links.LinkManager.GetItemUrl(publishedItem);
                            PagesToPurge.Add(PageSiteStartPath + "|" + RelativeUrl);
                        }
                    }
                }
                //clear cache
                CloudflareService cfService = new CloudflareService();
                bool success = cfService.ClearCache(PagesToPurge);
                if (!success)
                {
                    Sitecore.Context.ClientPage.ClientResponse.Alert("Purge of Cloudflare cache failed - you may wish to manually clear the cache.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while publishing item when clearing Cloudflare cache.", ex, this);
            }
        }
    }
}