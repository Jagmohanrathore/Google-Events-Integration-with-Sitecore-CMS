using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Xml.Xsl;
using Sitecore.Data.Items;
using Sitecore.Pipelines.RenderField;


namespace Oasis.GoogleEventTracking
{
  public class CustomGetLinkFieldValue : GetLinkFieldValue
  {
    protected override LinkRenderer CreateRenderer(Item item)
    {
      return new CustomLinkRenderer(item);
    }   
  }
}