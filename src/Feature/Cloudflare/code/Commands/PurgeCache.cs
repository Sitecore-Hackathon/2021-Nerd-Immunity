using NerdImmunity2021.Feature.Cloudflare.Models;
using NerdImmunity2021.Feature.Cloudflare.Services;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using System.Linq;

namespace NerdImmunity2021.Feature.Cloudflare.Commands
{
    public class PurgeCacheCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            bool isCacheCleared = false;
            if (context.Items.Length == 1)
            {
                var siteList = Sitecore.Configuration.Factory.GetSiteInfoList();
                Item item = context.Items[0];
                SiteInfo currentSite = siteList.FirstOrDefault(info => item.Paths.FullPath.StartsWith(info.RootPath + info.StartItem, StringComparison.InvariantCultureIgnoreCase));

                Item CloudflareSiteSettingsParent = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/system/Modules/Cloudflare/Settings");
                foreach (Item MySettings in CloudflareSiteSettingsParent.Children)
                {
                    CloudflareSiteSettings MyCloudflareSiteSetting = new CloudflareSiteSettings(MySettings);
                    if (MyCloudflareSiteSetting != null)
                        if (MyCloudflareSiteSetting.SiteStartItem.Value.ToLower().Equals(currentSite.RootPath + currentSite.StartItem))
                        {
                            CloudflareService cfService = new CloudflareService();
                            isCacheCleared = cfService.ClearSiteCache(MyCloudflareSiteSetting);
                        }
                }

            }

            var response = "Cloudflare cache purge ";
            if (isCacheCleared)
            {
                response += "successful";
            } 
            else
            {
                response += "unsuccessful, please purge cache from the Cloudflare dashboard";
            }        
                                
            Sitecore.Context.ClientPage.ClientResponse.Alert(response);
        }

        protected void PurgeCache(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {

                args.WaitForPostBack(false);
            }

        }
    }
}