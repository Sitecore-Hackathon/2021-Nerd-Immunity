using NerdImmunity2021.Feature.Cloudflare.Models;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Services
{
    public class CloudflareService
    {
        public string AddPageRule(CloudflarePageSettings pageItem)
        {
            return "RULE ADDED";
        }

        public bool RemovePageRule(CloudflarePageSettings pageItem)
        {
            return true;
        }

        public bool ClearCache(List<string> PagesToPurge)
        {
            foreach (string PageToPurge in PagesToPurge)
            {
                string[] PageInfo = PageToPurge.Split('|');
                if (PageInfo.Length != 2)
                    Log.Error("Invalid info sent to Cloudflare cache purge on publish.", this);
                else
                {
                    if (PageInfo[0].Equals("ALL"))
                    {
                        Log.Audit("Cloudflare cache purge for media item " + PageInfo[1] + " (for all sites) queued.", this);
                        continue;
                    }
                    Log.Audit("Cloudflare cache purge for " + PageInfo[1] + " (for site " + PageInfo[0] + ") queued.", this);
                }
            }
            return true;
        }
    }
}