﻿using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflare.Services
{
    public class CloudflareService
    {
        public string AddPageRule(Item pageItem)
        {
            return "RULE ADDED";
        }

        public bool RemovePageRule(Item pageItem)
        {
            return true;
        }
    }
}