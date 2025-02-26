﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Application.Utils
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}