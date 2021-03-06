using NerdImmunity2021.Feature.Cloudflare.Models;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Services
{
    public class CloudflareService
    {

        private string AuthHeader => "b3152693c5b0c6a7bec3811d1486630d3f48e";
        private string ContentTypeHeader => "application/json";
        private string BaseApiUrl => "https://api.cloudflare.com/client/v4/zones/";
        private string JsonContentType => "application/json";
        private JsonSerializerSettings SerialSettings => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public string AddPageRule(string SiteRoot, string PagePath)
        {
            //get site from item
            string ZoneID = "";
            string TargetDomain = "";
            Item CloudflareSiteSettingsParent = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/system/Modules/Cloudflare/Settings");
            foreach (Item MySettings in CloudflareSiteSettingsParent.Children)
            {
                CloudflareSiteSettings MyCloudflareSiteSetting = new CloudflareSiteSettings(MySettings);
                if (MyCloudflareSiteSetting != null)
                    if (MyCloudflareSiteSetting.SiteStartItem.Value.ToLower().Equals(SiteRoot.ToLower()))
                    {
                        ZoneID = MyCloudflareSiteSetting.CFZoneID.Value;
                        TargetDomain = MyCloudflareSiteSetting.DeliveryTargetHostname.Value;
                    }
            }
            //call Cloudflare for the given site
            string url = $"{BaseApiUrl}{ZoneID}/pagerules";
            using (HttpClient CfClient = new HttpClient())
            {
                CfClient.DefaultRequestHeaders.Add("X-Auth-Key", AuthHeader);
                CfClient.DefaultRequestHeaders.Add("X-Auth-Email", "Corey.Caplette@gmail.com");
                //var param = new PageRuleApiModel
                //{
                //    url = CfCommands[i].Substring(0, CfCommands[i].IndexOf('|'))
                //};
                var serialContent = "{\"targets\":[{\"target\": \"url\",\"constraint\": {\"operator\": \"matches\",\"value\": \"" + TargetDomain + "/" + PagePath + "\"}}].\"actions\":[{\"id\":\"always_online\",\"value\":\"on\"}]";
                    //JsonConvert.SerializeObject(param, SerialSettings);
                var paramContent = new StringContent(serialContent, Encoding.UTF8, JsonContentType);
                using (var res = Task.Run(() => CfClient.PostAsync(url, paramContent)))
                {
                    var content = Task.Run(() => res.Result.Content.ReadAsStringAsync()).Result;
                    //var response = JsonConvert.DeserializeObject<T>(content);
                }
            }
                return "RULE ADDED";
        }

        public bool RemovePageRule(CloudflarePageSettings pageItem)
        {
            return true;
        }

        public bool ClearCache(List<string> PagesToPurge)
        {
            //get all cloudflare site settings
            List<CloudflareSiteSettings> AllCloudflareSiteSettings = new List<CloudflareSiteSettings>();
            Item CloudflareSiteSettingsParent = Sitecore.Configuration.Factory.GetDatabase("master").GetItem("/sitecore/system/Modules/Cloudflare/Settings");
            foreach (Item MySettings in CloudflareSiteSettingsParent.Children)
            {
                CloudflareSiteSettings MyCloudflareSiteSetting = new CloudflareSiteSettings(MySettings);
                if (MyCloudflareSiteSetting != null)
                    AllCloudflareSiteSettings.Add(MyCloudflareSiteSetting);
            }
            int NumOfSites = AllCloudflareSiteSettings.Count;
            string[] CfCommands = new string[NumOfSites];
            foreach (string PageToPurge in PagesToPurge)
            {
                string[] PageInfo = PageToPurge.Split('|');
                if (PageInfo.Length != 2)
                    Log.Error("Invalid info sent to Cloudflare cache purge on publish.", this);
                else
                {
                    if (PageInfo[0].Equals("ALL"))
                    {
                        for (int i = 0; i < NumOfSites; i++)
                        {
                            CfCommands[i] += AllCloudflareSiteSettings[i].DeliveryTargetHostname.Value + PageInfo[1] + "|";
                        }
                        Log.Audit("Cloudflare cache purge for media item " + PageInfo[1] + " (for all sites) queued.", this);
                        continue;
                    }
                    for (int i = 0; i < NumOfSites; i++)
                    {
                        if (AllCloudflareSiteSettings[i].SiteStartItem.Value.ToLower() == PageInfo[0].ToLower())
                        {
                            CfCommands[i] += AllCloudflareSiteSettings[i].DeliveryTargetHostname.Value + PageInfo[i] + "|";
                            Log.Audit("Cloudflare cache purge for " + PageInfo[1] + " (for site " + PageInfo[0] + ") queued.", this);
                        }
                    }
                }
            }
            for (int i = 0; i < NumOfSites; i++)
            {
                if (CfCommands[i] != null)
                {
                    //remove last | from string
                    CfCommands[i] = CfCommands[i].Substring(0, CfCommands[i].Length - 1);
                    //call Cloudflare for the given site
                    string url = BaseApiUrl + AllCloudflareSiteSettings[i].CFZoneID.Value + "/purge_cache";
                    using (HttpClient CfClient = new HttpClient())
                    {
                        CfClient.DefaultRequestHeaders.Add("X-Auth-Key", AuthHeader);
                        CfClient.DefaultRequestHeaders.Add("X-Auth-Email", "Corey.Caplette@gmail.com");
                        var param = new UrlApiModel
                        {
                            files = CfCommands[i].Split('|')
                        };
                        var serialContent = JsonConvert.SerializeObject(param, SerialSettings);
                        var paramContent = new StringContent(serialContent, Encoding.UTF8, JsonContentType);
                        using (var res = Task.Run(() => CfClient.PostAsync(url, paramContent)))
                        {
                            var content = Task.Run(() => res.Result.Content.ReadAsStringAsync()).Result;
                            //var response = JsonConvert.DeserializeObject<T>(content);
                        }
                    }
                }
            }
            return true;
        }
    }
    public class UrlApiModel
    {
        public string[] files { get; set; }
    }
    public class PageRuleApiModel
    {

    }
}