using NerdImmunity2021.Feature.Cloudflare.Models;
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
            //ItemProcessedEventArgs itemProcessedEventArgs = args as ItemProcessedEventArgs;
            //PublishItemContext context = itemProcessedEventArgs != null ? itemProcessedEventArgs.Context : null;
            ////TODO: validate publish is against public database
            //Sitecore.Data.Items.Item target = context.PublishOptions.TargetDatabase.GetItem(context.ItemId,context.PublishOptions.Language);
            //if (target == null)
            //    return;
            //CloudflarePageSettings publishedPage = new CloudflarePageSettings(target);
            //if (publishedPage.fullyCachePageField.Checked)
            //{
            //    //clear cache
            //}
        }
    }
}