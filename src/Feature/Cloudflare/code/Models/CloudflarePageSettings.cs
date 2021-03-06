using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdImmunity2021.Feature.Cloudflare.Models
{
    public class CloudflarePageSettings
    {
        public CheckboxField fullyCachePageField;
        public Field cfPageRuleId;
        public Sitecore.Data.ID fullyCachePageFieldID = new Sitecore.Data.ID("{5116173A-016D-47C7-B250-D935CE4ACAF5}");
        public Sitecore.Data.ID cfPageRuleIdFieldID = new Sitecore.Data.ID("{D81E5834-A396-43B7-8C6D-5CAF061436B6}");

        public CloudflarePageSettings(Item item)
        {
            fullyCachePageField = item.Fields[fullyCachePageFieldID];
            cfPageRuleId = item.Fields[cfPageRuleIdFieldID];
        }
    }
}