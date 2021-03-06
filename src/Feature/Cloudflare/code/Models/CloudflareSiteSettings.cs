using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Models
{
    public class CloudflareSiteSettings
    {
        public DatasourceField SiteStartItem;
        public Field DeliveryTargetHostname;
        public Field CFToken;
        public Field CFZoneID;
        public Sitecore.Data.ID SiteStartItemFieldID = new Sitecore.Data.ID("{C62ABB42-B73B-47B8-8422-D2C1CFEB0B23}");
        public Sitecore.Data.ID DeliveryTargetHostnameFieldID = new Sitecore.Data.ID("{028D2A8B-F42F-4B3E-8B96-A1336A633A9F}");
        public Sitecore.Data.ID CFTokenFieldID = new Sitecore.Data.ID("{DF80E6F4-CA63-40FD-95E5-2ADC5B995A47}");
        public Sitecore.Data.ID CFZoneIDFieldID = new Sitecore.Data.ID("{6DFB971D-3517-4CEA-A864-F4CADD369AF3}");

        public CloudflareSiteSettings(Item item)
        {
            SiteStartItem = item.Fields[SiteStartItemFieldID];
            DeliveryTargetHostname = item.Fields[DeliveryTargetHostnameFieldID];
            CFToken = item.Fields[CFTokenFieldID];
            CFZoneID = item.Fields[CFZoneIDFieldID];
        }
    }
}