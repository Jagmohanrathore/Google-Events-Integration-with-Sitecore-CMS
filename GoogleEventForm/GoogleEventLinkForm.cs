using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.WebControls;
using Sitecore.Xml;
using System;
using System.Xml;
using Sitecore.Shell.Web.UI;
using Sitecore.Web.UI.Sheer;
using Sitecore.Controls;
using Sitecore.Web.UI.XamlSharp.Xaml;
using Sitecore.Globalization;
using System.Text.RegularExpressions;
using GoogleEventTracking.Global;
using GoogleEventTracking.GlobalFiles;


namespace Oasis.GoogleEventTracking.GoogleEventForm
{
    public class GoogleEventLinkForm : DialogForm
    {
        protected Edit Category;
        protected Edit Action;
        protected Edit Label1;
        protected DataContext GoogleLinkDataContext;
        protected Edit Value1;
        protected Checkbox NonInteraction;
        Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (Sitecore.Context.ClientPage.IsEvent)
            {
                return;
            }

            this.GoogleLinkDataContext.GetFromQueryString();
            string xml = StringUtil.GetString(new string[] { WebUtil.GetQueryString("va"), "<link/>" });
            string queryString = WebUtil.GetQueryString("ro");
            XmlDocument document = XmlUtil.LoadXml(xml);
            System.Xml.XmlNode node = document.SelectSingleNode("/link");
            string attribute = XmlUtil.GetAttribute("category", node);
            string str4 = XmlUtil.GetAttribute("action", node);
            string str5 = XmlUtil.GetAttribute("label1", node);
            string str6 = string.Empty;
            string str7 = XmlUtil.GetAttribute("value1", node);
            string str8 = XmlUtil.GetAttribute("noninteraction", node);

            this.Category.Value = attribute;
            this.Action.Value = str4;
            this.Label1.Value = str5;
            this.Value1.Value = str7;
            this.NonInteraction.Value = str8;
            string str11 = XmlUtil.GetAttribute("id", node);

        }

        /// <summary>
        /// Handler for ok button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnOK(object sender, EventArgs args)
        {

            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            this.GoogleLinkDataContext.GetFromQueryString();
            string xml = StringUtil.GetString(new string[] { WebUtil.GetQueryString("va"), "<link/>" });
            string queryString = WebUtil.GetQueryString("ro");
            XmlDocument document = XmlUtil.LoadXml(xml);
            System.Xml.XmlNode node = document.SelectSingleNode("/link");
            Packet packet = new Packet(document);
            SetAttribute(packet, "category", this.Category.Value);
            SetAttribute(packet, "action", this.Action.Value);
            SetAttribute(packet, "label1", this.Label1.Value);
            SetAttribute(packet, "value1", this.Value1.Value);
            SetAttribute(packet, "noninteraction", this.NonInteraction.Value);

            if (!string.IsNullOrEmpty(this.Category.Value) && !string.IsNullOrEmpty(this.Action.Value))
            {

                string onClickValues = "_gaq.push(['_trackEvent',$EventDetail$]);";
                string strParameters = null;
                strParameters = "'" + this.Category.Value + "'";
                strParameters = strParameters + "," + "'" + this.Action.Value + "'";
                if (!string.IsNullOrEmpty(this.Label1.Value))
                {
                    strParameters = strParameters + "," + "'" + this.Label1.Value + "'";
                }

                if (!string.IsNullOrEmpty(this.Value1.Value))
                {
                    strParameters = strParameters + "," + this.Value1.Value;
                }

                if (!string.IsNullOrEmpty(this.NonInteraction.Value) && this.NonInteraction.Value == "1")
                {
                    strParameters = strParameters + "," + "'true'";
                }

                onClickValues = onClickValues.Replace("$EventDetail$", strParameters);
                string finalValues = onClickValues;
                SetAttribute(packet, "onclick", finalValues);
            }
            Sitecore.Context.ClientPage.ClientResponse.SetDialogValue(packet.OuterXml);

            base.OnOK(sender, args);
        }

        /// <summary>
        ///  set attribute by packet for control
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="name"></param>
        /// <param name="control"></param>
        private static void SetAttribute(Packet packet, string name, Control control)
        {
            Assert.ArgumentNotNull(packet, "packet");
            Assert.ArgumentNotNullOrEmpty(name, "name");
            Assert.ArgumentNotNull(control, "control");
            if (control.Value.Length > 0)
            {
                SetAttribute(packet, name, control.Value);
            }
        }

        /// <summary>
        /// set attribute name and value of controls
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetAttribute(Packet packet, string name, string value)
        {
            Assert.ArgumentNotNull(packet, "packet");
            Assert.ArgumentNotNullOrEmpty(name, "name");
            Assert.ArgumentNotNull(value, "value");
            packet.SetAttribute(name, value);
        }


    }
}
